using System;
using System.Reactive.Disposables;
using App.XF.DI.DependencyService;
using CrossPlatformLiveData;
using ToyaMobileNative.XF.i18n;
using IRxSchedulersFacade = App.XF.Utils.Rx.IRxSchedulersFacade;

namespace App.XF.ViewModel.Base
{
    /// <summary>
    /// Base abstract ViewModel
    /// </summary>
    public abstract class BaseViewModel : IBaseViewModel
    {
        protected CompositeDisposable RxDisposables = new CompositeDisposable();
        protected readonly IAppRes AppRes;
        protected readonly IRxSchedulersFacade RxSchedulersFacade;
        protected readonly IDeviceLog Log;

        public ILiveData<ToastRequest> ToastRequestLiveData { get; } = new LiveData<ToastRequest>();
        public ILiveData<AlertRequest> AlertRequestLiveData { get; } = new LiveData<AlertRequest>();

        protected BaseViewModel(IAppRes appRes, IRxSchedulersFacade rxSchedulersFacade, IDeviceLog deviceLog)
        {
            AppRes = appRes;
            RxSchedulersFacade = rxSchedulersFacade;
            Log = deviceLog;
        }

        ~BaseViewModel()
        {
            if (RxDisposables?.IsDisposed != false) RxDisposables?.Dispose();
        }

        protected void ShortToast(string msg)
        {
            ToastRequestLiveData.PostValue(new ToastRequest { Msg = msg, Length = ToastRequest.ToastLength.Short });
        }

        protected void LongToast(string msg)
        {
            ToastRequestLiveData.PostValue(new ToastRequest { Msg = msg, Length = ToastRequest.ToastLength.Long });
        }

        protected void OkAlert(string title, string msg = null, Action okAction = null, string okActionText = null)
        {
            AlertRequestLiveData.PostValue(AlertRequest.OkAlertRequest(title, msg, okAction, okActionText));
        }

        protected void OkCancelAlert(string title, Action okAction, string msg = null, Action cancelAction = null,
            string okActionText = null, string cancelActionText = null)
        {
            AlertRequestLiveData.PostValue(AlertRequest.OkCancelAlertRequest(
                title, okAction, msg, cancelAction, okActionText, cancelActionText));
        }
    }
}
