using Newtonsoft.Json;

namespace Personio.Api.Models.Request
{
    public class CreateEmployeeRequest
    {
        [JsonProperty(PropertyName = "employee")]
        public Employee Employee { get; set; }
    }
}
