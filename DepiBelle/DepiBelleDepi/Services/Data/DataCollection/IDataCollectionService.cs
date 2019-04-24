using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelleDepi.Models;

namespace DepiBelleDepi.Services.Data
{
    public interface IDataCollectionService<T> where T : EntityBase, new()
    {
        bool Initialize(DataServiceConfig config);

        Task<List<T>> GetAll(string token = null,
                                                  int limit = 20,
                                                  object offset = null,
                                                  QueryLike queryLike = null,
                                                  List<QueryOrderBy> querysOrderBy = null,
                                                  List<QueryWhere> querysWhere = null);

        Task<T> Get(string id, string token = null);

        Task<bool> AddOrReplace(T item, bool autoKey = true, string token = null);

        Task<bool> Remove(string id, string token = null);

        Task<bool> RemoveAll(string token = null);

        Task<bool> Subscribe(Action<ServiceSubscriberEventParam<T>> action, string token = null);

        Task<bool> UnSubscribe();

    }
}
