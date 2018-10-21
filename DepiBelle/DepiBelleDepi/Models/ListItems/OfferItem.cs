using System;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
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
