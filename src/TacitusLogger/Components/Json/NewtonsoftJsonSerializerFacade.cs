using Newtonsoft.Json;

namespace TacitusLogger.Components.Json
{
    public class NewtonsoftJsonSerializerFacade : IJsonSerializerFacade
    {
        public string Serialize(object obj, JsonSerializerSettings jsonSerializerSettings)
        {
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }
    }
}
