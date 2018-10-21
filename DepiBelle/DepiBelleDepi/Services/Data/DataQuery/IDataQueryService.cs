using System;
using System.Threading.Tasks;
using DepiBelleDepi.Models;

namespace DepiBelleDepi.Services.Data.DataQuery
{
    public interface IDataQueryService<T>
    {
        bool Initialize(DataServiceConfig config);

        Task<T> Get(string token = null);
    }
}
