using System;
namespace DepiBelle.Models
{
    public class PurchasableListItem:BaseListItem
    {
        public string Id
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Discount
        {
            get;
            set;
        }

        public double PriceWithDisccount
        {
            get { return HasDiscount ? Price * ((double)Discount / 100) : Price; }
        }

        public bool HasDiscount
        {
            get { return Discount > 0; }
        }
    }
}
