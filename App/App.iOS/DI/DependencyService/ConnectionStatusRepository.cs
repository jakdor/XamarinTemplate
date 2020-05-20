using App.XF.DI.DependencyService;
using Plugin.Connectivity;

namespace App.iOS.DI.DependencyService
{
    internal class ConnectionStatusRepository : IConnectionStatusRepository
    {
        private readonly IDeviceLog _log;

        public ConnectionStatusRepository(IDeviceLog log)
        {
            _log = log;
        }

        public bool CheckNetworkStatus()
        {
            if (CrossConnectivity.Current.IsConnected) return true;

            _log.Info("ConnectionStatusRepository", "Internet status: no service!");
            return false;
        }
    }
}
