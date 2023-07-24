using Newtonsoft.Json;

namespace Personio.Api.Models
{
    public class UpdateData
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
