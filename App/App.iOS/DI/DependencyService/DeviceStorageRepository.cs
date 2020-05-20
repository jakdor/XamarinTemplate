using App.XF.DI.DependencyService;
using Foundation;

namespace App.iOS.DI.DependencyService
{
    internal class DeviceStorageRepository : BaseDeviceStorageRepository, IDeviceStorageRepository
    {
        private readonly NSUserDefaults _storage = NSUserDefaults.StandardUserDefaults;

        public void SaveObj<T>(string key, T obj)
        {
            _storage.SetString(Serialize(obj), key);
        }

        public T GetObj<T>(string key)
        {
            var objStr = _storage.StringForKey(key);
            return string.IsNullOrEmpty(objStr) ? default(T) : Deserialize<T>(objStr);
        }

        public bool ContainsKey(string key)
        {
            return _storage.ValueForKey(new NSString(key)) != null;
        }
    }
}
