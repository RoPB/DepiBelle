using System;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class PurchasableItem:BaseListItem
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public int Discount
        {
            get;
            set;
        }

        public double SellPrice
        {
            get { return HasDiscount ? Price * ((double)Discount / 100) : Price; }
        }

        [JsonIgnore]
        public bool HasDiscount
        {
            get { return Discount > 0; }
        }
    }
}
