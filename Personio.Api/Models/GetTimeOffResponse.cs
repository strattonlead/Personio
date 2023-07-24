﻿using Newtonsoft.Json;

namespace Personio.Api.Models
{
    public class GetTimeOffResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "date")]
        public object Data { get; set; }
#warning TODO
    }
}
