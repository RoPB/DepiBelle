using System;
using System.Threading.Tasks;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Services.Data.LocalData;
using DepiBelle.Utilities;

namespace DepiBelle.ViewModels
{
	public class PurchaseViewModel:ViewModelBase
    {

        private IConfigService _configService;
        private IDataService<Order> _ordersDataService;
        private ILocalDataService _localDataService;

        public PurchaseViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataService<Order>>();
            _localDataService = _localDataService ?? DependencyContainer.Resolve<ILocalDataService>();
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            await AddOrders();
            IsLoading = false;
        }

        private async Task GetOrders()
        {
            var key = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{_configService.Orders}/{key}" });
            var orders = await _ordersDataService.GetAll();
        }

        private async Task AddOrders()
        {

            var date = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{_configService.Orders}/{date}" });

            await AddOrder(date);
            await AddOrder(date);
            await AddOrder(date);
        }

        private async Task AddOrder(string date)
        {
            var key = Constants.Constants.LOCAL_DATA_ORDER_KEY;

            var localDataOrder = new LocalDataOrder();
            localDataOrder.date = date;
            localDataOrder.lastNumber = 0;

            var isAnyLocalOrderSaved = await _localDataService.Contains(key);

            if (isAnyLocalOrderSaved){
                localDataOrder = await _localDataService.Get<LocalDataOrder>(key);
                if (localDataOrder.date.Equals(date))
                    ++localDataOrder.lastNumber;
                else
                    await _localDataService.Remove(key);
            }


            await _localDataService.AddOrReplace<LocalDataOrder>(key, localDataOrder);

            await _ordersDataService.AddOrReplace(new Order() { number = localDataOrder.lastNumber });
        }

    }
}
