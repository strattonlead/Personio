using Newtonsoft.Json;

namespace Personio.Api.Models.Request
{
    public class AuthRequest
    {
        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "client_secret")]
        public string ClientSecret { get; set; }
    }
}
