using Newtonsoft.Json;

namespace Personio.Api.Models
{
    public class UpdateEmployeeRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "employee")]
        public Employee Employee { get; set; }
    }
}
