using System;
namespace DepiBelle.Models
{
    public class BaseListItem:BindableBase
    {
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set{ _isSelected = value; RaisePropertyChanged(); }
        }
    }
}
