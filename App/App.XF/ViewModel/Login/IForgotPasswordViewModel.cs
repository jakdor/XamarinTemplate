using CrossPlatformLiveData;

namespace App.XF.ViewModel.Login
{
    /// <summary>
    /// Forgot password dialog ViewModel
    /// </summary>
    public interface IForgotPasswordViewModel
    {
        ILiveData<RxWrapper<bool>> PasswordResetLiveData { get; }
        string Login { get; set; }
        string Email { get; set; }

        bool IsUserEntryValid();
        void OnSendButtonClicked();
    }
}
