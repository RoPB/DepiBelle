using System;
namespace DepiBelle.Models
{
    public class OfferListItem:BaseListItem
    {

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

        public bool ShowPriceWithDisccount
        {
            get;
            set;
        }

        public double PriceWithDisccount
        {
            get;
            set;
        }

    }
}
