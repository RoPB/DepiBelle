using System;
using System.Threading.Tasks;
using DepiBelle.Services.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DepiBelle
{
    public partial class App : Application
    {
        private INavigationService _navigationService;

        public App()
        {
            InitializeComponent();
            _navigationService = _navigationService ?? DependencyContainer.Resolve<INavigationService>();
            _navigationService.InitializeAsync();
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
