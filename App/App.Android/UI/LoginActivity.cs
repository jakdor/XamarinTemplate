using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using App.Android.UI.Base;
using App.Android.UI.Login;
using App.XF.DI.DependencyService;
using App.XF.ViewModel.Login;
using ToyaMobileNative.XF.i18n;

namespace App.Android.UI
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoTitle", 
        ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class LoginActivity : BaseActivity, ILoginFragmentContract
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            if (savedInstanceState == null)
            {
                SupportFragmentManager.BeginTransaction()
                    .Add(Resource.Id.login_activity_frame_layout, LoginFragment.NewInstance())
                    .Commit();

                Log.Info(LocalClassName, "Switched to LoginFragment");
            }
        }

        public IDeviceLog GetDeviceLog()
        {
            return Log;
        }

        public IAppRes GetAppRes()
        {
            return AppRes;
        }

        public ILoginViewModel GetLoginViewModel()
        {
            return App.DiContainer.Resolve<ILoginViewModel>();
        }

        public IForgotPasswordViewModel GetForgotPasswordViewModel()
        {
            return App.DiContainer.Resolve<IForgotPasswordViewModel>();
        }

        public void SwitchToSplashActivity()
        {
            StartActivity(new Intent(ApplicationContext, typeof(SplashActivity)));
            Finish();
        }

        public void HideSoftKeyboard()
        {
            var view = CurrentFocus;
            if (view == null) return;
            var inputMethodManager = (InputMethodManager) GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(view.WindowToken, 0);
        }
    }
}