using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DepiBelle.Models
{
    public class CartItemsGrouped:ObservableCollection<PurchasableListItem>
    {
        public string Name { get; set; }

        public CartItemsGrouped(string name, List<PurchasableListItem> listItems)
        {
            this.Name = name;
            if (listItems != null)
                foreach (PurchasableListItem item in listItems)
                    Items.Add(item);
        }
    }
}
