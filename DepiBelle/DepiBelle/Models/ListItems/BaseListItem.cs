using System;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelle.Models
{
    public class BaseListItem:BindableBase
    {
        private bool _isSelected = false;

        [Ignored]
        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set{ _isSelected = value; RaisePropertyChanged(); }
        }
        [Ignored]
        [JsonIgnore]
        public ICommand OnSelectedCommand
        {
            get;
            set;
        }
    }
}
