using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class UpdateData
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
