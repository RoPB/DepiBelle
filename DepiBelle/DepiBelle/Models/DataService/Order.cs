using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class Order : EntityBase
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("offers")]
        public List<PurchasableItem> Offers { get; set; } = new List<PurchasableItem>();

        [JsonProperty("promotions")]
        public List<PurchasableItem> Promotions { get; set; } = new List<PurchasableItem>();

        [JsonProperty("total")]
        public double Total { get; set; }

    }
}
