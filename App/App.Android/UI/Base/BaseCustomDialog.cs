using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.XF.DI.DependencyService;
using App.XF.ViewModel.Base;
using CrossPlatformLiveData;
using ToyaMobileNative.XF.i18n;

namespace App.Android.UI.Base
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract base class for custom fullscreen dialogs
    /// </summary>
    public abstract class BaseCustomDialog : global::Android.App.Dialog
    {
        protected readonly IAppRes AppRes;
        protected readonly IDeviceLog Log;
        protected readonly ILifecycleManager LifecycleManager = new LifecycleManager();

        protected BaseCustomDialog(Context context, IAppRes appRes, IDeviceLog log) : base(context)
        {
            AppRes = appRes;
            Log = log;
        }

        protected BaseCustomDialog(Context context, IAppRes appRes, IDeviceLog log, int themeResId)
            : base(context, themeResId)
        {
            AppRes = appRes;
            Log = log;
        }

        protected BaseCustomDialog(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected BaseCustomDialog(Context context, bool cancelable,
            IDialogInterfaceOnCancelListener cancelListener) : base(context, cancelable, cancelListener)
        {
        }

        protected BaseCustomDialog(Context context, bool cancelable, EventHandler cancelHandler)
            : base(context, cancelable, cancelHandler)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
        }

        public override void Show()
        {
            base.Show();
            LifecycleManager.OnResume();
        }

        public override void Dismiss()
        {
            LifecycleManager.Dispose();
            base.Dismiss();
        }

        protected void SubscribeToastDialog(IBaseViewModel viewModel)
        {
            viewModel.ToastRequestLiveData.Observe(LifecycleManager, OnToast, 
                e => Log.Error("BaseCustomDialog", "error observing ToastRequestLiveData", e));
        }

        private void OnToast(ToastRequest request)
        {
            Toast.MakeText(Context, request.Msg,
                request.Length == ToastRequest.ToastLength.Short ? ToastLength.Short : ToastLength.Long).Show();
        }
    }
}