using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using App.Android.UI.Base;
using App.XF.ViewModel.Splash;

namespace App.Android.UI
{
    [Activity(Label = "@string/app_name", Theme = "@style/splashscreen", MainLauncher = true,
        NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : BaseActivity
    {
        private readonly ISplashViewModel _viewModel = App.DiContainer.Resolve<ISplashViewModel>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);

            if (savedInstanceState == null)
            {
                _viewModel.StartupLiveData.Observe(LifecycleManager, HandelNewAppStartStatus,
                    e => Log.Error("SplashActivity", "Error observing StartupLiveData", e));
            }

            _viewModel.StartUp();
        }

        private void HandelNewAppStartStatus(AppStartStatus status)
        {
            switch (status)
            {
                case AppStartStatus.Pending:
                    break;
                case AppStartStatus.LoginRequired:
                    StartActivity(new Intent(ApplicationContext, typeof(LoginActivity)));
                    OverridePendingTransition(0, 0);
                    Finish();
                    break;
                case AppStartStatus.SwitchToMain:
                    StartActivity(new Intent(ApplicationContext, typeof(MainActivity)));
                    OverridePendingTransition(0, 0);
                    Finish();
                    break;
                case AppStartStatus.NoInternet:
                    Toast.MakeText(this, AppRes["NoNetworkToast"], ToastLength.Long).Show();

                    StartActivity(new Intent(ApplicationContext, typeof(MainActivity)));
                    OverridePendingTransition(0, 0);
                    Finish();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}