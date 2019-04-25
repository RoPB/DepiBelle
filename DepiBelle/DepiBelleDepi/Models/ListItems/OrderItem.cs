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
        private bool _isBeingAttended;
        private bool _isBeingAttendedByUser;

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
        public bool IsBeingAttended
        {
            get { return _isBeingAttended; }
            set { _isBeingAttended = value; RaisePropertyChanged(); }
        }

        [JsonIgnore]
        [Ignored]
        public bool IsBeingAttendedByUser
        {
            get { return _isBeingAttendedByUser; }
            set { _isBeingAttendedByUser = value; RaisePropertyChanged(); }
        }

       [JsonIgnore]
        [Ignored]
        public ICommand OnAttendCommand
        {
            get;
            set;
        }

    }
}
