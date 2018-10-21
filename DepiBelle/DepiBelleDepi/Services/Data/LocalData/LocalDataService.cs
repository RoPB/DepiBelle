using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;

namespace DepiBelleDepi.Services.Data.LocalData
{
    public class LocalDataService : ILocalDataService
    {
        public Task<bool> Contains(string key)
        {
            return Task.Run(() => CrossSettings.Current.Contains(key));
        }

        public Task<T> Get<T>(string key)
        {
            return Task.Run(() => { return JsonConvert.DeserializeObject<T>(CrossSettings.Current.GetValueOrDefault(key, string.Empty)); });

        }

        public Task<bool> AddOrReplace<T>(string key, T item)
        {
            return Task.Run(() => { return CrossSettings.Current.AddOrUpdateValue(key,JsonConvert.SerializeObject(item)); });
        }

        public Task<bool> Remove(string key)
        {
            return Task.Run(() => { CrossSettings.Current.Remove(key); return true; });
        }
    }
}
