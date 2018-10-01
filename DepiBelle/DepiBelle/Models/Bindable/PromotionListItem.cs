using System;
namespace DepiBelle.Models
{
    public class PromotionListItem:BaseListItem
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public double Price
        {
            get;
            set;
        }
    }
}
