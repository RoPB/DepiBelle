using System;
using System.Windows.Input;
using Newtonsoft.Json;

namespace DepiBelle.Models
{
    public class BaseListItem:BindableBase
    {
        private bool _isSelected = false;

        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set{ _isSelected = value; RaisePropertyChanged(); }
        }

        public ICommand OnSelectedCommand
        {
            get;
            set;
        }
    }
}
