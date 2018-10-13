using System;
using Newtonsoft.Json;

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
        public double SellPrice
        {
            get { return HasDiscount ? Price - ((double)Discount * Price  / 100) : Price; }
        }

        [JsonIgnore]
        public bool HasDiscount
        {
            get { return Discount > 0; }
        }
    }
}
