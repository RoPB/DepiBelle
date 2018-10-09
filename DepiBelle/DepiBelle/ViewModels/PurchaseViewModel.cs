using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Models.Bindable;
using DepiBelle.Models.EventArgs;
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
        private List<BaseListItem> _promotions = new List<BaseListItem>();
        private List<BaseListItem> _offers = new List<BaseListItem>();
        private bool _isNoAnyAffordableItemAdded = false;

        private ObservableCollection<AffordableItemsGrouped> _affordableItems = new ObservableCollection<AffordableItemsGrouped>();
        public ObservableCollection<AffordableItemsGrouped> AffordableItems
        {
            get { return _affordableItems; }
            set { SetPropertyValue(ref _affordableItems, value); }
        }

        public bool IsNoAnyAffordableItemAdded
        {
            get { return _isNoAnyAffordableItemAdded; }
            set { SetPropertyValue(ref _isNoAnyAffordableItemAdded, value); }
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
            IsNoAnyAffordableItemAdded = AffordableItems.Count == 0;
        }

        public void ItemsAddedHandler(object sender, AffordableItem<Promotion> itemAdded)
        {
            HandleItemsAddedBadge(itemAdded.Added);

            if (itemAdded.Added)
            {
                var promotion = ListItemMapper.GetPromotionListItem(itemAdded.Item, true, PromotionSelectedCommand);
                _promotions.Add(promotion);
            }
            else
                _promotions.RemoveAll(p => ((PromotionListItem)p).Id == itemAdded.Item.Id);

            LoadAffordableItems();
        }

        public void ItemsAddedHandler(object sender, AffordableItem<Offer> itemAdded)
        {
            HandleItemsAddedBadge(itemAdded.Added);

            if (itemAdded.Added)
            {
                var offer = ListItemMapper.GetOfferListItem(itemAdded.Item, true, OfferSelectedCommand);
                _offers.Add(offer);
            }
            else
                _offers.RemoveAll(o => ((OfferListItem)o).Id == itemAdded.Item.Id);

            LoadAffordableItems();
        }

        private void PromotionSelected(PromotionListItem promotion)
        {
            _promotions.Remove(promotion);
            HandleItemsAddedBadge(false);
            LoadAffordableItems();
            PromotionRemoved.Invoke(this, promotion.Id);

        }

        private void OfferSelected(OfferListItem offer)
        {
            _offers.Remove(offer);
            HandleItemsAddedBadge(false);
            LoadAffordableItems();
            OfferRemoved.Invoke(this, offer.Id);
        }

        private void HandleItemsAddedBadge(bool added)
        {
            _itemsAdded += added ? 1 : -1;
            StrItemsAdded = _itemsAdded == 0 ? string.Empty : "" + _itemsAdded;
        }

        private void LoadAffordableItems()
        {
            AffordableItems.Clear();

            _promotions = _promotions.OrderBy(p => ((PromotionListItem)p).Name).ToList();
            _offers = _offers.OrderBy(o => ((OfferListItem)o).Name).ToList();

            if (_promotions.Count > 0)
                AffordableItems.Add(new AffordableItemsGrouped("Promociones", _promotions));
            if (_offers.Count > 0)
                AffordableItems.Add(new AffordableItemsGrouped("Cuerpo", _offers));

            IsNoAnyAffordableItemAdded = AffordableItems.Count == 0;
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
