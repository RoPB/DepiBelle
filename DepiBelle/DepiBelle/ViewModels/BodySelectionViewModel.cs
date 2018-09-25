using System;
using System.Threading.Tasks;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.ViewModels.Modals;

namespace DepiBelle.ViewModels
{
    public class BodySelectionViewModel:ViewModelBase
    {
        private IConfigService _configService;
        private IDataService<Offer> _offersDataService;

        public BodySelectionViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _offersDataService = _offersDataService ?? DependencyContainer.Resolve<IDataService<Offer>>();
            _offersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Offers });
        }

        public override async Task InitializeAsync(object navigationData=null)
        {
            //await _offersDataService.Remove("A");
            //await _offersDataService.AddOrReplace(new Offer(){id="saracatanga",name="papada",price=100},false);
            //var offer = await _offersDataService.Get("A");
            //var offers = await _offersDataService.GetAll();
            //var offers = await _offersDataService.GetAll();

            IsLoading = false;
        }
    }
}
