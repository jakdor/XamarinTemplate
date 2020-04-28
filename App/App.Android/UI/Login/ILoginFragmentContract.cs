using App.Android.UI.Base;
using App.XF.ViewModel.Login;

namespace App.Android.UI.Login
{
    internal interface ILoginFragmentContract : IBaseFragmentContract
    {
        ILoginViewModel GetLoginViewModel();
        IForgotPasswordViewModel GetForgotPasswordViewModel();
        void SwitchToSplashActivity();
        void HideSoftKeyboard();
    }
}