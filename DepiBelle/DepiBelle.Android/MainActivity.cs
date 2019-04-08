using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Plugin.Badge.Droid;
using Lottie.Forms.Droid;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BadgedTabbedPageRenderer))]
namespace DepiBelle.Droid
{
    [Activity(Label = "DepiBelle", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            ResolveDependencies();
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AnimationViewRenderer.Init();

            Plugin.CloudFirestore.CloudFirestore.Init(this);

            LoadApplication(new App());
            //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(this,
            //                                                               new UXDivers.Gorilla.Config("Good Gorilla")
            //                                                                .RegisterAssembly(typeof(FFImageLoading.Forms.CachedImage).Assembly)
            //                                                                .RegisterAssembly(typeof(FFImageLoading.Svg.Forms.SvgCachedImage).Assembly)
            //                                                                .RegisterAssembly(typeof(DepiBelle.App).Assembly)));
        }
                                                                            

        private void ResolveDependencies()
        {
            AndroidDependencyContainer.RegisterDependencies();
            DependencyContainer.RegisterDependencies();
        }


        public override void OnBackPressed()
        {
            //I disable hardware back button

            /*if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }*/
        }
    }
}