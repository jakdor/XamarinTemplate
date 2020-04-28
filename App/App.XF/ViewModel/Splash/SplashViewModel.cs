using System;
using System.Reactive.Linq;
using App.XF.DI.DependencyService;
using App.XF.Repository;
using App.XF.ViewModel.Base;
using CrossPlatformLiveData;
using ToyaMobileNative.XF.i18n;
using IRxSchedulersFacade = App.XF.Utils.Rx.IRxSchedulersFacade;

namespace App.XF.ViewModel.Splash
{
    /// <summary>
    /// <inheritdoc cref="ISplashViewModel"/>
    /// </summary>
    public class SplashViewModel : BaseViewModel, ISplashViewModel
    {
        private readonly IAuthRepository _authRepository;

        public ILiveData<AppStartStatus> StartupLiveData { get; } = new LiveData<AppStartStatus>();

        public SplashViewModel(IAppRes appRes, IRxSchedulersFacade rxSchedulersFacade, IDeviceLog deviceLog,
            IAuthRepository authRepository) 
            : base(appRes, rxSchedulersFacade, deviceLog)
        {
            _authRepository = authRepository;

            RxDisposables.Add(_authRepository.AuthStatus
                .ObserveOn(RxSchedulersFacade.IO())
                .SubscribeOn(RxSchedulersFacade.IO())
                .Subscribe(status =>
                {
                    if (status == AuthStatusEnum.LoggedOut)
                    {
                        StartupLiveData.PostValue(AppStartStatus.LoginRequired);
                    }
                    else if(status == AuthStatusEnum.NoInternet)
                    {
                        StartupLiveData.PostValue(AppStartStatus.NoInternet);
                    }
                }, e =>
                {
                    StartupLiveData.PostValue(AppStartStatus.LoginRequired);
                    Log.Error("SplashViewModel", "error subscribing to AuthStatus", e);
                }));
        }

        /// <summary>
        /// Get date required at startup
        /// </summary>
        public void StartUp()
        {
            StartupLiveData.PostValue(AppStartStatus.Pending);

            if (_authRepository.IsLoggedIn())
            {
                StartupLiveData.PostValue(AppStartStatus.SwitchToMain);
            }
            else
            {
                StartupLiveData.PostValue(AppStartStatus.LoginRequired);
            }
        }
    }
}
