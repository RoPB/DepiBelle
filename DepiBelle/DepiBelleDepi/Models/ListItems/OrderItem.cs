using System;
namespace DepiBelleDepi.Models
{
    public class OrderItem:BaseListItem
    {
        public string Id
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
