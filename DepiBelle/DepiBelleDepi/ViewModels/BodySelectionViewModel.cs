using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data;
using DepiBelleDepi.Services.Dialog;
using DepiBelleDepi.Services.Navigation;
using DepiBelleDepi.Services.Notification.Cart;
using Xamarin.Forms;

namespace DepiBelleDepi.ViewModels
{
    public class BodySelectionViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private IDataCollectionService<Offer> _offersDataService;

        private ICartNotificationService<Offer> _cartOfferManager;

        private List<string> _selectedOffers = new List<string>();
        private List<Offer> _headOffers = new List<Offer>();
        private List<Offer> _bodyOffers = new List<Offer>();
        private List<Offer> _pelvisOffers = new List<Offer>();
        private List<Offer> _armOffers = new List<Offer>();
        private List<Offer> _legOffers = new List<Offer>();

        private bool _showDiscount;
        private int _discount;

        public bool ShowDiscount
        {
            get { return _showDiscount; }
            set { SetPropertyValue(ref _showDiscount, value); }
        }
        public int Discount
        {
            get { return _discount; }
            set { SetPropertyValue(ref _discount, value); }
        }

        public ICommand BodyPartSelectionCommand { get; set; }

        public BodySelectionViewModel()
        {
            IsLoading = true;

            BodyPartSelectionCommand = new Command<string>(async (string bodyPart) => await BodyPartSelection(bodyPart));

            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _navigationService = _navigationService ?? DependencyContainer.Resolve<INavigationService>();
            _dialogService = _dialogService ?? DependencyContainer.Resolve<IDialogService>();
            _offersDataService = _offersDataService ?? DependencyContainer.Resolve<IDataCollectionService<Offer>>();
            _offersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Offers });

            _cartOfferManager = _cartOfferManager ?? DependencyContainer.Resolve<ICartNotificationService<Offer>>();
            _cartOfferManager.ItemRemoved += OfferRemovedHandler;
            _cartOfferManager.ItemAdded += OfferAddedHandler;
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            //await _offersDataService.Remove("A");
            //await _offersDataService.AddOrReplace(new Offer(){id="saracatanga",name="papada",price=100},false);
            //var offer = await _offersDataService.Get("A");
            //await _offersDataService.Subscribe((offer) => Console.Write(offer.Type.ToString()));
            try
            {
                var param = navigationData as BodySelectionNavigationParam;

                HandleDiscount(param);

                var offersAdded = param.OffersAdded;
                var offers = await _offersDataService.GetAll();
                await ClasificateOrders(offers);

                HandleOffersAdded(offers, offersAdded);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Se produjo un problema en la descarga de datos (OFERTAS)", "ERROR", "OK");
            }
            finally
            {
                IsLoading = false;
            }


        }

        private void HandleDiscount(BodySelectionNavigationParam param)
        {

            if (param.OffersAdded != null && param.OffersAdded.Count > 0)
            {
                Discount = param.OffersAdded.First().Discount;
            }
            else if (param.Config != null)
            {
                Discount = param.Config.Discount;
            }

            ShowDiscount = Discount > 0;
        }

        private async Task ClasificateOrders(List<Offer> offers)
        {
            await Task.Run(() =>
            {

                _headOffers.AddRange(offers.Where(o => o.Category.Contains(Constants.Constants.CATEGORY_HEAD)).ToList());
                _bodyOffers.AddRange(offers.Where(o => o.Category.Contains(Constants.Constants.CATEGORY_BODY)).ToList());
                _pelvisOffers.AddRange(offers.Where(o => o.Category.Contains(Constants.Constants.CATEGORY_PELVIS)).ToList());
                _armOffers.AddRange(offers.Where(o => o.Category.Contains(Constants.Constants.CATEGORY_ARM)).ToList());
                _legOffers.AddRange(offers.Where(o => o.Category.Contains(Constants.Constants.CATEGORY_LEG)).ToList());

            });
        }

        private async Task BodyPartSelection(string bodyPart)
        {
            List<Offer> offersList = null;
            var title = string.Empty;

            if (bodyPart.Equals(Constants.Constants.CATEGORY_HEAD))
            {
                offersList = _headOffers;
                title = Constants.Constants.CATEGORY_HEAD;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_BODY))
            {
                offersList = _bodyOffers;
                title = Constants.Constants.CATEGORY_BODY;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_PELVIS))
            {
                offersList = _pelvisOffers;
                title = Constants.Constants.CATEGORY_PELVIS;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_ARM))
            {
                offersList = _armOffers;
                title = Constants.Constants.CATEGORY_ARM;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_LEG))
            {
                offersList = _legOffers;
                title = Constants.Constants.CATEGORY_LEG;
            }

            var navigationParam = new PartSelectionNavigationParam()
            {
                SelectedOffers = _selectedOffers,
                Offers = offersList,
                Discount = _discount,
                Title = title
            };

            await _navigationService.NavigateToAsync<PartSelectionViewModel>(navigationParam);

        }


        private void OfferAddedHandler(object sender, CartItem<Offer> e)
        {
            _selectedOffers.Add(e.Item.Id);
        }

        public void OfferRemovedHandler(object sender, string offerId)
        {
            _selectedOffers.Remove(offerId);
        }

        public void HandleOffersAdded(List<Offer> offers, List<PurchasableItem> offersAdded)
        {
            if (offersAdded != null)
            {
                foreach (var offerAdded in offersAdded)
                {
                    var offerToAdd = offers.Where(o => o.Id == offerAdded.Id).FirstOrDefault();
                    if (offerToAdd != null)
                        _cartOfferManager.ItemAdded(this, new CartItem<Offer>() { Item = offerToAdd, Discount = offerAdded.Discount });
                }

            }

        }

    }
}
