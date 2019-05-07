using System;
using System.Collections.Generic;

namespace DepiBelleDepi.Models
{
    public class PromotionsNavigationParam
    {
        public List<PurchasableItem> PromotionsAdded { get; set; }

        public bool ShowAddRemoveButtons { get; set; }
    }
}
