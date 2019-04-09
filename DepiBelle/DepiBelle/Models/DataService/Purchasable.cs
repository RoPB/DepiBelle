using System;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelle.Models
{
    public class Purchasable : EntityBase
    {

        [MapTo("name")]
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [MapTo("price")]
        [JsonProperty("price")]
        public double Price
        {
            get;
            set;
        }

        public Purchasable()
        {

        }

        public Purchasable(string id, string name, double price) : base(id)
        {
            this.Name = name;
            this.Price = price;
        }
    }
}
