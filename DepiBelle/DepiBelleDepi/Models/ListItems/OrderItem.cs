using System;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelleDepi.Models
{
    public class OrderItem:BaseListItem
    {
        private string _time;
        private string _name;

        public string Id
        {
            get;
            set;
        }

        public string Time
        {

            get { return _time; }
            set { _time = value; RaisePropertyChanged(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        [JsonIgnore]
        [Ignored]
        public ICommand OnAttendCommand
        {
            get;
            set;
        }

        [JsonIgnore]
        [Ignored]
        public ICommand OnBlockCommand
        {
            get;
            set;
        }


    }
}
