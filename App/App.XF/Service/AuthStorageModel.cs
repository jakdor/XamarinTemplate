namespace App.XF.Service
{
    public class AuthStorageModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public bool Save { get; set; }
    }
}
