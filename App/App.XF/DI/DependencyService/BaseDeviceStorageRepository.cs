using System;
using System.Text;
using Newtonsoft.Json;

namespace App.XF.DI.DependencyService
{
    /// <summary>
    /// Abstract base class for shared DeviceStorageRepository internal methods
    /// </summary>
    public abstract class BaseDeviceStorageRepository
    {
        protected string Serialize<T>(T obj)
        {
            var serializedObj = JsonConvert.SerializeObject(obj);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedObj));
        }

        protected T Deserialize<T>(string str)
        {
            var serializedObj = Encoding.UTF8.GetString(Convert.FromBase64String(str));
            return JsonConvert.DeserializeObject<T>(serializedObj);
        }
    }
}
