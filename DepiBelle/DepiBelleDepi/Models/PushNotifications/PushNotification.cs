using System;
using System.Collections.Generic;

namespace DepiBelleDepi.Models.PushNotifications
{
    public class PushNotification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int Badge { get; set; }
        public bool IsContentAvailablePresent { get; set; }
        public string Item { get; set; }
    }
}
