namespace App.Web.Rest.Model.Auth
{
    /// <summary>
    /// IToyaMobileService - post response model
    /// </summary>
    public class LoginResult
    {
        public LoginResultStatusEnum Status { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}