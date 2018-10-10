using System;
namespace DepiBelle.Models
{
    public class CartItem<T> where T : Purchasable
    {
        public bool Added
        {
            get;
            set;
        }

        public int Discount
        {
            get;
            set;
        }

        public T Item
        {
            get;
            set;
        }
    }
}
