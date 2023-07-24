using Newtonsoft.Json;

namespace Personio.Api.Models
{
    public class TokenData
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
