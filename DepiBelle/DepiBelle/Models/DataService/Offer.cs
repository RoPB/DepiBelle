using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Offer : Purchasable
    {
        [JsonProperty("name")]
        public string Name
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

        public Offer(string id, double price, string name) : base(id,price)
        {
            this.Name = name;
        }

    }
}
