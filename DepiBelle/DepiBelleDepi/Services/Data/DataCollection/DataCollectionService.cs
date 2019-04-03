using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelleDepi.Models;
using Firebase.Database;
using Firebase.Database.Streaming;
using Newtonsoft.Json;

namespace DepiBelleDepi.Services.Data
{
    public class DataCollectionService<T> : IDataCollectionService<T> where T : EntityBase
    {

        private IDisposable _subscriptor;
        private DataServiceConfig Config { get; set; }
        protected string Uri { get { return Config.Uri; } }
        protected string Key { get { return Config.Key; } }


        public bool Initialize(DataServiceConfig config)
        {
            if (Config == null)
                Config = config;

            Task.Run(async () => await UnSubscribe());

            return true;
        }

        private bool IsServiceInitialized()
        {
            if (Config == null)
                throw new Exception("Have to Initialize Collection Data Service");

            return true;
        }

        private FirebaseClient CreateClient(string token = null)
        {
            IsServiceInitialized();

            var options = new FirebaseOptions();

            if (!string.IsNullOrEmpty(token))
                options.AuthTokenAsyncFactory = new Func<Task<string>>(() => { return Task.Run(() => token); });

            return new FirebaseClient(Uri, options);
        }

        public virtual async Task<List<T>> GetAll(string token = null)
        {
            try
            {
                var items = new List<T>();

                var client = CreateClient(token);

                var serviceItems = await client.Child(Key).OnceAsync<T>();

                foreach (var serviceItem in serviceItems)
                {
                    var item = GetItem(serviceItem);
                    items.Add(item);
                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public virtual async Task<T> Get(string id, string token = null)
        {
            try
            {
                var items = new List<T>();

                var client = CreateClient(token);

                var serviceItem = await client.Child($"{Key}/{id}").OnceSingleAsync<T>();

                serviceItem.Id = id;

                return serviceItem;

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
                var items = new List<T>();

                var client = CreateClient(token);

                if (autoKey)
                {
                    item.Id = string.Empty;
                    await client.Child(Key).PostAsync(JsonConvert.SerializeObject(item), true);
                }
                else

                    await client.Child($"{Key}/{item.Id}").PutAsync(JsonConvert.SerializeObject(item));

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
                var items = new List<T>();

                var client = CreateClient(token);

                await client.Child($"{Key}/{id}").DeleteAsync();

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual Task<bool> RemoveAll(string token = null)
        {
            throw new NotImplementedException();
        }


        public virtual Task<bool> Subscribe(Action<ServiceSubscriberEventParam<T>> action, string token = null)
        {
            try
            {
                if (_subscriptor == null)
                {
                    var items = new List<T>();

                    var client = CreateClient(token);

                    _subscriptor = client.Child(Key).AsObservable<T>().Subscribe(elem =>
                    {
                        var type = elem.EventType.Equals(FirebaseEventType.Delete) ? SubscriptionEventType.Delete : SubscriptionEventType.InsertOrUpdate;

                        action.Invoke(new ServiceSubscriberEventParam<T>() { Item = GetItem(elem), Type = type });
                    });
                }

                return Task.Run(() => true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UnSubscribe()
        {
            try
            {
                if (_subscriptor != null)
                {
                    _subscriptor.Dispose();
                    _subscriptor = null;
                }

                return Task.Run(() => true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private T GetItem(FirebaseObject<T> firebaseObject)
        {

            var item = firebaseObject.Object;
            item.Id = firebaseObject.Key;

            return item;
        }


    }
}
