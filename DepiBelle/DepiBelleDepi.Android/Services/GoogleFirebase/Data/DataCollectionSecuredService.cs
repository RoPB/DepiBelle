using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Authentication;
using Firebase.Database;

namespace DepiBelleDepi.Droid.Services.GoogleFirebase.Data
{
    public class DataCollectionSecuredService<T> : DataCollectionService<T> where T : EntityBase
    {
        private IAuthenticationService _authenticationService;

        public DataCollectionSecuredService()
        {

            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
        }


        public override async Task<List<T>> GetAll(string token = null)
        {
            try
            {
                var items = await base.GetAll(_authenticationService.Token);
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
                var item = await base.Get(id, _authenticationService.Token);
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
                return await base.AddOrReplace(item, autoKey, _authenticationService.Token);
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.AddOrReplace(item, autoKey, _authenticationService.Token);
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
                return await base.Remove(id, _authenticationService.Token);
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.Remove(id, _authenticationService.Token);
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

        public override async Task<bool> RemoveAll(string token = null)
        {
            try
            {
                return await base.RemoveAll(_authenticationService.Token);
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.RemoveAll(_authenticationService.Token);
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

        public override async Task<bool> Subscribe(Action<ServiceSubscriberEventParam<T>> action, string token = null)
        {

            try
            {
                return await base.Subscribe(action, _authenticationService.Token);
            }
            catch (FirebaseException fe)
            {
                if (fe.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.Subscribe(action, _authenticationService.Token);
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
