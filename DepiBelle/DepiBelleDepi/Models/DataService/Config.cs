using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
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
