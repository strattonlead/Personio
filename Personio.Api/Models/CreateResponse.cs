using Newtonsoft.Json;

namespace Personio.Api.Models
{
    public class CreateResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public CreateData Data { get; set; }
    }
}
