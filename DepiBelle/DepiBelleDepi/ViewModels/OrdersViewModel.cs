using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data;
using DepiBelleDepi.Utilities;
using Xamarin.Forms;

namespace DepiBelleDepi.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private IApplicationManager _applicationMananger;
        private IDataCollectionService<Order> _ordersDataService;

        private Dictionary<string, Order> _dicOrders = new Dictionary<string, Order>();
        private List<Order> _pendingOrders = new List<Order>();
        private int _pendingOrdersCount;
        private bool _showPendingOrders;

        private ObservableCollection<OrderItem> _orders = new ObservableCollection<OrderItem>();

        public ObservableCollection<OrderItem> Orders
        {
            get { return _orders; }
            set { SetPropertyValue(ref _orders, value); }
        }

        public int PendingOrdersCount
        {
            get { return _pendingOrdersCount; }
            set { SetPropertyValue(ref _pendingOrdersCount, value); }
        }

        public bool ShowPendingOrders
        {
            get { return _showPendingOrders; }
            set { SetPropertyValue(ref _showPendingOrders, value); }
        }

        public ICommand AddPendingOrdersToMainListCommand { get; set; }

        public OrdersViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();
            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataCollectionService<Order>>();
            AddPendingOrdersToMainListCommand = new Command(async () => await AddPendingOrdersToMainList());

        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(_configService.User, _configService.Password);

            var date = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{_configService.OrdersInProcess}/{date}" });

            await _ordersDataService.Subscribe(async (obj) =>
            {
                await OnOrderCollectionModified(obj);
            });

            var orders = await _ordersDataService.GetAll();

            await LoadOrders(orders);

            IsLoading = false;
        }

        private Task LoadOrders(List<Order> orders)
        {
            return Task.Run(() =>
            {
                orders = orders.OrderBy(o => o.Number).ToList();

                orders.ForEach(o =>
                {
                    AddOrderToMainList(o);
                });
            });

        }

        private Task OnOrderCollectionModified(ServiceSubscriberEventParam<Order> param)
        {
            return Task.Run(() =>
            {

                var order = param.Item;

                if (param.Type.Equals(SubscriptionEventType.InsertOrUpdate))
                {
                    AddUpdateOrder(order);
                }
                else
                    RemoveOrder(order.Id);
            });


        }

        private void AddUpdateOrder(Order order)
        {
            //ADD
            if (!_dicOrders.ContainsKey(order.Id) && !_pendingOrders.Any(o => o.Id == order.Id))
            {
                AddOrderToPendingList(order);
            }
            //THERE WONT BE UPDATES
        }

        private Task RemoveOrder(string id)
        {
            return Task.Run(async () =>
            {
                var removeFromPending = await RemoveOrderFromPendingList(id);
                if (!removeFromPending)
                    await RemoveOrderFromMainList(id);

            });
        }

        private void AddOrderToMainList(Order order)
        {

            if (!_dicOrders.ContainsKey(order.Id))
            {
                _dicOrders.Add(order.Id, order);
                Orders.Insert(0,GetOrderListItem(order));
            }
        }

        private Task RemoveOrderFromMainList(string id)
        {
            return Task.Run(() =>
            {
                var orderToDelete = Orders.Where(o => o.Id == id).FirstOrDefault();
                if (orderToDelete != null)
                {
                    Orders.Remove(orderToDelete);
                    _dicOrders.Remove(id);
                }
            });
        }

        private Task AddOrderToPendingList(Order order)
        {
            return Task.Run(() =>
            {

                _pendingOrders.Add(order);
                UpdatePendingCount();
            });
        }

        private Task<bool> RemoveOrderFromPendingList(string id)
        {
            return Task.Run(() =>
            {

                var pendingOrderToDelete = _pendingOrders.Where(o => o.Id == id).FirstOrDefault();

                if (pendingOrderToDelete != null)
                {
                    _pendingOrders.Remove(pendingOrderToDelete);
                    UpdatePendingCount();
                    return true;
                }

                return false;
            });
        }

        private Task AddPendingOrdersToMainList()
        {
            return Task.Run(() =>
            {
                while (_pendingOrders.Count > 0)
                {
                    var order = _pendingOrders[0];
                    AddOrderToMainList(order);
                    _pendingOrders.RemoveAt(0);
                    UpdatePendingCount();
                }
            });

        }

        private void UpdatePendingCount()
        {
            PendingOrdersCount = _pendingOrders.Count;
            ShowPendingOrders = PendingOrdersCount > 0;
        }

        private OrderItem GetOrderListItem(Order order)
        {
            return new OrderItem() { Id = order.Id, Number = order.Number, Name = order.Name }; ;
        }
    }
}
