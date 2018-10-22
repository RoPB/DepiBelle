using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data;
using DepiBelleDepi.Services.Data.DataQuery;
using DepiBelleDepi.Utilities;

namespace DepiBelleDepi.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private IApplicationManager _applicationMananger;
        private IDataCollectionService<Order> _ordersDataService;

        private Dictionary<string, Order> _dicOrders = new Dictionary<string, Order>();

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
                await OnOrderCollectionModified(obj);
            });
            var orders = await _ordersDataService.GetAll();

            await LoadOrders(orders);

            IsLoading = false;
        }

        private async Task LoadOrders(List<Order> orders)
        {
            orders = orders.OrderBy(o => o.Number).ToList();

            orders.ForEach(o => _dicOrders.Add(o.Id, o));
        }

        private async Task OnOrderCollectionModified(ServiceSubscriberEventParam<Order> param)
        {
            var order = param.Item;

            if (param.Type.Equals(SubscriptionEventType.InsertOrUpdate))
            {

                await AddUpdateOrder(order);
            }
            else
                await RemoveOrder(order.Id);
        }

        private async Task AddUpdateOrder(Order order)
        {
            //ADD
            if (!_dicOrders.ContainsKey(order.Id)) 
            {
                _dicOrders.Add(order.Id, order);
            }
            //UPDATE
            else
            {
                _dicOrders[order.Id] = order;
            }
        }

        private async Task RemoveOrder(string id)
        {
            _dicOrders.Remove(id);
        }
    }
}
