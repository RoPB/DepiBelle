using System;
using Newtonsoft.Json;

namespace DepiBelle.Models.DataService
{
    public class Config
    {
        [JsonProperty("disscount")]
        public int Disscount
        {
            get;
            set;
        }
    }
}
