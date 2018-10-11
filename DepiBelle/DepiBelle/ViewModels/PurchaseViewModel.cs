using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Services.Data.LocalData;
using DepiBelle.Utilities;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PurchaseViewModel : ViewModelBase
    {

        private IConfigService _configService;
        private IDataCollectionService<Order> _ordersDataService;
        private ILocalDataService _localDataService;

        private string _strItemsAdded = "";
        private int _itemsAdded = 0;
        private List<PurchasableListItem> _promotions = new List<PurchasableListItem>();
        private List<PurchasableListItem> _offers = new List<PurchasableListItem>();
        private bool _isNoAnyPurchasableItemAdded = false;
        private double _total;

        private ObservableCollection<CartItemsGrouped> _purchasableItems = new ObservableCollection<CartItemsGrouped>();
        public ObservableCollection<CartItemsGrouped> PurchasableItems
        {
            get { return _purchasableItems; }
            set { SetPropertyValue(ref _purchasableItems, value); }
        }


        public double Total
        {
            get { return _total; }
            set { SetPropertyValue(ref _total, value); }
        }

        public bool IsNoAnyPurchasableItemAdded
        {
            get { return _isNoAnyPurchasableItemAdded; }
            set { SetPropertyValue(ref _isNoAnyPurchasableItemAdded, value); }
        }

        public ICommand PromotionSelectedCommand { get; set; }
        public ICommand OfferSelectedCommand { get; set; }
        public EventHandler<string> PromotionRemoved { get; set; }
        public EventHandler<string> OfferRemoved { get; set; }

        public string StrItemsAdded { get { return _strItemsAdded; } set { SetPropertyValue(ref _strItemsAdded, value); } }

        public PurchaseViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataCollectionService<Order>>();
            _localDataService = _localDataService ?? DependencyContainer.Resolve<ILocalDataService>();
            PromotionSelectedCommand = new Command<PromotionListItem>(PromotionSelected);
            OfferSelectedCommand = new Command<OfferListItem>(OfferSelected);
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            IsLoading = false;
            IsNoAnyPurchasableItemAdded = PurchasableItems.Count == 0;
        }

        public void ItemsAddedHandler(object sender, CartItem<Promotion> itemAdded)
        {
            HandleItemsAddedBadge(itemAdded.Added);

            if (itemAdded.Added)
            {
                var promotion = ListItemMapper.GetPromotionListItem(itemAdded.Item, true, PromotionSelectedCommand);
                _promotions.Add(promotion);
            }
            else
                _promotions.RemoveAll(p => p.Id == itemAdded.Item.Id);

            LoadPurchasableItems();
        }

        public void ItemsAddedHandler(object sender, CartItem<Offer> itemAdded)
        {
            HandleItemsAddedBadge(itemAdded.Added);

            if (itemAdded.Added)
            {
                var offer = ListItemMapper.GetOfferListItem(itemAdded.Item, itemAdded.Discount, true, OfferSelectedCommand);
                _offers.Add(offer);
            }
            else
                _offers.RemoveAll(o => o.Id == itemAdded.Item.Id);

            LoadPurchasableItems();
        }

        private void PromotionSelected(PromotionListItem promotion)
        {
            _promotions.Remove(promotion);
            HandleItemsAddedBadge(false);
            LoadPurchasableItems();
            PromotionRemoved.Invoke(this, promotion.Id);

        }

        private void OfferSelected(OfferListItem offer)
        {
            _offers.Remove(offer);
            HandleItemsAddedBadge(false);
            LoadPurchasableItems();
            OfferRemoved.Invoke(this, offer.Id);
        }

        private void HandleItemsAddedBadge(bool added)
        {
            _itemsAdded += added ? 1 : -1;
            StrItemsAdded = _itemsAdded == 0 ? string.Empty : "" + _itemsAdded;
        }

        private void LoadPurchasableItems()
        {
            PurchasableItems.Clear();

            _promotions = _promotions.OrderBy(p => p.Name).ToList();
            _offers = _offers.OrderBy(o => o.Name).ToList();

            if (_promotions.Count > 0)
                PurchasableItems.Add(new CartItemsGrouped("Promociones", _promotions));
            if (_offers.Count > 0)
                PurchasableItems.Add(new CartItemsGrouped("Cuerpo", _offers));


            Total = 0;

            _promotions.ForEach(p => Total += p.SellPrice);
            _offers.ForEach(o => Total += o.SellPrice);

            IsNoAnyPurchasableItemAdded = PurchasableItems.Count == 0;

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
