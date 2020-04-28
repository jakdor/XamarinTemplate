using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using App.Android.UI.Base;
using App.XF.DI.DependencyService;
using App.XF.ViewModel.Base;
using App.XF.ViewModel.Login;
using CrossPlatformLiveData;
using ToyaMobileNative.XF.i18n;

namespace App.Android.UI.Login
{
    /// <inheritdoc />
    /// <summary>
    /// Custom fullscreen reset password dialog
    /// </summary>
    public class ResetPassDialog : BaseCustomDialog
    {
        private readonly IForgotPasswordViewModel _viewModel;

        private TextView _header;
        private TextInputLayout _loginInputLayout;
        private TextInputLayout _emailInputLayout;
        private AppCompatEditText _loginEditText;
        private AppCompatEditText _emailEditText;
        private FrameLayout _progressBarContainer;
        private TextView _resetPassButton;
       

        public ResetPassDialog(Context context, IAppRes appRes, IDeviceLog log, IForgotPasswordViewModel viewModel)
            : base(context, appRes, log)
        {
            _viewModel = viewModel;
            if (_viewModel is IBaseViewModel vm) SubscribeToastDialog(vm);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.dialog_reset_pass);

            Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            Window.SetGravity(GravityFlags.Center);

            _header = FindViewById<TextView>(Resource.Id.reset_pass_header);
            _loginInputLayout = FindViewById<TextInputLayout>(Resource.Id.reset_pass_login_input);
            _emailInputLayout = FindViewById<TextInputLayout>(Resource.Id.reset_pass_email_input);
            _loginEditText = FindViewById<AppCompatEditText>(Resource.Id.reset_pass_login_edit_text);
            _emailEditText = FindViewById<AppCompatEditText>(Resource.Id.reset_pass_email_edit_text);
            _progressBarContainer = FindViewById<FrameLayout>(Resource.Id.reset_pass_progress_container);
            _resetPassButton = FindViewById<TextView>(Resource.Id.reset_pass_button);

            FindViewById<ImageView>(Resource.Id.reset_pass_close).Click += (sender, args) => Dismiss();

            _loginEditText.TextChanged += (sender, args) => Validate();
            _emailEditText.TextChanged += (sender, args) => Validate();

            _emailEditText.EditorAction += (sender, args) => OnPassResetClick();
            _resetPassButton.Click += (sender, args) => OnPassResetClick();

            ApplyI18N();

            if(savedInstanceState != null) return;

            _viewModel.PasswordResetLiveData.Observe(LifecycleManager, OnPasswordReset, 
                e => Log.Error("ResetPassDialog", "Error observing PasswordResetLiveData", e));
        }

        private void OnPasswordReset(RxWrapper<bool> status)
        {
            switch (status.Status)
            {
                case RxStatus.NoData:
                    SetCanceledOnTouchOutside(true);
                    _progressBarContainer.Visibility = ViewStates.Gone;
                    break;
                case RxStatus.Ok:
                    if (status.Data)
                    {
                        Dismiss();
                    }
                    else
                    {
                        SetCanceledOnTouchOutside(true);
                        _progressBarContainer.Visibility = ViewStates.Gone;
                        _resetPassButton.Enabled = true;
                        _resetPassButton.SetBackgroundColor(
                            Context.Resources.GetColor(Resource.Color.colorPrimary, Context.Theme));
                    }
                    break;
                case RxStatus.Pending:
                    if (CurrentFocus != null)
                    {
                        var inputManager = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
                        inputManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                    }

                    SetCanceledOnTouchOutside(false);
                    _progressBarContainer.Visibility = ViewStates.Visible;
                    _resetPassButton.Enabled = false;
                    _resetPassButton.SetBackgroundColor(
                        Context.Resources.GetColor(Resource.Color.lightGray, Context.Theme));
                    break;
                default:
                case RxStatus.Error:
                    SetCanceledOnTouchOutside(true);
                    _progressBarContainer.Visibility = ViewStates.Gone;
                    _resetPassButton.Enabled = true;
                    _resetPassButton.SetBackgroundColor(
                        Context.Resources.GetColor(Resource.Color.colorPrimary, Context.Theme));
                    break;
            }
        }

        private void Validate()
        {
            _viewModel.Login = _loginEditText.Text;
            _viewModel.Email = _emailEditText.Text;
            if (_viewModel.IsUserEntryValid())
            {
                _resetPassButton.Enabled = true;
                _resetPassButton.SetBackgroundColor(
                    Context.Resources.GetColor(Resource.Color.colorPrimary, Context.Theme));
            }
            else
            {
                _resetPassButton.Enabled = false;
                _resetPassButton.SetBackgroundColor(
                    Context.Resources.GetColor(Resource.Color.lightGray, Context.Theme));
            }
        }

        private void OnPassResetClick()
        {
            _viewModel.Login = _loginEditText.Text;
            _viewModel.Email = _emailEditText.Text;
            _viewModel.OnSendButtonClicked();
        }

        private void ApplyI18N()
        {
            _header.Text = AppRes["ForgotPasswordTitle"];
            _loginInputLayout.Hint = AppRes["LoginUserPlaceholder"];
            _emailInputLayout.Hint = AppRes["LoginEmailPlaceholder"];
            _resetPassButton.Text = AppRes["ForgotPasswordSendButton"];
        }
    }
}