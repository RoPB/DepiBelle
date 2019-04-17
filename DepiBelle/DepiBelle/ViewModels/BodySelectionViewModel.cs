using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Services.Notification;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Services.Dialog;
using DepiBelle.Services.Navigation;
using Xamarin.Forms;
using DepiBelle.Utilities;

namespace DepiBelle.ViewModels
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

                var config = navigationData as Config;

                if (config != null)
                {
                    Discount = config.Discount;
                }

                ShowDiscount = Discount > 0;


                var queryLike = new QueryLike();
                queryLike.LikeField = typeof(Offer)
                                .GetProperty(nameof(Offer.Name))
                                .GetPropertyAttribute((Plugin.CloudFirestore.Attributes.MapToAttribute dna) => dna.Mapping);

                queryLike.LikeValue = "B";
                queryLike = null;

                var queryOrderBy = new QueryOrderBy();
                queryOrderBy.OrderByField = typeof(Offer)
                                        .GetProperty(nameof(Offer.Name))
                                        .GetPropertyAttribute((Plugin.CloudFirestore.Attributes.MapToAttribute dna) => dna.Mapping);

                var queryWhere = new QueryWhere();
                queryWhere.WhereField = typeof(Offer)
                                        .GetProperty(nameof(Offer.Price))
                                        .GetPropertyAttribute((Plugin.CloudFirestore.Attributes.MapToAttribute dna) => dna.Mapping);
                queryWhere.Type = QueryWhereEnum.GreaterThan;
                queryWhere.ValueField = 60;

                // var offers = await _offersDataService.GetAll(querysOrderBy: new List<QueryOrderBy>() { queryOrderBy }, queryLike: queryLike, querysWhere:new List<QueryWhere>() { queryWhere });
                var offers = await _offersDataService.GetAll();

                await ClasificateOrders(offers);
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

            var navigationParam = new PartSelectionNavigationParam() { 
                SelectedOffers = _selectedOffers,
                Offers = offersList, 
                Discount = _discount,
                Title=title};

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

    }
}
