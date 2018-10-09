using System;
using System.Threading.Tasks;
using DepiBelle.Models;
using DepiBelle.Services.Data;

namespace DepiBelle.Droid.Services.GoogleFirebase.Data
{
    public class DataQueryService<T> : IDataQueryService<T>
    {

        private DataServiceConfig Config { get; set; }
        protected string Uri { get { return Config.Uri; } }
        protected string Key { get { return Config.Key; } }


        public bool Initialize(DataServiceConfig config)
        {
            if (Config == null)
                Config = config;

            return true;
        }

        private bool IsServiceInitialized()
        {
            if (Config == null)
                throw new Exception("Have to Initialize Query Data Service");

            return true;
        }

        public virtual Task<T> Get(string token = null)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
