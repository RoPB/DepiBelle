using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models.PushNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DepiBelleDepi
{
    public partial class App : Application
    {
        private IPushNotificableApplicationManager _applicationManager;

        public App(PushNotification pushNotification=null)
        {
            InitializeComponent();
            _applicationManager = _applicationManager ?? DependencyContainer.Resolve<IPushNotificableApplicationManager>();
            _applicationManager.Initialize(pushNotification);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
