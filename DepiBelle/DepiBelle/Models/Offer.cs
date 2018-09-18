using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Offer : Entity
    {
        public string name
        {
            get;
            set;
        }
        public double price
        {
            get;
            set;
        }

        //this two properties so 
        //serialization wont serialize category 
        //but deserialization will load category
        [JsonIgnore]
        public string category
        {
            get;
            set;
        }
        [JsonProperty("category")]
        private string categorySetter
        {
            set { category = value; }
        }

    }
}
