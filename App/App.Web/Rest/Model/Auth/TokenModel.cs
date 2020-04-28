namespace App.Web.Rest.Model.Auth
{
    /// <summary>
    ///  IToyaMobileService - Token model used in auth header and refresh request
    /// </summary>
    public class TokenModel
    {
        public string Login { get; set; }
        public string Token { get; set; }

        public TokenModel(string login, string token)
        {
            Login = login;
            Token = token;
        }
    }
}