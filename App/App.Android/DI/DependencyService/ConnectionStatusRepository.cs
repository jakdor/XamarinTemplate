using Android.Content;
using Android.Net;
using App.XF.DI.DependencyService;

namespace App.Android.DI.DependencyService
{
    internal class ConnectionStatusRepository : IConnectionStatusRepository
    {
        private readonly ConnectivityManager _connectivityManager;
        private readonly IDeviceLog _log;

        public ConnectionStatusRepository(Context appContext, IDeviceLog log)
        {
            _log = log;
            _connectivityManager = (ConnectivityManager) appContext.GetSystemService(Context.ConnectivityService);
        }

        public bool CheckNetworkStatus()
        {
            if (_connectivityManager == null)
            {
                _log.Error("ConnectionStatusRepository", "Internet status: failed to get status!");
                return false;
            }

            var networkInfo = _connectivityManager.ActiveNetworkInfo;
            if (networkInfo == null)
            {
                _log.Error("ConnectionStatusRepository", "Internet status: no service!");
                return false;
            }

            return true;
        }
    }
}