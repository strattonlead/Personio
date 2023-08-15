using Newtonsoft.Json;
using Personio.Api.Models.Attributes;

namespace Personio.Api.Models.Response
{
    public class GetTimeOffTypesResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public TypeAndAttributesObject<TimeOffTypeAttributes>[] Data { get; set; }
    }
}
