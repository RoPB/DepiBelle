using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Offer : EntityBase
    {
        [JsonProperty("name")]
        public string Name
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

        //this two properties so 
        //serialization wont serialize category 
        //but deserialization will load category
        [JsonIgnore]
        public string Category
        {
            get;
            set;
        }
        [JsonProperty("category")]
        private string CategorySetter
        {
            set { Category = value; }
        }

    }
}
