using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Models.Bindable;
using DepiBelle.Models.EventArgs;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Services.Data.LocalData;
using DepiBelle.Utilities;

namespace DepiBelle.ViewModels
{
    public class PurchaseViewModel : ViewModelBase
    {

        private IConfigService _configService;
        private IDataService<Order> _ordersDataService;
        private ILocalDataService _localDataService;

        private string _strItemsAdded = "";
        private int _itemsAdded = 0;

        private ObservableCollection<AffordableItemsGrouped> _affordableItems = new ObservableCollection<AffordableItemsGrouped>();
        public ObservableCollection<AffordableItemsGrouped> AffordableItems
        {
            get { return _affordableItems; }
            set { SetPropertyValue(ref _affordableItems, value); }
        }

        private List<BaseListItem> _promotions = new List<BaseListItem>();
        private List<BaseListItem> _offers = new List<BaseListItem>();

        public ICommand PromotionSelectedCommand { get; set; }
        public ICommand OfferSelectedCommand { get; set; }

        public string StrItemsAdded { get { return _strItemsAdded; } set { SetPropertyValue(ref _strItemsAdded, value); } }

        public PurchaseViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataService<Order>>();
            _localDataService = _localDataService ?? DependencyContainer.Resolve<ILocalDataService>();
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            IsLoading = false;
        }

        public void ItemsAddedHandler(object sender, AffordableItem<Promotion> itemAdded)
        {
            HandleItemAdded(itemAdded.Added);

            var promotion = ListItemMapper.GetPromotionListItem(itemAdded.Item, true, OfferSelectedCommand);
            _promotions.Add(promotion);

            LoadAffordableItems();
        }

        public void ItemsAddedHandler(object sender, AffordableItem<Offer> itemAdded)
        {
            HandleItemAdded(itemAdded.Added);

            var offer = ListItemMapper.GetOfferListItem(itemAdded.Item, true, OfferSelectedCommand);
            _offers.Add(offer);

            LoadAffordableItems();
        }

        private void LoadAffordableItems(){
            AffordableItems.Clear();
            AffordableItems.Add(new AffordableItemsGrouped("Promociones", _promotions));
            AffordableItems.Add(new AffordableItemsGrouped("Cuerpo", _offers));
        }

        private void HandleItemAdded(bool added)
        {
            _itemsAdded += added ? 1 : -1;
            StrItemsAdded = _itemsAdded == 0 ? string.Empty : "" + _itemsAdded;
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

            var localDataOrder = new DataOrder();
            localDataOrder.Date = date;
            localDataOrder.LastNumber = 0;

            var isAnyLocalOrderSaved = await _localDataService.Contains(key);

            if (isAnyLocalOrderSaved)
            {
                localDataOrder = await _localDataService.Get<DataOrder>(key);
                if (localDataOrder.Date.Equals(date))
                    ++localDataOrder.LastNumber;
                else
                    await _localDataService.Remove(key);
            }

            await _localDataService.AddOrReplace<DataOrder>(key, localDataOrder);

            await _ordersDataService.AddOrReplace(new Order() { Number = localDataOrder.LastNumber });
        }

    }
}
