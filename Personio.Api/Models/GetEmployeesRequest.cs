using System;
using System.Collections.Generic;

namespace Personio.Api.Models
{
    public class GetEmployeesRequest
    {
        /// <summary>
        /// Minvalue = 1
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Minvalue = 0
        /// </summary>
        public int Offset { get; set; }
        public string Email { get; set; }
        public DateTime? UpdatedSince { get; set; }
        public IEnumerable<string> Attributes { get; set; }
    }
}
