using Android.Content;
using App.XF.DI.DependencyService;

namespace App.Android.DI.DependencyService
{
    internal class DeviceStorageRepository: BaseDeviceStorageRepository, IDeviceStorageRepository
    {
        private readonly ISharedPreferences _sharedPreferences;

        public DeviceStorageRepository(Context context)
        {
            _sharedPreferences = context.GetSharedPreferences(
                context.GetString(Resource.String.pref_file_name), FileCreationMode.Private);
        }

        public void SaveObj<T>(string key, T obj)
        {
            var editor = _sharedPreferences.Edit();
            editor.PutString(key, Serialize(obj));
            editor.Apply();
        }

        public T GetObj<T>(string key)
        {
            var objStr = _sharedPreferences.GetString(key, "");
            return string.IsNullOrEmpty(objStr) ? default(T) : Deserialize<T>(objStr);
        }

        public bool ContainsKey(string key)
        {
            return _sharedPreferences.Contains(key);
        }
    }
}
