using System;
using System.Threading.Tasks;
using DepiBelle.Configuration;
using DepiBelle.Models;
using DepiBelle.Services.Authentication;
using DepiBelle.Services.Data;
using DepiBelle.Utilities;
using Plugin.Settings;

namespace DepiBelle.Managers.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private IAuthenticationService _authenticationService;


        public ApplicationManager()
        {
            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
            _authenticationService.Initialize(DataServiceConstants.APP_TOKEN);
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
