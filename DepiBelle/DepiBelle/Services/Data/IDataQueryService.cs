using System;
using System.Threading.Tasks;
using DepiBelle.Models;

namespace DepiBelle.Services.Data
{
    public interface IDataQueryService<T>
    {
        bool Initialize(DataServiceConfig config);

        Task<T> Get(string token = null);
    }
}
