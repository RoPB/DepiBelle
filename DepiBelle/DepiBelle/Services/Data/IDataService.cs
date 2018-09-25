using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelle.Models;

namespace DepiBelle.Services.Data
{
    public interface IDataService<T> where T : EntityBase
    {
        bool Initialize(DataServiceConfig config);

        Task<List<T>> GetAll(string token = null);

        Task<T> Get(string id, string token = null);

        Task<bool> AddOrReplace(T item, bool autoKey=true, string token = null);

        Task<bool> Remove(string id, string token = null);

        Task<bool> RemoveAll(string token = null);

    }
}
