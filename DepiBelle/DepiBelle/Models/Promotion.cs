using System;
namespace DepiBelle.Models
{
    public class Promotion:Entity
    {
        public string name
        {
            get;
            set;
        }
        public string description
        {
            get;
            set;
        }
        public double price
        {
            get;
            set;
        }
    }
}
