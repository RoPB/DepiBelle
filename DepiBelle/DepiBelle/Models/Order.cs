using System;
using System.Collections.Generic;

namespace DepiBelle.Models
{
    public class Order : Entity
    {
        public int number { get; set; }

        public List<Offer> offers { get; set; } = new List<Offer>();

        public List<Promotion> promotions { get; set; } = new List<Promotion>();

    }
}
