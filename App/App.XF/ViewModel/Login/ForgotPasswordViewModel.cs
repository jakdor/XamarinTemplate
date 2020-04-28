using System;
using System.Net.Mail;
using System.Reactive.Linq;
using App.XF.DI.DependencyService;
using App.XF.Repository;
using App.XF.ViewModel.Base;
using CrossPlatformLiveData;
using ToyaMobileNative.XF.i18n;
using IRxSchedulersFacade = App.XF.Utils.Rx.IRxSchedulersFacade;

namespace App.XF.ViewModel.Login
{
    /// <summary>
    /// <inheritdoc cref="IForgotPasswordViewModel"/>
    /// </summary>
    public class ForgotPasswordViewModel : BaseViewModel, IForgotPasswordViewModel
    {
        private readonly IForgotPasswordRepository _forgotPasswordRepository;

        public ILiveData<RxWrapper<bool>> PasswordResetLiveData { get; } = new LiveData<RxWrapper<bool>>();
        public string Login { get; set; }
        public string Email { get; set; }

        public ForgotPasswordViewModel(IAppRes appRes, IRxSchedulersFacade rxSchedulersFacade, IDeviceLog deviceLog,
            IForgotPasswordRepository forgotPasswordRepository) : base(appRes, rxSchedulersFacade, deviceLog)
        {
            _forgotPasswordRepository = forgotPasswordRepository;

            RxDisposables.Add(_forgotPasswordRepository.ForgotPasswordRequestStatusSubject
                .SubscribeOn(RxSchedulersFacade.IO())
                .ObserveOn(RxSchedulersFacade.UI())
                .Subscribe(HandleNewForgotPasswordRequestStatus, e =>
                    {
                        LongToast(AppRes["ForgotPasswordError"]);
                        Log.Error("ForgotPasswordPageViewModel",
                            "error subscribing to ForgotPasswordRequestStatusSubject", e);
                    }
                ));
        }

        private void HandleNewForgotPasswordRequestStatus(ForgotPasswordRequestStatus status)
        {
            switch (status)
            {
                case ForgotPasswordRequestStatus.Idle:
                    PasswordResetLiveData.PostValue(RxWrapper<bool>.NoData());
                    break;
                case ForgotPasswordRequestStatus.Pending:
                    PasswordResetLiveData.PostValue(RxWrapper<bool>.Pending());
                    break;
                case ForgotPasswordRequestStatus.Success:
                    LongToast(AppRes["ForgotPasswordSuccess"]);
                    PasswordResetLiveData.PostValue(RxWrapper<bool>.Ok(true));
                    break;
                case ForgotPasswordRequestStatus.BadData:
                    ShortToast(AppRes["ForgotPasswordBadData"]);
                    PasswordResetLiveData.PostValue(RxWrapper<bool>.Ok(false));
                    break;
                case ForgotPasswordRequestStatus.NoNetwork:
                    ShortToast(AppRes["NoNetworkToast"]);
                    PasswordResetLiveData.PostValue(RxWrapper<bool>.Error(new Exception(AppRes["NoNetworkToast"])));
                    break;
                case ForgotPasswordRequestStatus.Error:
                default:
                    ShortToast(AppRes["ForgotPasswordError"]);
                    Log.Error("ForgotPasswordPageViewModel", "ForgotPasswordRequest returned unknown error");
                    PasswordResetLiveData.PostValue(RxWrapper<bool>.Error(new Exception(AppRes["ForgotPasswordError"])));
                    break;
            }
        }

        /// <summary>
        /// Validates user entry
        /// </summary>
        /// <returns>true if valid</returns>
        public bool IsUserEntryValid()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Email)) return false;

            try
            {
                var address = new MailAddress(Email);
                return address.Address == Email;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Forward call request to ForgotPasswordRepository
        /// </summary>
        public void OnSendButtonClicked()
        {
            if (IsUserEntryValid()) _forgotPasswordRepository.RequestPasswordReset(Login, Email);
        }
    }
}
