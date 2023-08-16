using Personio.Api;
using Personio.Api.Models.Request;
using System;
using System.Linq;
using Xunit;

namespace Personio.Tests
{
    public class AbsenceTests : BaseUnitTest
    {
        [Fact]
        public void AbsenceTypesTest()
        {
            var offTypesResponse = PersonioClient.GetTimeOffTypesAsync(new GetTimeOffTypesRequest()).Result;
            Assert.NotNull(offTypesResponse);
        }

        [Fact]
        public void AbsencesTest()
        {
            var offTypesResponse = PersonioClient.GetTimeOffsAsync(new GetTimeOffsRequest() { Limit = 10 }).Result;
            Assert.NotNull(offTypesResponse);
        }

        [Fact]
        public void CreateTimeOffTest()
        {
            /* Geht nur STundenbasiert! Steht falsch in der API mit den Required Parametern!! */
            var employee = PersonioClient.GetEmployeesAsync(new GetEmployeesRequest()).Result.PagedList.Data.FirstOrDefault();
            var timeOffType = PersonioClient.GetTimeOffsAsync(new GetTimeOffsRequest()).Result.PagedList.Data.FirstOrDefault();
            var result = PersonioClient.CreateTimeOffAsync(new CreateTimeOffRequest()
            {
                StartDate = DateTime.UtcNow.AddDays(16),
                EndDate = DateTime.UtcNow.AddDays(16),
                EmployeeId = employee.Id.Value,
                TimeOffTypeId = timeOffType.TimeOffTypeId
            }).Result;
        }
    }
}