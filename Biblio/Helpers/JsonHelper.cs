using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Biblio.Helpers
{
    public static class JsonHelper
    {
        public static string ToJson(this object obj)
        {
            return ToFormattedJson(obj);
        }

        public static string ToFormattedJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { Converters = { new StringEnumConverter() }, NullValueHandling = NullValueHandling.Ignore });
        }

        public static string ToJsonExtended(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { Converters = { new StringEnumConverter() }, NullValueHandling = NullValueHandling.Ignore });
        }

        public static string ToCamelCaseJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() },
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
