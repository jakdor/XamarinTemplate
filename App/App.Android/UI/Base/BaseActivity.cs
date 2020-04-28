using Android.Support.V7.App;
using Android.Widget;
using App.XF.DI.DependencyService;
using App.XF.ViewModel.Base;
using CrossPlatformLiveData.Android;
using ToyaMobileNative.XF.i18n;

namespace App.Android.UI.Base
{
    /// <summary>
    /// Base Activity class
    /// </summary>
    public abstract class BaseActivity : LiveDataAppCompatActivity
    {
        protected readonly IDeviceLog Log = App.DiContainer.Resolve<IDeviceLog>();
        protected readonly IAppRes AppRes = App.DiContainer.Resolve<IAppRes>();

        protected void SubscribeToastDialog(IBaseViewModel viewModel)
        {
            viewModel.ToastRequestLiveData.Observe(LifecycleManager, OnToast,
                e => Log.Error("BaseActivity", "error observing ToastRequestLiveData", e));

            viewModel.AlertRequestLiveData.Observe(LifecycleManager, OnDialog,
                e => Log.Error("BaseActivity", "error observing AlertRequestLiveData", e));
        }

        private void OnToast(ToastRequest request)
        {
            Toast.MakeText(this, request.Msg,
                request.Length == ToastRequest.ToastLength.Short ? ToastLength.Short : ToastLength.Long).Show();
        }

        private void OnDialog(AlertRequest request)
        {
            var dialog = new AlertDialog.Builder(this);
            dialog.SetTitle(request.Title);
            if(request.Msg != null) dialog.SetMessage(request.Msg);

            dialog.SetPositiveButton(
                string.IsNullOrEmpty(request.OkActionText) ? AppRes["DialogYes"] : request.OkActionText,
                (sender, args) => { request.OkAction?.Invoke(); });

            if (request.Type == AlertRequest.AlertType.OkCancel)
            {
                dialog.SetNegativeButton(
                    string.IsNullOrEmpty(request.CancelActionText) ? AppRes["DialogNo"] : request.CancelActionText,
                    (sender, args) => { request.CancelAction?.Invoke(); });
            }

            dialog.Show();
        }
    }
}