using Personio.Api.Models.Attributes;
using System;

namespace Personio.Api.Models.Response
{
    public class GetTimeOffPeriodsResponse : BasePagedListResponse<TimeOffPeriodAttributes, TimeOffPeriod>
    {
        protected override Func<TimeOffPeriodAttributes, TimeOffPeriod> Converter => x => x.ToTimeOffPeriod();
    }
}
