using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelleDepi.Models
{
    public class Order : EntityBase
    {
        [MapTo("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [MapTo("time")]
        [JsonProperty("time")]
        public string Time { get; set; }

        [MapTo("offers")]
        [JsonProperty("offers")]
        public List<PurchasableItem> Offers { get; set; } = new List<PurchasableItem>();

        [MapTo("promotions")]
        [JsonProperty("promotions")]
        public List<PurchasableItem> Promotions { get; set; } = new List<PurchasableItem>();

        [MapTo("total")]
        [JsonProperty("total")]
        public double Total { get; set; }

    }
}
