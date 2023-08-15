using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class AuthResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public TokenData Data { get; set; }
    }
}
