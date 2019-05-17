using System;
using System.Collections.Generic;

namespace DepiBelleDepi.Models
{
    public class BodySelectionNavigationParam
    {
        public Config Config { get; set; }

        public List<OfferItem> OffersAdded { get; set; }

        public bool ToAttend { get; set; }
    }
}
