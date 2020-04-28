using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using App.Web;
using App.Web.Rest.Model.Auth;
using App.XF.DI.DependencyService;
using App.XF.Utils.Rx;

namespace App.XF.Repository
{
    /// <summary>
    /// Repository for resetting user password
    /// </summary>
    class ForgotPasswordRepository : IForgotPasswordRepository
    {
        private readonly IRestServices _restServices;
        private readonly IRxSchedulersFacade _rxSchedulersFacade;
        private readonly IDeviceLog _log;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public BehaviorSubject<ForgotPasswordRequestStatus> ForgotPasswordRequestStatusSubject { get; } =
            new BehaviorSubject<ForgotPasswordRequestStatus>(ForgotPasswordRequestStatus.Idle);

        public ForgotPasswordRepository(IRestServices restServices, IRxSchedulersFacade rxSchedulersFacade,
            IDeviceLog log)
        {
            _restServices = restServices;
            _rxSchedulersFacade = rxSchedulersFacade;
            _log = log;
        }

        ~ForgotPasswordRepository()
        {
            if(!_disposable.IsDisposed) _disposable.Dispose();
        }

        /// <summary>
        /// Makes PostForgotPassword call
        /// </summary>
        public void RequestPasswordReset(string login, string email)
        {
            ForgotPasswordRequestStatusSubject.OnNext(ForgotPasswordRequestStatus.Pending);

            _disposable.Add(_restServices.PostForgotPassword(login, email)
                .ObserveOn(_rxSchedulersFacade.IO())
                .SubscribeOn(_rxSchedulersFacade.IO())
                .Subscribe(result =>
                {
                    if (result.Status == ForgotPasswordStatusEnum.Ok)
                    {
                        _log.Info("ForgotPasswordRepository","successful reset password request");
                        ForgotPasswordRequestStatusSubject.OnNext(ForgotPasswordRequestStatus.Success);
                    }
                    else
                    {
                        _log.Info("ForgotPasswordRepository", "bad reset password request");
                        ForgotPasswordRequestStatusSubject.OnNext(ForgotPasswordRequestStatus.BadData);
                    }
                },
                e =>
                {
                    if (e.InnerException != null &&
                        e.InnerException.Message.Contains("ConnectFailure (Network is unreachable)"))
                    {
                        _log.Error("ForgotPasswordRepository", "no network", e);
                        ForgotPasswordRequestStatusSubject.OnNext(ForgotPasswordRequestStatus.NoNetwork);
                    }
                    else
                    {
                        _log.Error("ForgotPasswordRepository", "error occured during PostForgotPassword call", e);
                        ForgotPasswordRequestStatusSubject.OnNext(ForgotPasswordRequestStatus.Error);
                    }
                }));

            _log.Info("ForgotPasswordRepository","call to PostForgotPassword");
        }
    }
}