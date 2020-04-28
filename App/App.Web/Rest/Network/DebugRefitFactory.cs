using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace App.Web.Rest.Network
{
    public class DebugRefitFactory : IRefitFactory
    {
        public T BuildService<T>(string url)
        {
            return RestService.For<T>(
                new HttpClient(new HttpLoggingHandler()) {BaseAddress = new Uri(url)}, new RefitSettings
                {
                    ContentSerializer = new JsonContentSerializer(
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }
                    )
                });
        }
    }
}
