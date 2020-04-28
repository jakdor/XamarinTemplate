using App.XF.DI.DependencyService;

namespace App.XF.Service
{
    /// <summary>
    /// Provides and stores authentication info
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IDeviceStorageRepository _deviceStorageRepository;
        private const string AuthStorageKey = "AuthStorageKey";

        private string _token;
        private string _refreshToken;
        private string _userName;
        private bool _save;

        public AuthService(IDeviceStorageRepository deviceStorageRepository)
        {
            _deviceStorageRepository = deviceStorageRepository;

            if (!_deviceStorageRepository.ContainsKey(AuthStorageKey)) return;

            var storageModel = _deviceStorageRepository.GetObj<AuthStorageModel>(AuthStorageKey);
            _token = storageModel.Token;
            _refreshToken = storageModel.RefreshToken;
            _userName = storageModel.UserName;
            _save = storageModel.Save;
        }

        /// <summary>
        /// Check if user has stored session token - loads token form AccountStore
        /// </summary>
        /// <returns>true if token available</returns>
        public bool IsLoggedIn()
        {
            return _token != null && !string.IsNullOrEmpty(_token);
        }

        /// <returns>If saving is enabled</returns>
        public bool IsSave() => _save;
       
        /// <returns>Bearer session token</returns>
        public string GetBearerToken()
        {
            return _token;
        }

        /// <returns>Refresh token</returns>
        public string GetRefreshToken()
        {
            return _refreshToken;
        }

        /// <returns>User login</returns>
        public string GetUserName()
        {
            return _userName;
        }

        /// <summary>
        /// Save session token using AccountStore
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="token">bearer token</param>
        /// <param name="refreshToken">refresh token</param>
        public void SaveToken(string userName, string token, string refreshToken)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(token))
            {
                _token = token;
                _refreshToken = refreshToken;
                _userName = userName;
                _save = true;

                var storageModel = new AuthStorageModel
                {
                    Token = _token,
                    RefreshToken = _refreshToken,
                    UserName = _userName,
                    Save = _save
                };

                _deviceStorageRepository.SaveObj(AuthStorageKey, storageModel);
            }
        }

        /// <summary>
        /// Persist token in memory till app is closed / object destroyed
        /// </summary>
        /// <param name="userName">user login</param>
        /// <param name="token">session token</param>
        /// <param name="refreshToken">refresh token</param>
        public void SetSingleSessionToken(string userName, string token, string refreshToken)
        {
            _token = token;
            _refreshToken = refreshToken;
            _userName = userName;
            _save = false;
        }

        /// <summary>
        /// Clear saved user session
        /// </summary>
        public void LogOut()
        {
            _token = null;
            _refreshToken = null;
            _userName = null;

            if (_save)
            {
                var storageModel = new AuthStorageModel
                {
                    Token = _token,
                    RefreshToken = _refreshToken,
                    UserName = _userName,
                    Save = false
                };

                _deviceStorageRepository.SaveObj(AuthStorageKey, storageModel);
            }

            _save = false;
        }
    }
}
