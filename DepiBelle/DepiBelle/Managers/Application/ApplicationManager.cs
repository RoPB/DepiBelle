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
      
        public ApplicationManager()
        {
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
            _authenticationService.Initialize(_configService.AppToken);
            //_authenticationService.Initialize(_configService.AppName);
        }

        public async Task Login(string user, string password)
        {
            try
            {
                await _authenticationService.Authenticate(user, password);
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un problema de autenticación");
            }

        }


    }
}
