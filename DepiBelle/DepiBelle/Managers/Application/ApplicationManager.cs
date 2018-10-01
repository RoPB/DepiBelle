using System;
using System.Threading.Tasks;
using DepiBelle.Services.Authentication;
using DepiBelle.Services.Config;
using DepiBelle.Services.Dialog;


namespace DepiBelle.Managers.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private IConfigService _configService;
        private IAuthenticationService _authenticationService;
        private IDialogService _dialogService;


        public ApplicationManager()
        {
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
            _dialogService = _dialogService ?? DependencyContainer.Resolve<IDialogService>();
            _authenticationService.Initialize(_configService.AppToken);
        }

        public async Task Login(string user, string password)
        {
            try
            {
                await _authenticationService.Authenticate(user, password);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Se produjo un problema de autenticación", "ERROR", "OK");
            }

        }


    }
}
