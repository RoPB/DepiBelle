using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class User
    {
        [JsonProperty("userId")]
        public string UserId{ get; set; }

        [JsonProperty("visitCount")]
        public int VisitCount { get; set; }

        [JsonProperty("totalSpend")]
        public double TotalSpend { get; set; }
    }
}
