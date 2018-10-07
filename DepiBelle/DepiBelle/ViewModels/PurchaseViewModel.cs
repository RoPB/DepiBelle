using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
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

        private ObservableCollection<BaseListItem> _affordableItems = new ObservableCollection<BaseListItem>();
        public ObservableCollection<BaseListItem> AffordableItems
        {
            get { return _affordableItems; }
            set { SetPropertyValue(ref _affordableItems, value); }
        }

        private ObservableCollection<PromotionListItem> _promotions = new ObservableCollection<PromotionListItem>();
        private ObservableCollection<OfferListItem> _offers = new ObservableCollection<OfferListItem>();

        public ObservableCollection<PromotionListItem> Promotions
        {
            get { return _promotions; }
            set { SetPropertyValue(ref _promotions, value); }
        }

        public ObservableCollection<OfferListItem> Offers
        {
            get { return _offers; }
            set { SetPropertyValue(ref _offers, value); }
        }

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
            Promotions.Add(promotion);
        }

        public void ItemsAddedHandler(object sender, AffordableItem<Offer> itemAdded)
        {
            HandleItemAdded(itemAdded.Added);
            var offer = ListItemMapper.GetOfferListItem(itemAdded.Item, true, OfferSelectedCommand);
            Offers.Add(offer);
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
