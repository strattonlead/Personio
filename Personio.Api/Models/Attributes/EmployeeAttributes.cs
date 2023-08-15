using Newtonsoft.Json;
using Personio.Api.Models.Response;

namespace Personio.Api.Models.Attributes
{
    public class EmployeeAttributes
    {
        [JsonProperty(PropertyName = "id")]
        public AttributeObject<int> Id { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public AttributeObject<string> FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public AttributeObject<string> LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public AttributeObject<string> Email { get; set; }

        public static implicit operator Employee(EmployeeAttributes x)
            => new Employee()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            };

        public Employee ToEmployee() => (Employee)this;

    }
}
