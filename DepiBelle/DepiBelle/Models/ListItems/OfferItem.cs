using System;

namespace DepiBelle.Models
{
    public class OfferItem:PurchasableItem
    {
        public string Image
        {
            get { return $"{this.Name}.png"; }           
        }   
    }
}
