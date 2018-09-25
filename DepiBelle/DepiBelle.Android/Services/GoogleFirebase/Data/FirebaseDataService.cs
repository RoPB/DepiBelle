using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepiBelle.Models;
using DepiBelle.Services.Data;
using Firebase.Database;
using Newtonsoft.Json;

namespace DepiBelle.Droid.Services.GoogleFirebase.Data
{
    public class FirebaseDataService<T> : IDataService<T> where T : EntityBase
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
                throw new Exception("Have to Initialize Data Service");

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

        public virtual async Task<bool> AddOrReplace(T item, bool autoKey=true, string token = null)
        {
            try
            {
                var items = new List<T>();

                var client = CreateClient(token);

                if(autoKey)

                    await client.Child(Key).PostAsync(JsonConvert.SerializeObject(item), true);

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

        public virtual bool Subscribe(Action<T> action, string token = null)
        {
            try
            {
                var items = new List<T>();

                var client = CreateClient(token);

                client.Child(Key).AsObservable<T>().Subscribe(elem =>
                {
                    action.Invoke(GetItem(elem));
                });

                return true;

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
