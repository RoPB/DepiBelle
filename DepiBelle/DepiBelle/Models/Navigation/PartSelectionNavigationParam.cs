using System;
using System.Collections.Generic;

namespace DepiBelle.Models
{
    public class PartSelectionNavigationParam
    {
        public List<string> SelectedOffers { get; set; } = new List<string>();

        public List<Offer> Offers { get; set; }

        public int Discount { get; set; }

        public string Title { get; set; }
    }
}
