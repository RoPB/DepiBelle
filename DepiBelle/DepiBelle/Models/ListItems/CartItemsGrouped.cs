using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DepiBelle.Models
{
    public class CartItemsGrouped:ObservableCollection<PurchasableItem>
    {
        public string Name { get; set; }

        public CartItemsGrouped(string name, List<PurchasableItem> listItems)
        {
            this.Name = name;
            if (listItems != null)
                foreach (PurchasableItem item in listItems)
                    Items.Add(item);
        }
    }
}
