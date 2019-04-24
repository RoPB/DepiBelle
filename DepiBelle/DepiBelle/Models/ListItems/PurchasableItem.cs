using System;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelle.Models
{
    public class PurchasableItem:BaseListItem
    {
        [JsonProperty("id")]
        public string Id
        {
            get;
            set;
        }

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

        [JsonProperty("discount")]
        public int Discount
        {
            get;
            set;
        }

        [JsonProperty("sellPrice")]
        [Ignored]
        public double SellPrice
        {
            get { return HasDiscount ? Price - ((double)Discount * Price  / 100) : Price; }
        }

        [JsonIgnore]
        [Ignored]
        public bool HasDiscount
        {
            get { return Discount > 0; }
        }
    }
}
