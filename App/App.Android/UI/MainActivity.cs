using Android.App;
using Android.Content.PM;
using Android.OS;
using App.Android.UI.Base;
using App.XF.ViewModel.Base;
using App.XF.ViewModel.Main;

namespace App.Android.UI
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {
        private readonly IMainViewModel _viewModel = App.DiContainer.Resolve<IMainViewModel>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            if (savedInstanceState == null){
                if (_viewModel is IBaseViewModel vm) SubscribeToastDialog(vm);
                _viewModel.ShowStartupToast();
            }
        }
    }
}

