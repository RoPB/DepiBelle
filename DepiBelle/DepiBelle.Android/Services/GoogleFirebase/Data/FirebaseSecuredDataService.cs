using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelle.Services.Authentication;
using Firebase.Database;
using DepiBelle.Models;

namespace DepiBelle.Droid.Services.GoogleFirebase.Data
{
    public class FirebaseSecuredDataService<T> : FirebaseDataService<T> where T : Entity
    {
        private IAuthenticationService _authenticationService;

        public FirebaseSecuredDataService()
        {

            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
        }


        public override async Task<List<T>> GetAll(string token = null)
        {
            try
            {
                token = _authenticationService.Token;
                var items = await base.GetAll(token);
                return items;
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.GetAll(_authenticationService.Token);
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

        public override async Task<T> Get(string id, string token = null)
        {
            try
            {
                token = _authenticationService.Token;
                var item = await base.Get(id, token);
                return item;
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.Get(id, _authenticationService.Token);
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

        public override async Task<bool> AddOrReplace(T item, bool autoKey = true, string token = null)
        {
            try
            {
                token = _authenticationService.Token;
                return await base.AddOrReplace(item, autoKey, token);
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.AddOrReplace(item, autoKey, token);
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

        public override async Task<bool> Remove(string id, string token = null)
        {
            try
            {
                token = _authenticationService.Token;
                return await base.Remove(id, token);
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.Remove(id, token);
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
