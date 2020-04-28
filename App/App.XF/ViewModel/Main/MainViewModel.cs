using System;
using System.Reactive.Linq;
using App.XF.DI.DependencyService;
using App.XF.Repository;
using App.XF.ViewModel.Base;
using CrossPlatformLiveData;
using ToyaMobileNative.XF.i18n;
using IRxSchedulersFacade = App.XF.Utils.Rx.IRxSchedulersFacade;

namespace App.XF.ViewModel.Main
{
    /// <summary>
    /// <inheritdoc cref="IMainViewModel"/>
    /// </summary>
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        private readonly IAuthRepository _authRepository;

        public ILiveData<AuthStatusEnum> AuthStatus { get; } = new LiveData<AuthStatusEnum>();
        public string UserName => _authRepository.GetUserName();

        public MainViewModel(IAppRes appRes, IRxSchedulersFacade rxSchedulersFacade, IDeviceLog deviceLog, 
            IAuthRepository authRepository) : base(appRes, rxSchedulersFacade, deviceLog)
        {
            _authRepository = authRepository;
            
            RxDisposables.Add(authRepository.AuthStatus
                .ObserveOn(RxSchedulersFacade.IO())
                .SubscribeOn(RxSchedulersFacade.IO())
                .Subscribe(status => AuthStatus.PostValue(status),
                    exception => Log.Error("MainViewModel", 
                        "Error observing AuthRepository AuthStatus", exception)));
        }

        /// <summary>
        /// Test toast
        /// </summary>
        public void ShowStartupToast()
        {
            ShortToast(AppRes["AppName"]);
        }

        /// <summary>
        /// User logout action
        /// </summary>
        public void ForceLogout()
        {
            _authRepository.LogOut(true);
        }
    }
}
