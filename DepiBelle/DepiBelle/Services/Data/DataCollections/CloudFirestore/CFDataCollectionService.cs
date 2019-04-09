using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelle.Models;
using Plugin.CloudFirestore;
using System.Linq;
using Newtonsoft.Json;

namespace DepiBelle.Services.Data
{
    public class CFDataCollectionService<T> : IDataCollectionService<T> where T  : EntityBase, new()
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
            try
            {
                var items = await CrossCloudFirestore.Current
                                        .Instance.GetCollection(Key)
                                        .GetDocumentsAsync();

                return items.ToObjects<T>().ToList();
            }
            catch(Exception ex)
            {
                throw ex; 
            }

        }

        public virtual async Task<T> Get(string id, string token = null)
        {
            try
            {
                var item = await CrossCloudFirestore.Current
                                        .Instance.GetCollection(Key)
                                        .GetDocument(id)
                                        .GetDocumentAsync();

                return item.ToObject<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> AddOrReplace(T item, bool autoKey = true, string token = null)
        {
            try
            {
                //TODO

                //IS ALWAYS ADDING AN AUTOKEY
                //if (autoKey)
                //    item.Id = string.Empty;

                //GET ID OF CREATED ITEM?

                if (string.IsNullOrEmpty(item.Id))
                {
                    await CrossCloudFirestore.Current
                                        .Instance.GetCollection(Key)
                                        .AddDocumentAsync(item);
                }
                else
                {
                    await CrossCloudFirestore.Current
                                        .Instance.GetCollection(Key)
                                        .GetDocument(item.Id)
                                        .UpdateDataAsync(item);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> Remove(string id, string token = null)
        {
            try
            {
                await CrossCloudFirestore.Current
                                        .Instance.GetCollection(Key)
                                        .GetDocument(id)
                                        .DeleteDocumentAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public virtual async Task<bool> RemoveAll(string token = null)
        {
            try
            {
                //TODO
                //COMO BORRO TODOS?

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
