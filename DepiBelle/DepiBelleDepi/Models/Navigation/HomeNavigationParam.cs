using System;
namespace DepiBelleDepi.Models
{
    public class HomeNavigationParam
    {
        public Order Order { get; set; }
        public bool ToAttend { get; set; }
        public bool IsNewOrder { get { return string.IsNullOrEmpty(Order.Id); } }
    }
}
