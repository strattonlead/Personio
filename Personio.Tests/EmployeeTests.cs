using Personio.Api;
using Personio.Api.Models.Request;
using Xunit;

namespace Personio.Tests
{
    public class EmployeeTests : BaseUnitTest
    {
        [Fact]
        public void AuthTest()
        {
            var request = new GetEmployeesRequest();
            var employees = PersonioClient.GetEmployeesAsync(request).Result;
            Assert.Equal(10, employees.Count);
        }
    }
}