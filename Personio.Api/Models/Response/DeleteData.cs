using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class DeleteData
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
