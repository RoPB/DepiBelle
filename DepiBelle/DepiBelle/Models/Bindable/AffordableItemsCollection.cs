using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DepiBelle.Models.Bindable
{
    public class AffordableItemsCollection<T>:ObservableCollection<T>
    {
        public string Name { get; set; }

        public AffordableItemsCollection(string name, List<T> affordableItems)
        {
            this.Name = name;
            if (affordableItems != null)
                foreach (T item in affordableItems)
                    Items.Add(item);
        }
    }
}
