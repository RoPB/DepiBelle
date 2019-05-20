using System;
using System.Threading.Tasks;
using DepiBelleDepi.Models;
using DepiBelleDepi.Models.PushNotifications;
using DepiBelleDepi.Services.Authentication;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data;
using DepiBelleDepi.Services.Navigation;
using DepiBelleDepi.Services.PushNotifications;
using Newtonsoft.Json;

namespace DepiBelleDepi.Managers.Application
{
    public class ApplicationManager : IPushNotificableApplicationManager, IAuthenticableApplicationManager
    {
        private INavigationService _navigationService;
        private IConfigService _configService;
        private IAuthenticationService _authenticationService;
        private IPushNotificationsService _pushNotificationService;
        private IDataCollectionService<User> _usersService;

        private static string _loggedUser = "";

        public ApplicationManager()
        {
            _navigationService = _navigationService ?? DependencyContainer.Resolve<INavigationService>();
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
            _pushNotificationService = _pushNotificationService ?? DependencyContainer.Resolve<IPushNotificationsService>();
            _usersService = _usersService ?? DependencyContainer.Resolve<IDataCollectionService<User>>();
            _usersService.Initialize(new DataServiceConfig() { Uri = _configService.ServiceUri, Key = _configService.DepilatorUsers });
        }

        public async Task Initialize(PushNotification pushNotification=null)
        {

            if (pushNotification != null)
            {
                await HandlePushNotification(true,true, pushNotification);
            }
            else
            {
                await _navigationService.InitializeAsync();
            }
                
        }

        public async Task HandlePushNotification(bool openedByTouchNotification, bool isAppInBackground, PushNotification pushNotification)
        {
            if (openedByTouchNotification)
            {
                if(pushNotification.LinkeableItem != null)
                {
                    var linkeableItem = JsonConvert.DeserializeObject<LinkeableItem>(pushNotification.LinkeableItem);

                    await LinkeableItemsHelper.HandleLink(_navigationService, linkeableItem);
                }

            }

        }

        public async Task TrySendToken()
        {
            if (string.IsNullOrEmpty(_loggedUser) || !_pushNotificationService.IsValidToken())
                return;

            var token = _pushNotificationService.GetToken();
            await _usersService.AddOrReplace(new User() {Id= token, Email = _loggedUser, PushToken = token },false);
        }


        public async Task Login(string user, string password)
        {
            try
            {
                //_authenticationService.Initialize(_configService.AppToken);
                _authenticationService.Initialize();
                await _authenticationService.Authenticate(user, password);
                _loggedUser = user;
                await TrySendToken();
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un problema de autenticación");
            }

        }

    }
}
