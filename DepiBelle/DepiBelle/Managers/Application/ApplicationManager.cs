using System;
using System.Threading.Tasks;
using DepiBelle.Configuration;
using DepiBelle.Models;
using DepiBelle.Services.Authentication;
using DepiBelle.Services.Data;
using DepiBelle.Utilities;
using Plugin.Settings;

namespace DepiBelle.Managers.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private IAuthenticationService _authenticationService;
        private IDataService<Offer> _offersDataService;
        private IDataService<Promotion> _promotionsDataService;
        private IDataService<Order> _ordersDataService;

        public ApplicationManager()
        {
            _authenticationService = _authenticationService ?? DependencyContainer.Resolve<IAuthenticationService>();
            _authenticationService.Initialize(DataServiceConstants.APP_TOKEN);

            _offersDataService = _offersDataService ?? DependencyContainer.Resolve<IDataService<Offer>>();
            _offersDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = DataServiceConstants.OFFERS });

            _promotionsDataService = _promotionsDataService?? DependencyContainer.Resolve<IDataService<Promotion>>();
            _promotionsDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = DataServiceConstants.PROMOTIONS });

            _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataService<Order>>();
        }

        public async Task Login(string user, string password)
        {
            try
            {
                await _authenticationService.Authenticate(user, password);

                //await _offersDataService.Remove("A");
                //await _offersDataService.AddOrReplace(new Offer(){id="saracatanga",name="papada",price=100},false);
                //var offer = await _offersDataService.Get("A");
                //var offers = await _offersDataService.GetAll();


                //await AddOrders();

                //await GetOrders();

                //await GetOffers();

                await GetPromotions();
            }
            catch (Exception ex)
            {

            }

        }

        private async Task GetOffers(){

            var offers = await _offersDataService.GetAll();
        }

        private async Task GetPromotions()
        {

            var promotions = await _promotionsDataService.GetAll();
        }

        private async Task AddOrders(){

            var key = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = $"{DataServiceConstants.ORDERS}/{key}" });
            await AddOrder(key);
            await AddOrder(key);
            await AddOrder(key);
        }

        private async Task AddOrder(string key)
        {
            var localFileKey = DataServiceConstants.ORDERS;
            var number = 0;
            if (!CrossSettings.Current.Contains(key, localFileKey))
                CrossSettings.Current.Clear(localFileKey);//all other days
            else
                number = CrossSettings.Current.GetValueOrDefault(key, 0, localFileKey);

            CrossSettings.Current.AddOrUpdateValue(key, ++number, localFileKey);

            await _ordersDataService.AddOrReplace(new Order() { number = number });
        }

        private async Task GetOrders(){
            var key = DateConverter.ShortDate(DateTime.Now);
            _ordersDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = $"{DataServiceConstants.ORDERS}/{key}" });
            var orders = await _ordersDataService.GetAll();
        }


    }
}
