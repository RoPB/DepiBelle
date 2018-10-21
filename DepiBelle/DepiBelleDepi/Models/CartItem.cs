using System;
namespace DepiBelleDepi.Models
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
