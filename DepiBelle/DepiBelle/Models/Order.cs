using System;
using System.Collections.Generic;

namespace DepiBelle.Models
{
    public class Order : Entity
    {
        public int number { get; set; }

        public int disccount { get; set; } = 0;

        public List<Offer> offers { get; set; } = new List<Offer>();

        public Promotion promotion { get; set; }

    }
}
