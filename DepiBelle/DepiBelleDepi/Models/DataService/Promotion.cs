using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    public class Promotion : Purchasable
    {

        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }

        public Promotion(string id, double price, string name, string description) : base(id, name, price)
        {
            this.Description = description;
        }
    }
}
