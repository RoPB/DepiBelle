using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Config
    {
        [JsonProperty("discount")]
        public int Discount
        {
            get;
            set;
        }
    }
}
