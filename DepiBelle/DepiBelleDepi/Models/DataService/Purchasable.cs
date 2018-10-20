using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    public class Purchasable : EntityBase
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

        public Purchasable(string id, string name, double price) : base(id)
        {
            this.Name = name;
            this.Price = price;
        }
    }
}
