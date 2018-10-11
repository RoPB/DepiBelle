using System;
namespace DepiBelle.Models
{
    public class OfferListItem:PurchasableListItem
    {

        public string Image
        {
            get { return $"{this.Name}.png"; }           
        }   
    }
}
