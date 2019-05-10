using System;
using System.Threading.Tasks;
using Android.App;
using DepiBelleDepi.Managers.Application;
using Firebase.Iid;

namespace DepiBelleDepi.Droid.Services.PushNotifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseRegistrationService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            //FirebaseInstanceId.Instance.Token;
            DependencyContainer.Resolve<IPushNotificableApplicationManager>().TrySendToken();
        }
    }
}
