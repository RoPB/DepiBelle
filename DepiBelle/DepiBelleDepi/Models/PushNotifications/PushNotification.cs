using System;
using System.Collections.Generic;

namespace DepiBelleDepi.Models.PushNotifications
{
    public enum PushNotificationType { Unknown, NewOrder};

    public class PushNotification
    {
        public PushNotificationType Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Badge { get; set; }
        public bool IsContentAvailablePresent { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}
