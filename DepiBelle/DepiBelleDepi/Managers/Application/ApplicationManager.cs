using System;
using System.Threading.Tasks;
using DepiBelleDepi.Services.Authentication;
using DepiBelleDepi.Services.Config;

namespace DepiBelleDepi.Managers.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private IConfigService _configService;
        private IAuthenticationService _authenticationService;


        public ApplicationManager()
        {
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
           
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
