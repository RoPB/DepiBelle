using System;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelle.Models
{
    public class Offer : Purchasable
    {
        //this two properties so 
        //serialization wont serialize category 
        //but deserialization will load category
        [MapTo("category")]
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

        public Offer()
        {

        }

        public Offer(string id, double price, string name) : base(id, name, price)
        {

        }

    }
}
