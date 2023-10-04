using Personio.Api;
using Personio.Api.Models.Request;
using System.Net;
using Xunit;

namespace Personio.Tests
{
    public class EmployeeTests : BaseUnitTest
    {
        [Fact]
        public void GetEmployeesTest()
        {
            var request = new GetEmployeesRequest();
            var employees = PersonioClient.GetEmployeesAsync(request).Result;
            Assert.Equal(HttpStatusCode.OK, employees.StatusCode);

            employees = PersonioClient.GetEmployees(request);
            Assert.Equal(HttpStatusCode.OK, employees.StatusCode);
        }

        [Fact]
        public void GetEmployeesErrorTest()
        {
            var request = new GetEmployeesRequest() { Limit = 1000 };
            var employees = PersonioClient.GetEmployeesAsync(request).Result;

            Assert.NotEqual(HttpStatusCode.OK, employees.StatusCode);
            Assert.Null(employees.PagedList);
        }
    }
}