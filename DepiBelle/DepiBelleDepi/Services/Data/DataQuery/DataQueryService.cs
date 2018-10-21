using System;
using System.Net.Http;
using System.Threading.Tasks;
using DepiBelleDepi.Models;
using Newtonsoft.Json;

namespace DepiBelleDepi.Services.Data.DataQuery
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

        public virtual async Task<T> Get(string token = null)
        {
            try
            {
                IsServiceInitialized();

                var auth = token != null ? $"?auth={token}" : string.Empty;

                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Config.Uri + Config.Key + "/.json" + auth);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string serialized = await response.Content.ReadAsStringAsync();

                    T result = await Task.Run(() =>
                        JsonConvert.DeserializeObject<T>(serialized));

                    return result;
                }
                else throw new NotAuthorizedException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
