using System;
using System.Threading.Tasks;
using DepiBelle.Configuration;
using DepiBelle.Models;
using DepiBelle.Services.Data;

namespace DepiBelle.ViewModels
{
    public class PromotionsViewModel : ViewModelBase
    {
        private IDataService<Promotion> _promotionsDataService;

        public PromotionsViewModel()
        {
            IsLoading = true;
            _promotionsDataService = _promotionsDataService ?? DependencyContainer.Resolve<IDataService<Promotion>>();
            _promotionsDataService.Initialize(new Config() { Uri = DataServiceConstants.URI, Key = DataServiceConstants.PROMOTIONS });
        }

        public override async Task InitializeAsync(object navigationData=null)
        {
            //var promotions = await _promotionsDataService.GetAll();
            IsLoading = true;
        }

    }
}
