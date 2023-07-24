﻿using System;
using System.Collections.Generic;

namespace Personio.Api.Models
{
    public class GetTimeOffsRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime UpdatedFrom { get; set; }
        public DateTime UpdatedTo { get; set; }
        public IEnumerable<int> EmployeeIds { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}
