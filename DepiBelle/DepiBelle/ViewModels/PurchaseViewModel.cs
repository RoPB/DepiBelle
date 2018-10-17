using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Services.Notification;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Services.Data.LocalData;
using DepiBelle.Utilities;
using DepiBelle.ViewModels.Modals;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PurchaseViewModel : ViewModelBase
    {

        private IConfigService _configService;
        private IDataCollectionService<Order> _ordersDataService;
        private ILocalDataService _localDataService;

        private ICartNotificationService<Promotion> _cartPromotionManager;
        private ICartNotificationService<Offer> _cartOfferManager;

        private bool _uploadingOrder;
        private string _strItemsAdded = "";
        private int _itemsAdded = 0;
        private List<PurchasableItem> _promotions = new List<PurchasableItem>();
        private List<PurchasableItem> _offers = new List<PurchasableItem>();
        private bool _showMainContent = false;
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

        public bool ShowMainContent
        {
            get { return _showMainContent; }
            set { SetPropertyValue(ref _showMainContent, value); }
        }

        public ICommand PromotionSelectedCommand { get; set; }
        public ICommand OfferSelectedCommand { get; set; }
        public ICommand ConfirmPurchaseCommand { get; set; }

        public string StrItemsAdded { get { return _strItemsAdded; } set { SetPropertyValue(ref _strItemsAdded, value); } }

        public PurchaseViewModel()
        {
            IsLoading = true;
            ShowMainContent = PurchasableItems.Count > 0;
            _uploadingOrder = false;
            PromotionSelectedCommand = new Command<PromotionItem>(PromotionSelected);
            OfferSelectedCommand = new Command<OfferItem>(OfferSelected);
            ConfirmPurchaseCommand = new Command(async () => await ConfirmPurchase());

            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataCollectionService<Order>>();
            _localDataService = _localDataService ?? DependencyContainer.Resolve<ILocalDataService>();

            _cartPromotionManager = _cartPromotionManager ?? DependencyContainer.Resolve<ICartNotificationService<Promotion>>();
            _cartOfferManager = _cartOfferManager ?? DependencyContainer.Resolve<ICartNotificationService<Offer>>();

            _cartPromotionManager.ItemAdded += PromotionAddedHandler;
            _cartOfferManager.ItemAdded += OfferAddedHandler;

        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            IsLoading = false;
        }

        public void PromotionAddedHandler(object sender, CartItem<Promotion> itemAdded)
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

        public void OfferAddedHandler(object sender, CartItem<Offer> itemAdded)
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

        private void PromotionSelected(PromotionItem promotion)
        {
            _promotions.Remove(promotion);
            HandleItemsAddedBadge(false);
            LoadPurchasableItems();
            _cartPromotionManager.ItemRemoved.Invoke(this, promotion.Id);

        }

        private void OfferSelected(OfferItem offer)
        {
            _offers.Remove(offer);
            HandleItemsAddedBadge(false);
            LoadPurchasableItems();
            _cartOfferManager.ItemRemoved.Invoke(this, offer.Id);
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

            ShowMainContent = PurchasableItems.Count > 0;

        }

        private async Task ConfirmPurchase()
        {
            var order = new Order();
            order.Offers = _offers;
            order.Promotions = _promotions;
            order.Total = Total;
            order.Name = "Viviana";

            await UploadOrder(order);
        }

        private async Task UploadOrder(Order order)
        {
            var error = false;
            ModalViewModelBase viewModel = null;

            try
            {
                if (!_uploadingOrder)
                {
                    _uploadingOrder = true;
                    IsLoading = true;

                    Func<bool, Task> afterCloseModalFunction = OrderCompleted;
                    viewModel = await ModalService.PushAsync<ConfirmationModalViewModel>(afterCloseModalFunction);

                    var date = DateConverter.ShortDate(DateTime.Now);
                    _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{_configService.Orders}/{date}" });
                    order.Number = await GetOrderNumber(date);

                    await _ordersDataService.AddOrReplace(order);

                }
            }
            catch (Exception ex)
            {
                error = true;
            }
            finally
            {
                if (_uploadingOrder)
                {
                    if (!error)
                    {
                        viewModel.CloseModalCommand.Execute(order);
                    }
                    else
                    {
                        viewModel.CloseModalCommand.Execute(null);
                    }
                }
            }
        }

        private async Task OrderCompleted(bool error)
        {
            if (!error)
            {
                DependencyContainer.Refresh();
                await NavigationService.NavigateToAsync<HomeTabbedViewModel>();
            }
            else
            {
                _uploadingOrder = false;
                IsLoading = false;
            }

        }

        private async Task<int> GetOrderNumber(string date)
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
            return localDataOrder.LastNumber;

        }

        //NO SE ESTA USANDO
        private async Task GetOrders()
        {
            var key = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{_configService.Orders}/{key}" });
            var orders = await _ordersDataService.GetAll();
        }



    }
}
