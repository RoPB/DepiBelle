using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Promotion:EntityBase
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }
        [JsonProperty("price")]
        public double Price
        {
            get;
            set;
        }
    }
}
