namespace App.XF.DI.DependencyService
{
    /// <summary>
    /// Device storage repository interface - to be implemented on each platform
    /// </summary>
    public interface IDeviceStorageRepository
    {
        void SaveObj<T>(string key, T obj);
        T GetObj<T>(string key);
        bool ContainsKey(string key);
    }
}
