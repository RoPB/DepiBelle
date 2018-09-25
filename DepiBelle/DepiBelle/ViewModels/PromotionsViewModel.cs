using System;
using System.Threading.Tasks;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;

namespace DepiBelle.ViewModels
{
    public class PromotionsViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private IDataService<Promotion> _promotionsDataService;

        public EventHandler<int> ItemsAddedEventHandler { get; set; }

        public PromotionsViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _promotionsDataService = _promotionsDataService ?? DependencyContainer.Resolve<IDataService<Promotion>>();
            _promotionsDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Promotions });
        }

        public override async Task InitializeAsync(object navigationData=null)
        {
            //var promotions = await _promotionsDataService.GetAll();
            IsLoading = true;
        }

    }
}
