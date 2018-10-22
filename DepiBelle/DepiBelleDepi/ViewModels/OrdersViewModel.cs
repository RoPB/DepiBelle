using System;
using System.Threading.Tasks;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data;
using DepiBelleDepi.Services.Data.DataQuery;
using DepiBelleDepi.Utilities;

namespace DepiBelleDepi.ViewModels
{
    public class OrdersViewModel:ViewModelBase
    {
        private IConfigService _configService;
        private IApplicationManager _applicationMananger;
        private IDataCollectionService<Order> _ordersDataService;

        public OrdersViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();
            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataCollectionService<Order>>();


        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(_configService.User, _configService.Password);

            var date = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{_configService.Orders}/{date}" });
            await _ordersDataService.Subscribe(async (obj) =>
            {

            });
            var orders = await _ordersDataService.GetAll();

            IsLoading = false;
        }
    }
}
