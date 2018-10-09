using System;
using System.Threading.Tasks;
using Firebase.Database;
using DepiBelle.Services.Authentication;

namespace DepiBelle.Droid.Services.GoogleFirebase.Data
{
    public class DataQuerySecuredService<T>:DataQueryService<T>
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
                token = _authenticationService.Token;
                var item = await base.Get(token);
                return item;
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
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
                else
                {
                    throw fe;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
