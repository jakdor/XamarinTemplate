namespace App.XF.DI.DependencyService
{
    public interface IConnectionStatusRepository
    {
        bool CheckNetworkStatus();
    }
}
