using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Promotion : Purchasable
    {

        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }

        public Promotion(string id, double price, string name, string description) : base(id,price)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
