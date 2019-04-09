using System;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelle.Models
{
    public class Promotion : Purchasable
    {
    
        [MapTo("description")]
        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }

        public Promotion()
        {

        }

        public Promotion(string id, double price, string name, string description) : base(id,name,price)
        {
            this.Description = description;
        }
    }
}
