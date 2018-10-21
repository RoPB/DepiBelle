using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    public class PromotionItem : PurchasableItem
    {
        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }
    }
}
