using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class ErrorObject
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
