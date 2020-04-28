using System;
using System.Collections.Generic;
using CrossPlatformLiveData;

namespace App.XF.ViewModel.Login
{
    /// <summary>
    /// Login page ViewModel
    /// </summary>
    public interface ILoginViewModel
    {
        ILiveData<RxWrapper<Tuple<bool, string>>> LoginStatusLiveData { get; }
        IList<string> LanguagePickerList { get; }
        int LanguagePickerSelectedIndex { get; set; }
        string LoginUser { get; set; }
        string LoginPassword { get; set; }
        bool LoginSave { get; set; }

        LoginPassValidation ValidateLoginPasswordFields();
        void OnLoginButtonClicked();
    }

    /// <summary>
    /// User entry validation state
    /// </summary>
    public enum LoginPassValidation
    {
        Valid, EmptyLogin, EmptyPass, EmptyBoth
    }
}
