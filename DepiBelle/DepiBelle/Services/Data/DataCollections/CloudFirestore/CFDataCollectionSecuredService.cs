using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelle.Models;
using DepiBelle.Services.Authentication;
using Plugin.CloudFirestore;

namespace DepiBelle.Services.Data
{
    public class CFDataCollectionSecuredService<T> : CFDataCollectionService<T> where T : EntityBase, new()
    {
        private IAuthenticationService _authenticationService;

        public CFDataCollectionSecuredService()
        {

            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
        }


        public override async Task<List<T>> GetAll(string token = null,
                                                  int limit = 20,
                                                  object offset = null,
                                                  QueryLike queryLike = null,
                                                  List<QueryOrderBy> querysOrderBy = null,
                                                  List<QueryWhere> querysWhere = null)
        {
            try
            {
                var items = await base.GetAll(_authenticationService.Token,
                                              limit, offset, queryLike, querysOrderBy, querysWhere);
                return items;
            }
            catch (CloudFirestoreException fe)
            {
                if (fe.ErrorType.Equals(ErrorType.PermissionDenied))
                {
                    try
                    {
                        await _authenticationService.RefreshSession();

                        return await base.GetAll(_authenticationService.Token,
                                                limit, offset, queryLike, querysOrderBy, querysWhere);
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
            catch (CloudFirestoreException fe)
            {
                if (fe.ErrorType.Equals(ErrorType.PermissionDenied))
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
            catch (CloudFirestoreException fe)
            {
                if (fe.ErrorType.Equals(ErrorType.PermissionDenied))
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
            catch (CloudFirestoreException fe)
            {
                if (fe.ErrorType.Equals(ErrorType.PermissionDenied))
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
            catch (CloudFirestoreException fe)
            {
                if (fe.ErrorType.Equals(ErrorType.PermissionDenied))
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
            catch (CloudFirestoreException fe)
            {
                if (fe.ErrorType.Equals(ErrorType.PermissionDenied))
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
