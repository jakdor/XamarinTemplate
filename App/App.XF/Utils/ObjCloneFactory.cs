using Newtonsoft.Json;

namespace App.XF.Utils
{
    /// <summary>
    /// <inheritdoc cref="IObjCloneFactory"/>
    /// </summary>
    public class ObjCloneFactory : IObjCloneFactory
    {
        public T DeepClone<T>(T obj)
        {
            var objStr = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(objStr);
        }
    }
}
