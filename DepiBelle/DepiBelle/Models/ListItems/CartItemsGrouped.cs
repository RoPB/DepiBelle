using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DepiBelle.Models
{
    public class CartItemsGrouped:ObservableCollection<BaseListItem>
    {
        public string Name { get; set; }

        public CartItemsGrouped(string name, List<BaseListItem> listItems)
        {
            this.Name = name;
            if (listItems != null)
                foreach (BaseListItem item in listItems)
                    Items.Add(item);
        }
    }
}
