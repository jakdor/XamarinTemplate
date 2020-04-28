using App.XF.Repository;
using CrossPlatformLiveData;

namespace App.XF.ViewModel.Main
{
    /// <summary>
    /// Main Page ViewModel
    /// </summary>
    public interface IMainViewModel
    {
        ILiveData<AuthStatusEnum> AuthStatus { get; }
        string UserName { get; }

        void ShowStartupToast();
        void ForceLogout();
    }
}
