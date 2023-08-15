using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class DeleteResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public DeleteData Data { get; set; }
    }
}
