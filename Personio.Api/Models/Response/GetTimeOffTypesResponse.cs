using Personio.Api.Models.Attributes;
using System;

namespace Personio.Api.Models.Response
{
    public class GetTimeOffTypesResponse : BasePagedListResponse<TimeOffTypeAttributes, TimeOffType>
    {
        protected override Func<TimeOffTypeAttributes, TimeOffType> Converter => x => x.ToTimeOffType();
    }
}
