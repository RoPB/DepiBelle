using System;
using Android.Content;
using DepiBelleDepi.Models.PushNotifications;
using Firebase.Messaging;
using Newtonsoft.Json;

namespace DepiBelleDepi.Droid.Helpers.PushNotifications
{
    public class PushNotificationHelper
    {
        public static PushNotification TryGetPushNotification(Intent intent)
        {

            PushNotification pushNotification = null;

            if (intent != null && intent.Extras != null && intent.HasExtra(PushNotificationDataFlags.Flag)){

                var serializedPushNot=intent.GetStringExtra(PushNotificationDataFlags.Flag);
                pushNotification = JsonConvert.DeserializeObject<PushNotification>(serializedPushNot);
            }

            return pushNotification;
        }

        public static PushNotification TryGetPushNotification(RemoteMessage message)
        {

            PushNotification pushNotification = null;

            if (message.Data != null && message.Data.ContainsKey(PushNotificationDataFlags.Flag))
            {
                var badge = 0;
                var type=PushNotificationType.Unknown;
                var title = "";
                var body = "";

                if (message.Data.ContainsKey(PushNotificationDataFlags.Badge))
                    Int32.TryParse(message.Data[PushNotificationDataFlags.Badge], out badge);

                if (message.Data.ContainsKey(PushNotificationDataFlags.Type))
                    Enum.TryParse<PushNotificationType>(message.Data[PushNotificationDataFlags.Type], out type);

                if (message.Data.ContainsKey(PushNotificationDataFlags.Title))
                    title = message.Data[PushNotificationDataFlags.Title];

                if (message.Data.ContainsKey(PushNotificationDataFlags.Body))
                    body = message.Data[PushNotificationDataFlags.Body];

                pushNotification = new PushNotification() { Title = title, Body = body, Badge = badge, Type = type};

                //Add Data
                if (message.Data.ContainsKey(PushNotificationDataFlags.Item))
                    pushNotification.Data.Add(PushNotificationDataFlags.Item, message.Data[PushNotificationDataFlags.Item]);
            }

            return pushNotification;
        }

    }
}
