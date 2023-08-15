using Newtonsoft.Json;
using Personio.Api.Models.Attributes;

namespace Personio.Api.Models.Response
{
    public class EmployeeResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public TypeAndAttributesObject<EmployeeAttributes>[] Data { get; set; }
    }
}
