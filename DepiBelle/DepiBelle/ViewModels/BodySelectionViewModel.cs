using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Services.Navigation;
using DepiBelle.ViewModels.Modals;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class BodySelectionViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private INavigationService _navigationService;
        private IDataService<Offer> _offersDataService;

        private List<Offer> _headOffers = new List<Offer>();
        private List<Offer> _bodyOffers = new List<Offer>();
        private List<Offer> _pelvisOffers = new List<Offer>();
        private List<Offer> _armOffers = new List<Offer>();
        private List<Offer> _legOffers = new List<Offer>();

        public ICommand BodyPartSelectionCommand { get; set; }
        public EventHandler<int> ItemsAddedEventHandler { get; set; } 

        public BodySelectionViewModel()
        {
            IsLoading = true;

            BodyPartSelectionCommand = new Command<string>(async (string bodyPart) => await BodyPartSelection(bodyPart));

            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _navigationService = _navigationService ?? DependencyContainer.Resolve<INavigationService>();
            _offersDataService = _offersDataService ?? DependencyContainer.Resolve<IDataService<Offer>>();
            _offersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Offers });
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            //await _offersDataService.Remove("A");
            //await _offersDataService.AddOrReplace(new Offer(){id="saracatanga",name="papada",price=100},false);
            //var offer = await _offersDataService.Get("A");
            //await _offersDataService.Subscribe((offer) => Console.Write(offer.Type.ToString()));

            var offers = await _offersDataService.GetAll();
            await ClasificateOrders(offers);

            IsLoading = false;
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

            if (bodyPart.Equals(Constants.Constants.CATEGORY_HEAD))
            {
                offersList=_headOffers;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_BODY))
            {
                offersList = _bodyOffers;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_PELVIS))
            {
                offersList = _pelvisOffers;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_ARM))
            {
                offersList = _armOffers;
            }
            else if (bodyPart.Equals(Constants.Constants.CATEGORY_LEG))
            {
                offersList = _legOffers;
            }

            await _navigationService.NavigateToAsync<PartSelectionViewModel>(offersList);
        }
    }
}
