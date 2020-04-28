using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace App.Web.Rest.Network
{
    public class RefitFactory : IRefitFactory
    {
        public T BuildService<T>(string url){
            return RestService.For<T>(url, 
                new RefitSettings
                {
                    ContentSerializer = new JsonContentSerializer(
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        })
                });
        }
    }
}
    