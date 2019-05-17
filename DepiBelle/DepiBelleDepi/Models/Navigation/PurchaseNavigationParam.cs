using System;
namespace DepiBelleDepi.Models
{
    public class PurchaseNavigationParam
    {
        public Order Order { get; set; }

        public bool ToAttend { get; set; }

        public int CantItemsAdded { get { return Order.Promotions.Count + Order.Offers.Count; } }
    }
}
