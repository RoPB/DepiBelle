using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Purchasable: EntityBase
    {
        [JsonProperty("price")]
        public double Price
        {
            get;
            set;
        }

        public Purchasable(string id, double price) : base(id)
        {
            this.Price = price;
        }
    }
}
