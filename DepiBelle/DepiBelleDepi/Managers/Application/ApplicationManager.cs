using System;
using System.Threading.Tasks;
using DepiBelleDepi.Models.PushNotifications;
using DepiBelleDepi.Services.Authentication;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Navigation;
using DepiBelleDepi.Services.PushNotifications;

namespace DepiBelleDepi.Managers.Application
{
    public class ApplicationManager : IPushNotificableApplicationManager, IAuthenticableApplicationManager
    {
        private INavigationService _navigationService;
        private IConfigService _configService;
        private IAuthenticationService _authenticationService;
        private IPushNotificationsService _pushNotificationService;

        public ApplicationManager()
        {
            _navigationService = _navigationService ?? DependencyContainer.Resolve<INavigationService>();
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
            _pushNotificationService = _pushNotificationService ?? DependencyContainer.Resolve<IPushNotificationsService>();

        }

        public async Task Initialize(PushNotification pushNotification=null)
        {
            await _navigationService.InitializeAsync();
        }

        public async Task HandlePushNotification(bool openedByTouchNotification, bool isAppInBackground, PushNotification pushNotification)
        {

        }

        public async Task TrySendToken()
        {
            var token = _pushNotificationService.GetToken();
        }


        public async Task Login(string user, string password)
        {
            try
            {
                //_authenticationService.Initialize(_configService.AppToken);
                _authenticationService.Initialize();

                await _authenticationService.Authenticate(user, password);
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un problema de autenticación");
            }

        }

    }
}
