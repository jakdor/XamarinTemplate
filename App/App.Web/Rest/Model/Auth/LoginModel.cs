namespace App.Web.Rest.Model.Auth
{
    /// <summary>
    /// IToyaMobileService - post call model
    /// </summary>
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}