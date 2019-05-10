using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Lottie.Forms.Droid;
using Xamarin.Forms;
using Plugin.Badge.Droid;
using Acr.UserDialogs;
using DepiBelleDepi.Droid.Helpers.PushNotifications;
using System.Threading.Tasks;
using DepiBelleDepi.Managers.Application;
using Android.Content;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
namespace DepiBelleDepi.Droid
{
    [Activity(Label = "DepiBelleDepi", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static bool _isAppInBackground = false;
        private static bool _newIntent = false;
        public static bool IsAppInBackground
        {
            get { return _isAppInBackground; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            InitializeLibraries(savedInstanceState);

            ResolveDependencies();

            var pushNotification = PushNotificationHelper.TryGetPushNotification(this.Intent);
            _newIntent = false;

            LoadApplication(new App(pushNotification));
        }

        private void InitializeLibraries(Bundle savedInstanceState)
        {
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AnimationViewRenderer.Init();
            UserDialogs.Init(this);

            Plugin.CloudFirestore.CloudFirestore.Init(this);
            Plugin.FirebaseAuth.FirebaseAuth.Init(this);
        }

        private void ResolveDependencies()
        {
            AndroidDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _isAppInBackground = true;
        }

        protected override void OnResume()
        {
            base.OnResume();
            _isAppInBackground = false;
            if (_newIntent)
            {
                var pushNotificationParameter = PushNotificationHelper.TryGetPushNotification(this.Intent);

                if (pushNotificationParameter != null)
                {
                    Task.Run(async () => await DependencyContainer.Resolve<IPushNotificableApplicationManager>().
                                            HandlePushNotification(true, IsAppInBackground, pushNotificationParameter));
                }

                _newIntent = false;
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            this.Intent = intent;
            _newIntent = true;

        }

    }
}