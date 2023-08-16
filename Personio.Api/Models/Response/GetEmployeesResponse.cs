using Personio.Api.Models.Attributes;
using System;

namespace Personio.Api.Models.Response
{
    public class GetEmployeesResponse : BasePagedListResponse<EmployeeAttributes, Employee>
    {
        protected override Func<EmployeeAttributes, Employee> Converter => x => x.ToEmployee();
    }
}
