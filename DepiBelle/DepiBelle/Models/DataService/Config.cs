using System;
using Newtonsoft.Json;

namespace DepiBelle.Models.DataService
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
