using System;
using Android.OS;
using Android.Support.Design.Button;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using App.Android.UI.Base;
using App.XF.ViewModel.Login;
using CrossPlatformLiveData;

namespace App.Android.UI.Login
{
    /// <summary>
    /// Login Fragment
    /// </summary>
    public class LoginFragment : BaseFragment
    {
        private ILoginViewModel _viewModel;

        private AppBarLayout _appBar;
        private TextInputLayout _loginInputLayout;
        private TextInputLayout _passwordInputLayout;
        private AppCompatEditText _loginEditText;
        private AppCompatEditText _passwordEditText;
        private AppCompatCheckBox _saveCheckBox;
        private TextView _restPassButton;
        private TextView _langSpinnerLabel;
        private AppCompatSpinner _langSpinner;
        private MaterialButton _guestLoginButton;
        private TextView _loginButton;
        private TextView _loginStatus;
        private FrameLayout _progressBarContainer;

        private bool _langSpinnerInit;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_login, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _appBar = view.FindViewById<AppBarLayout>(Resource.Id.login_AppBar);
            _loginInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.login_text_input);
            _passwordInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.password_text_input);
            _loginEditText = view.FindViewById<AppCompatEditText>(Resource.Id.login_edit_text);
            _passwordEditText = view.FindViewById<AppCompatEditText>(Resource.Id.password_edit_text);
            _saveCheckBox = view.FindViewById<AppCompatCheckBox>(Resource.Id.login_save_checkbox);
            _restPassButton = view.FindViewById<TextView>(Resource.Id.login_reset_password);
            _langSpinnerLabel = view.FindViewById<TextView>(Resource.Id.login_lang_spinner_label);
            _langSpinner = view.FindViewById<AppCompatSpinner>(Resource.Id.login_lang_spinner);
            _guestLoginButton = view.FindViewById<MaterialButton>(Resource.Id.login_guest_button);
            _loginButton = view.FindViewById<TextView>(Resource.Id.login_button);
            _loginStatus = view.FindViewById<TextView>(Resource.Id.login_status);
            _progressBarContainer = view.FindViewById<FrameLayout>(Resource.Id.login_progress_container);

            _loginEditText.FocusChange += (sender, args) =>
            {
                if (args.HasFocus) _appBar.SetExpanded(false, true);
            };

            _passwordEditText.FocusChange += (sender, args) =>
            {
                if (args.HasFocus) _appBar.SetExpanded(false, true);
            };

            _passwordEditText.EditorAction += (sender, args) => OnLoginClick();
            _loginButton.Click += (sender, args) => OnLoginClick();

            _restPassButton.Click += (sender, args) => ShowResetPasswordDialog();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            if (Activity is ILoginFragmentContract contract)
            {
                if (_viewModel == null)
                {
                    _viewModel = contract.GetLoginViewModel();

                    _viewModel.LoginStatusLiveData.Observe(LifecycleManager, HandleNewLoginStatus, 
                        e => Log.Error("LoginFragment", "Error observing LoginStatusLiveData", e));
                }
            }
            else
            {
                throw new Exception("Context not implementing ILoginFragmentContract");
            }

            var adapter = new ArrayAdapter<string>(
                Context, Resource.Layout.item_spinner, _viewModel.LanguagePickerList);
            adapter.SetDropDownViewResource(Resource.Layout.support_simple_spinner_dropdown_item);
            _langSpinner.Adapter = adapter;
            _langSpinner.SetSelection(_viewModel.LanguagePickerSelectedIndex);

            _langSpinner.ItemSelected += (sender, args) =>
            {
                if (!_langSpinnerInit)
                {
                    _langSpinnerInit = true;
                    return;
                }
                _viewModel.LanguagePickerSelectedIndex = args.Position;
                ApplyI18N();
            };
        }

        private void HandleNewLoginStatus(RxWrapper<Tuple<bool, string>> status)
        {
            switch (status.Status)
            {
                case RxStatus.NoData:
                    _loginStatus.Visibility = ViewStates.Gone;
                    _progressBarContainer.Visibility = ViewStates.Gone;
                    _loginButton.Enabled = true;
                    _loginButton.SetBackgroundColor(Resources.GetColor(Resource.Color.colorPrimary, Context.Theme));
                    break;
                case RxStatus.Ok:
                    if (status.Data.Item1)
                    {
                        _loginStatus.Visibility = ViewStates.Visible;
                        _progressBarContainer.Visibility = ViewStates.Gone;
                        _loginStatus.Text = status.Data.Item2;

                        (Activity as ILoginFragmentContract)?.SwitchToSplashActivity();
                    }
                    else
                    {
                        _loginStatus.Visibility = ViewStates.Visible;
                        _progressBarContainer.Visibility = ViewStates.Gone;
                        _loginButton.Enabled = true;
                        _loginButton.SetBackgroundColor(Resources.GetColor(Resource.Color.colorPrimary, Context.Theme));

                        _loginStatus.Text = status.Data.Item2;
                    }
                    break;
                case RxStatus.Pending:
                    _loginStatus.Visibility = ViewStates.Gone;
                    _progressBarContainer.Visibility = ViewStates.Visible;
                    _loginButton.Enabled = false;
                    _loginButton.SetBackgroundColor(Resources.GetColor(Resource.Color.lightGray, Context.Theme));
                    break;
                default:
                case RxStatus.Error:
                    _loginStatus.Visibility = ViewStates.Visible;
                    _progressBarContainer.Visibility = ViewStates.Gone;
                    _loginButton.Enabled = true;
                    _loginStatus.Text = AppRes["LoginStatusToast.error"];
                    _loginButton.SetBackgroundColor(Resources.GetColor(Resource.Color.colorPrimary, Context.Theme));
                    break;
            }
        }

        private void OnLoginClick()
        {
            if(_viewModel.LoginStatusLiveData.Value.Status == RxStatus.Pending) return;

            _viewModel.LoginUser = _loginEditText.Text;
            _viewModel.LoginPassword = _passwordEditText.Text;
            _viewModel.LoginSave = _saveCheckBox.Checked;

            switch (_viewModel.ValidateLoginPasswordFields())
            {
                case LoginPassValidation.Valid:
                    _loginInputLayout.Error = string.Empty;
                    _passwordInputLayout.Error = string.Empty;
                    (Activity as ILoginFragmentContract)?.HideSoftKeyboard();

                    _viewModel.OnLoginButtonClicked();
                    break;
                case LoginPassValidation.EmptyLogin:
                    _loginInputLayout.Error = AppRes["EmptyField"];
                    _passwordInputLayout.Error = string.Empty;
                    break;
                case LoginPassValidation.EmptyPass:
                    _passwordInputLayout.Error = AppRes["EmptyField"];
                    _loginInputLayout.Error = string.Empty;
                    break;
                case LoginPassValidation.EmptyBoth:
                    _loginInputLayout.Error = AppRes["EmptyField"];
                    _passwordInputLayout.Error = AppRes["EmptyField"];
                    break;
            }
        }

        /// <summary>
        /// Display ResetPasswordDialog
        /// </summary>
        public void ShowResetPasswordDialog()
        {
            if (Activity is ILoginFragmentContract contract)
            {
                var dialogViewModel = contract.GetForgotPasswordViewModel();
                new ResetPassDialog(Context, AppRes, Log, dialogViewModel).Show();
            }
            else
            {
                throw new Exception("Context not implementing ILoginFragmentContract");
            }
        }

        protected override void ApplyI18N()
        {
            base.ApplyI18N();
            _loginInputLayout.Hint = AppRes["LoginUserPlaceholder"];
            _passwordInputLayout.Hint = AppRes["LoginPassPlaceholder"];
            _saveCheckBox.Text = AppRes["LoginSaveCheckBox"];
            _restPassButton.Text = AppRes["LoginRecoverPass"];
            _langSpinnerLabel.Text = AppRes["LoginLanguageLabel"];
            _guestLoginButton.Text = AppRes["LoginGuestButton"];
            _loginButton.Text = AppRes["LoginButton"];
            _loginInputLayout.Error = string.Empty;
            _passwordInputLayout.Error = string.Empty;
        }
        
        public static LoginFragment NewInstance()
        {
            var bundle = new Bundle();
            var fragment = new LoginFragment { Arguments = bundle };
            return fragment;
        }
    }
}