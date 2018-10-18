using System;
namespace DepiBelle.Models
{
    public class CartItem<T> where T : Purchasable
    {
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
