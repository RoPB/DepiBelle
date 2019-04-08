using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelle.Models;
using Plugin.CloudFirestore;
using System.Linq;

namespace DepiBelle.Services.Data.DataCollections.CloudFirestore
{
    public class CFDataCollectionService<T> : IDataCollectionService<T> where T : EntityBase
    {

        private DataServiceConfig Config { get; set; }
        protected string Uri { get { return Config.Uri; } }
        protected string Key { get { return Config.Key; } }

        public virtual bool Initialize(DataServiceConfig config)
        {
            if (Config == null)
                Config = config;

            return true;
        }

        public virtual async Task<List<T>> GetAll(string token = null)
        {
            var documents = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("yourcollection")
                                        .GetDocumentsAsync();

            return documents.ToObjects<T>().ToList();
        }

        public virtual Task<T> Get(string id, string token = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> AddOrReplace(T item, bool autoKey = true, string token = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Remove(string id, string token = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> RemoveAll(string token = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Subscribe(Action<ServiceSubscriberEventParam<T>> action, string token = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> UnSubscribe()
        {
            throw new NotImplementedException();
        }
    }
}
