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
using DepiBelle.Utilities;
using DepiBelle.ViewModels.Modals;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PurchaseViewModel : ViewModelBase
    {
        private string _userName;
        private string _time;
        private IConfigService _configService;
        private IDataCollectionService<Order> _ordersDataService;

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

            _cartPromotionManager = _cartPromotionManager ?? DependencyContainer.Resolve<ICartNotificationService<Promotion>>();
            _cartOfferManager = _cartOfferManager ?? DependencyContainer.Resolve<ICartNotificationService<Offer>>();

            _cartPromotionManager.ItemAdded += PromotionAddedHandler;
            _cartPromotionManager.ItemRemoved += PromotionRemovedHandler;
            _cartOfferManager.ItemAdded += OfferAddedHandler;
            _cartOfferManager.ItemRemoved += OfferRemovedHandler;

        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            var orderData = navigationData as HomeTabbedNavigationParam;
            _userName = orderData.Name;
            _time = orderData.Time;
            IsLoading = false;
        }

        public void PromotionAddedHandler(object sender, CartItem<Promotion> itemAdded)
        {
            HandleItemsAddedBadge(true);

            var promotion = ListItemMapper.GetPromotionListItem(itemAdded.Item, true, PromotionSelectedCommand);
            _promotions.Add(promotion);

            LoadPurchasableItems();
        }

        public void PromotionRemovedHandler(object sender, string promotionId)
        {
            if (sender != this)
            {
                HandleItemsAddedBadge(false);

                _promotions.RemoveAll(p => p.Id == promotionId);

                LoadPurchasableItems();
            }

        }


        public void OfferAddedHandler(object sender, CartItem<Offer> itemAdded)
        {
            HandleItemsAddedBadge(true);

            var offer = ListItemMapper.GetOfferListItem(itemAdded.Item, itemAdded.Discount, true, OfferSelectedCommand);
            _offers.Add(offer);

            LoadPurchasableItems();
        }

        public void OfferRemovedHandler(object sender, string offerId)
        {
            if (sender != this)
            {
                HandleItemsAddedBadge(false);

                _offers.RemoveAll(o => o.Id == offerId);

                LoadPurchasableItems();
            }

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
            order.Name = _userName;
            order.Date = DateConverter.ShortDate(DateTime.Now);
            order.Time = _time;

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

                    //TODO: REALDATABASE
                    //_ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{_configService.OrdersInProcess}/{order.Date}" });

                    _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.ServiceUri, Key = $"{_configService.Orders}/{_configService.OrdersInProcess}/{order.Date}" });

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
                await NavigationService.NavigateToAsync<WelcomeViewModel>();
            }
            else
            {
                _uploadingOrder = false;
                IsLoading = false;
            }

        }

        //NO SE ESTA USANDO
        private async Task GetOrders()
        {
            var key = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.ServiceUri, Key = $"{_configService.OrdersInProcess}/{key}" });
            var orders = await _ordersDataService.GetAll();
        }



    }
}
