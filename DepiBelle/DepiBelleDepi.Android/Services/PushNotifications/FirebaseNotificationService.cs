using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using DepiBelleDepi.Droid.Helpers.PushNotifications;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models.PushNotifications;
using Firebase.Messaging;
using Newtonsoft.Json;

namespace DepiBelleDepi.Droid.Services.PushNotifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            var pushNotification = PushNotificationHelper.TryGetPushNotification(message);
            if (pushNotification != null)
            {
                if (!MainApplication.IsForeground())
                    ShowNotification(pushNotification);

                Task.Run(async () => await DependencyContainer.Resolve<IPushNotificableApplicationManager>().
                                        HandlePushNotification(false, MainActivity.IsAppInBackground, pushNotification));
            }

        }

        public void ShowNotification(PushNotification pushNotification)
        {
            var intent = new Intent(this, typeof(SplashActivity));
            intent.AddFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

            var jsonPushNotification = JsonConvert.SerializeObject(pushNotification);
            intent.PutExtra(PushNotificationDataFlags.Flag, jsonPushNotification);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.ic_cart)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetContentText(pushNotification.Body)
                .SetContentTitle(pushNotification.Title)
                .SetNumber(pushNotification.Badge)//for Android 8.0
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true);

            var notificationManager = NotificationManager.FromContext(this);
            var uniqueid = (int)((DateTime.Now.Ticks / 1000L) % Int32.MaxValue);
            notificationManager.Notify(uniqueid, notificationBuilder.Build());


        }


    }
}
