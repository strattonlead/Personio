using Newtonsoft.Json;
using Personio.Api.Models.Attributes;

namespace Personio.Api.Models.Response
{
    public class GetTimeOffPeriodResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public TypeAndAttributesObject<TimeOffPeriodAttributes> Data { get; set; }
    }
}
