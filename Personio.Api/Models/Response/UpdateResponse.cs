using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class UpdateResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public UpdateData Data { get; set; }

        [JsonProperty(PropertyName = "meta")]
        public string[] Meta { get; set; }
    }
}
