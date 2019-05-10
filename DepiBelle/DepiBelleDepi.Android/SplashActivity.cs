using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace DepiBelleDepi.Droid
{
    [Activity(
        Icon = "@mipmap/icon",
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            InvokeMainActivity();
        }

        private void InvokeMainActivity()
        {
            var intent = new Intent(this, typeof(MainActivity));
            if (this.Intent != null && this.Intent.Extras != null)
                intent.PutExtras(this.Intent.Extras);

            StartActivity(intent);
        }
    }
}
