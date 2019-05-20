using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    public class DeepLink
    {
        [JsonProperty("navTo")]
        public string NavTo { get; set; }

        [JsonProperty("param")]
        public string Param { get; set; }
    }
}
