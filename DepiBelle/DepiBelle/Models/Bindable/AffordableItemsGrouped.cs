using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DepiBelle.Models.Bindable
{
    public class AffordableItemsGrouped:ObservableCollection<BaseListItem>
    {
        public string Name { get; set; }

        public AffordableItemsGrouped(string name, List<BaseListItem> affordableItems)
        {
            this.Name = name;
            if (affordableItems != null)
                foreach (BaseListItem item in affordableItems)
                    Items.Add(item);
        }
    }
}
