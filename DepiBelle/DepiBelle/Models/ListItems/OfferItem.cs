using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class OfferItem:PurchasableItem
    {
        [JsonIgnore]
        public string Image
        {
            get { return $"{this.Name}.png"; }           
        }   
    }
}
