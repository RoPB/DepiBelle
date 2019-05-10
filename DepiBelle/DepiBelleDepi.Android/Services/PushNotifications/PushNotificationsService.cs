using System;
using DepiBelleDepi.Services.PushNotifications;
using Firebase.Iid;

namespace DepiBelleDepi.Droid.Services.PushNotifications
{
    public class PushNotificationsService : IPushNotificationsService
    {
        public string GetToken()
        {
            return FirebaseInstanceId.Instance.Token;
        }

        public bool IsValidToken()
        {
            return FirebaseInstanceId.Instance != null && !String.IsNullOrEmpty(FirebaseInstanceId.Instance.Token);
        }
    }
}
