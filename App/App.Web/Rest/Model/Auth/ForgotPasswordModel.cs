namespace App.Web.Rest.Model.Auth
{
    /// <summary>
    /// IToyaMobileService - forgot password POST call model
    /// </summary>
    public class ForgotPasswordModel
    {
        public ForgotPasswordModel(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
    }
}