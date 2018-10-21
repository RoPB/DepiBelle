using System;
using System.Threading.Tasks;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Authentication;

namespace DepiBelleDepi.Services.Data.DataQuery
{
    public class DataQuerySecuredService<T> : DataQueryService<T>
    {

        private IAuthenticationService _authenticationService;

        public DataQuerySecuredService()
        {

            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
        }

        public override async Task<T> Get(string token = null)
        {
            try
            {
                var item = await base.Get(_authenticationService.Token);
                return item;
            }
            catch (NotAuthorizedException)
            {

                try
                {
                    await _authenticationService.RefreshSession();

                    return await base.Get(_authenticationService.Token);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
