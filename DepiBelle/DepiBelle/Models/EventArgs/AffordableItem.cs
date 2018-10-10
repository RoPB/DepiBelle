using System;
namespace DepiBelle.Models.EventArgs
{
    public class AffordableItem<T>
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
