using Newtonsoft.Json;

namespace Personio.Api.Models
{
    public class GetTimeOffTypesResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public TimeOffType[] Data { get; set; }
    }
}
