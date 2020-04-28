using CrossPlatformLiveData;

namespace App.XF.ViewModel.Base
{
    public interface IBaseViewModel
    {
        ILiveData<ToastRequest> ToastRequestLiveData { get; }
        ILiveData<AlertRequest> AlertRequestLiveData { get; }
    }
}