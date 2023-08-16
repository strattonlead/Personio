using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class BaseResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }
}
