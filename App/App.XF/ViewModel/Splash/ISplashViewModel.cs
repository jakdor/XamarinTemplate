using CrossPlatformLiveData;

namespace App.XF.ViewModel.Splash
{
    /// <summary>
    /// Splash screen pre lunch ViewModel
    /// </summary>
    public interface ISplashViewModel
    {
        ILiveData<AppStartStatus> StartupLiveData { get; }
        void StartUp();
    }

    public enum AppStartStatus
    {
        Pending, LoginRequired, SwitchToMain, NoInternet
    }
}
