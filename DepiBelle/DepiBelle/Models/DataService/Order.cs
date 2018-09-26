using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Order : EntityBase
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("offers")]
        public List<Offer> Offers { get; set; } = new List<Offer>();

        [JsonProperty("promotions")]
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();

    }
}
