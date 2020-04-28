using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using App.XF.DI.DependencyService;
using App.XF.Repository;
using App.XF.ViewModel.Base;
using CrossPlatformLiveData;
using ToyaMobileNative.XF.i18n;
using IRxSchedulersFacade = App.XF.Utils.Rx.IRxSchedulersFacade;

namespace App.XF.ViewModel.Login
{
    /// <summary>
    /// <inheritdoc cref="ILoginViewModel" />
    /// </summary>
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        private readonly IAuthRepository _authRepository;

        public ILiveData<RxWrapper<Tuple<bool, string>>> LoginStatusLiveData { get; } =
            new LiveData<RxWrapper<Tuple<bool, string>>>();

        public IList<string> LanguagePickerList { get; }

        private int _languagePickerSelectedIndex;
        public int LanguagePickerSelectedIndex
        {
            get => _languagePickerSelectedIndex;
            set
            {
                _languagePickerSelectedIndex = value;
                LanguagePickerSelectedIndexChanged();
            }
        }

        public string LoginUser { get; set; }
        public string LoginPassword { get; set; }
        public bool LoginSave { get; set; }

        public LoginViewModel(IAppRes appRes, IRxSchedulersFacade rxSchedulersFacade, IDeviceLog deviceLog, 
            IAuthRepository authRepository) : base(appRes, rxSchedulersFacade, deviceLog)
        {
            _authRepository = authRepository;

            LanguagePickerList = new List<string>(
                new[] { AppRes["LangName.pl"], AppRes["LangName.ro"], AppRes["LangName.en"] });

            switch (AppRes.LangCode)
            {
                case "pl":
                    _languagePickerSelectedIndex = 0;
                    break;
                case "ro":
                    _languagePickerSelectedIndex = 1;
                    break;
                default: //en
                    _languagePickerSelectedIndex = 2;
                    break;
            }

            RxDisposables.Add(_authRepository.LoginRequestStatusSubject
                .SubscribeOn(RxSchedulersFacade.IO())
                .ObserveOn(RxSchedulersFacade.UI())
                .Subscribe(HandleNewLoginRequestStatus, 
                    e =>
                    {
                        LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.Error(e));
                        Log.Error("LoginViewModel", "LoginRequestStatusSubject error occurred", e);
                    }));
        }

        /// <summary>
        /// Handle LoginRequestStatus received from AuthRepository
        /// </summary>
        /// <param name="status">LoginRequestStatus enum</param>
        private void HandleNewLoginRequestStatus(LoginRequestStatus status)
        {
            switch (status)
            {
                case LoginRequestStatus.Idle:
                    LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.NoData());

                    break;
                case LoginRequestStatus.Pending:
                    LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.Pending());
                    
                    break;
                case LoginRequestStatus.Authorized:
                    Log.Info("LoginViewModel", "Login successful");

                    LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.Ok(
                        new Tuple<bool, string>(true, AppRes["LoginStatusToast.authorized"])));

                    break;
                case LoginRequestStatus.Unauthorized:
                    Log.Info("LoginViewModel", "Login request returned user not authorized");

                    LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.Ok(
                        new Tuple<bool, string>(false, AppRes["LoginStatusToast.unauthorized2"])));

                    break;
                case LoginRequestStatus.BadLogin:
                    Log.Info("LoginViewModel", "Login request returned bad cardinals");

                    LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.Ok(
                        new Tuple<bool, string>(false, AppRes["LoginStatusToast.unauthorized"])));

                    break;
                case LoginRequestStatus.NoNetwork:
                    LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.Ok(
                        new Tuple<bool, string>(false, AppRes["NoNetworkToast"])));

                    break;
                case LoginRequestStatus.Error:
                default:
                    Log.Error("LoginViewModel", "Login request returned unknown error");

                    LoginStatusLiveData.PostValue(RxWrapper<Tuple<bool, string>>.Error(
                        new Exception("Login request returned unknown error")));

                    break;
            }
        }

        /// <summary>
        /// Handle LanguagePicker index change
        /// </summary>
        private void LanguagePickerSelectedIndexChanged()
        {
            switch (LanguagePickerSelectedIndex)
            {
                case 0:
                    AppRes.SetLang("pl");
                    break;
                case 1:
                    AppRes.SetLang("ro");
                    break;
                default:
                    AppRes.SetLang("en");
                    break;
            }
        }

        /// <summary>
        /// Validate user input in Login/Password fields
        /// </summary>
        /// <returns>LoginPassValidation</returns>
        public LoginPassValidation ValidateLoginPasswordFields()
        {
            if (string.IsNullOrEmpty(LoginUser) && string.IsNullOrEmpty(LoginPassword))
            {
                return LoginPassValidation.EmptyBoth;
            }

            if (string.IsNullOrEmpty(LoginUser))
            {
                return LoginPassValidation.EmptyLogin;
            }

            if (string.IsNullOrEmpty(LoginPassword))
            {
                return LoginPassValidation.EmptyPass;
            }

            return LoginPassValidation.Valid;
        }

        /// <summary>
        /// Called on login button click
        /// </summary>
        public void OnLoginButtonClicked()
        {
            Log.Info("LoginViewModel", "OnLoginButtonClicked()");

            if (ValidateLoginPasswordFields() == LoginPassValidation.Valid)
            {
                _authRepository.Login(LoginUser, LoginPassword, LoginSave);
            }
        }
    }
}
