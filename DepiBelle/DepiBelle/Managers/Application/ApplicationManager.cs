using System;
using System.Threading.Tasks;
using DepiBelle.Models;
using DepiBelle.Services.Authentication;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Utilities;
using Plugin.Settings;

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
        }

        public async Task Login(string user, string password)
        {
            try
            {
                await _authenticationService.Authenticate(user, password);
            }
            catch (Exception ex)
            {

            }

        }


    }
}
