using System;
using System.Threading.Tasks;
using DepiBelle.Configuration;
using DepiBelle.Models;
using DepiBelle.Services.Data;
using DepiBelle.Services.Data.LocalData;
using DepiBelle.Utilities;
using Plugin.Settings;

namespace DepiBelle.ViewModels
{
	public class PurchaseViewModel:ViewModelBase
    {

        private IDataService<Order> _ordersDataService;
        private ILocalDataService<KeyValue<int>> _localDataService;

        public PurchaseViewModel()
        {
            IsLoading = true;
            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataService<Order>>();
            _localDataService = _localDataService ?? DependencyContainer.Resolve<ILocalDataService<KeyValue<int>>>();
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            IsLoading = false;
        }

        private async Task GetOrders()
        {
            var key = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = $"{DataServiceConstants.ORDERS}/{key}" });
            var orders = await _ordersDataService.GetAll();
        }

        private async Task AddOrders()
        {

            var key = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = $"{DataServiceConstants.ORDERS}/{key}" });
            await AddOrder(key);
            await AddOrder(key);
            await AddOrder(key);
        }

        private async Task AddOrder(string key)
        {
            var localFileKey = DataServiceConstants.ORDERS;
            var number = 0;
            if (!CrossSettings.Current.Contains(key, localFileKey))
                CrossSettings.Current.Clear(localFileKey);//all other days
            else
                number = CrossSettings.Current.GetValueOrDefault(key, 0, localFileKey);

            CrossSettings.Current.AddOrUpdateValue(key, ++number, localFileKey);

            await _ordersDataService.AddOrReplace(new Order() { number = number });
        }

    }
}
