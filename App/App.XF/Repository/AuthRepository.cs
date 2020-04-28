using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using App.Web;
using App.Web.Rest.Model.Auth;
using App.XF.DI.DependencyService;
using App.XF.Service;
using App.XF.Utils.Rx;

namespace App.XF.Repository
{
    /// <summary>
    /// Repository handling authorization and current user info 
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly IRestServices _restServices;
        private readonly IAuthService _authService;
        private readonly IDeviceLog _log;
        private readonly IRxSchedulersFacade _rxSchedulersFacade;
        private readonly IConnectionStatusRepository _connectionStatusRepository;

        public BehaviorSubject<LoginRequestStatus> LoginRequestStatusSubject { get; } =
            new BehaviorSubject<LoginRequestStatus>(LoginRequestStatus.Idle);
        public BehaviorSubject<AuthStatusEnum> AuthStatus { get; } =
            new BehaviorSubject<AuthStatusEnum>(AuthStatusEnum.Idle);

        public AuthRepository(IRestServices restServices, IAuthService authService,
            IRxSchedulersFacade rxSchedulersFacade, 
            IConnectionStatusRepository connectionStatusRepository, IDeviceLog log)
        {
            _restServices = restServices;
            _authService = authService;
            _rxSchedulersFacade = rxSchedulersFacade;
            _connectionStatusRepository = connectionStatusRepository;
            _log = log;
        }

        /// <summary>
        /// Check if user has stored session token - forward call to IAuthService.IsLoggedIn()
        /// </summary>
        /// <returns>true if token available</returns>
        public bool IsLoggedIn()
        {
            if (_authService.IsLoggedIn())
            {
                _log.Info("AuthRepository","user logged in");
                return true;
            }
            else
            {
                _log.Info("AuthRepository","user not logged in");
                return false;
            }
        }

        /// <summary>
        /// Makes PostLogin call to Services - get session token 
        /// </summary>
        public void Login(string username, string password, bool saveToken)
        {
            LoginRequestStatusSubject.OnNext(LoginRequestStatus.Pending);
            
            _restServices.PostLogin(username, password)
                .SubscribeOn(_rxSchedulersFacade.IO())
                .ObserveOn(_rxSchedulersFacade.IO())
                .Subscribe(
                data => {
                    if (data == null)
                    {
                        _log.Info("AuthRepository","LoginPost returned null");
                        LoginRequestStatusSubject.OnNext(LoginRequestStatus.Error);
                        return;
                    }

                    if (data.Status == LoginResultStatusEnum.Logged)
                    {
                        _log.Info("AuthRepository","received session token");
                        if (saveToken)
                        {
                            _authService.SaveToken(username, data.Token, data.RefreshToken);
                        }
                        else
                        {
                            _authService.SetSingleSessionToken(username, data.Token, data.RefreshToken);
                        }

                        AuthStatus.OnNext(AuthStatusEnum.Logged);
                        LoginRequestStatusSubject.OnNext(LoginRequestStatus.Authorized);
                    }
                    else if(data.Status == LoginResultStatusEnum.NotAuthorized)
                    {
                        _log.Info("AuthRepository","user not authorized");
                        LoginRequestStatusSubject.OnNext(LoginRequestStatus.Unauthorized);
                    }
                    else
                    {
                        _log.Info("AuthRepository","bad cardinals");
                        LoginRequestStatusSubject.OnNext(LoginRequestStatus.BadLogin);
                    }
                },
                e => {
                    if (e.InnerException != null &&
                        e.InnerException.Message.Contains("ConnectFailure (Network is unreachable)"))
                    {
                        _log.Error("AuthRepository","no network", e);
                        LoginRequestStatusSubject.OnNext(LoginRequestStatus.NoNetwork);
                    }
                    else
                    {
                        _log.Error("AuthRepository","Login failed", e);
                        LoginRequestStatusSubject.OnNext(LoginRequestStatus.Error);
                    }
                });

            _log.Info("AuthRepository", "PostLogin");
        }

        /// <summary>
        /// Forward logout to AuthService and clear cart
        /// <param name="force">set if triggered by user</param>
        /// </summary>
        public void LogOut(bool force = false)
        {
            _authService.LogOut();

            AuthStatus.OnNext(force ? AuthStatusEnum.LoggedOutUserAction : AuthStatusEnum.LoggedOut);
        }

        /// <summary>
        /// Call with session token, for api methods with params other then session token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiCallMethodFunc">api method delegate</param>
        /// <returns>IObservable</returns>
        public IObservable<T> CallWithAuthToken<T>(Func<IObservable<T>> apiCallMethodFunc)
        {
            var hasInternetService = _connectionStatusRepository.CheckNetworkStatus();
            if (!hasInternetService) NoNetwork();
            return _restServices.CallWithAuthToken(
                apiCallMethodFunc, () => LogOut(), RefreshToken, NoNetwork, hasInternetService);
        }

        /// <summary>
        /// Notify no network
        /// </summary>
        private void NoNetwork()
        {
            AuthStatus.OnNext(AuthStatusEnum.NoInternet);
            Task.Delay(5000).ContinueWith(task => AuthStatus.OnNext(AuthStatusEnum.Idle));
        }

        /// <returns>Current bearer token</returns>
        public string GetBearerToken()
        {
            return _authService.GetBearerToken();
        }

        /// <summary>
        /// Makes synchronous PostRefresh call to Services
        /// </summary>
        /// <returns>token refresh success status</returns>
        public bool RefreshToken()
        {
            TokenModel tokenModel;

            try
            {
                tokenModel = _restServices.PostRefresh(GetUserName(), _authService.GetRefreshToken()).Wait();
                _log.Info("AuthRepository","token refresh called");
            }
            catch (Exception e)
            {
                _log.Error("AuthRepository","Unable to refresh bearer token", e);
                return false;
            }

            if (tokenModel == null)
            {
                _log.Error("AuthRepository","Bearer token refresh request returned null");
                return false;
            }

            if (_authService.IsSave())
            {
                _authService.SaveToken(GetUserName(), tokenModel.Token, _authService.GetRefreshToken());
            }
            else
            {
                _authService.SetSingleSessionToken(GetUserName(), tokenModel.Token,
                    _authService.GetRefreshToken());
            }

            _log.Info("AuthRepository","Successfully updated bearer token");

            return true;
        }

        /// <returns>Current user login/username</returns>
        public string GetUserName()
        {
            return _authService.GetUserName();
        }
    }
}
