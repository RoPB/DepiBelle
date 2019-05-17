using System;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelle.Models
{
    public class OfferItem:PurchasableItem
    {
        [JsonIgnore]
        [Ignored]
        public string Image
        {
            get { return $"{this.Name}.png"; }           
        }   
    }
}
