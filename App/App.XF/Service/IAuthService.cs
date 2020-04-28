namespace App.XF.Service
{
    public interface IAuthService
    {
        bool IsLoggedIn();
        bool IsSave();
        string GetBearerToken();
        string GetRefreshToken();
        string GetUserName();
        void SaveToken(string userName, string token, string refreshToken);
        void SetSingleSessionToken(string userName, string token, string refreshToken);
        void LogOut();
    }
}
