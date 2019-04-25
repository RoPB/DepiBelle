using System;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelleDepi.Models
{
    public class BaseListItem : BindableBase
    {
        private bool _isSelected = false;

        [JsonIgnore]
        [Ignored]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(); }
        }

        [JsonIgnore]
        [Ignored]
        public ICommand OnSelectedCommand
        {
            get;
            set;
        }
    }
}
