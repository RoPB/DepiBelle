using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class PromotionItem:PurchasableItem
    {
        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }
    }
}
