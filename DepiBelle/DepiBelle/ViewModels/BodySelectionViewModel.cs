using System;
using System.Threading.Tasks;
using DepiBelle.Configuration;
using DepiBelle.Models;
using DepiBelle.Services.Data;
using DepiBelle.ViewModels.Modals;

namespace DepiBelle.ViewModels
{
    public class BodySelectionViewModel:ViewModelBase
    {
        private IDataService<Offer> _offersDataService;

        public BodySelectionViewModel()
        {
            IsLoading = true;
            _offersDataService = _offersDataService ?? DependencyContainer.Resolve<IDataService<Offer>>();
            _offersDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = DataServiceConstants.OFFERS });
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
