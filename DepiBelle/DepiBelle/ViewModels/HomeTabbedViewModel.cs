using System;
using System.Threading.Tasks;
using DepiBelle.Managers.Application;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data.DataQuery;

namespace DepiBelle.ViewModels
{
	public class HomeTabbedViewModel:ViewModelBase
    {
        private IConfigService _configService;
        private IDataQueryService<Config> _dataQueryConfigService;
        private IApplicationManager _applicationMananger;

        private PromotionsViewModel _promotionsViewModel;
        private BodySelectionViewModel _bodySelectionViewModel;
        private PurchaseViewModel _purchaseViewModel;

        public HomeTabbedViewModel()
        {
             IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _dataQueryConfigService = _dataQueryConfigService ?? DependencyContainer.Resolve<IDataQueryService<Config>>();
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();
            _promotionsViewModel = _promotionsViewModel ?? DependencyContainer.Resolve<PromotionsViewModel>();
            _bodySelectionViewModel = _bodySelectionViewModel ?? DependencyContainer.Resolve<BodySelectionViewModel>();
            _purchaseViewModel = _purchaseViewModel ?? DependencyContainer.Resolve<PurchaseViewModel>();

            _promotionsViewModel.ItemsAddedEventHandler +=_purchaseViewModel.ItemsAddedHandler;
            _bodySelectionViewModel.ItemsAddedEventHandler += _purchaseViewModel.ItemsAddedHandler;

            _purchaseViewModel.PromotionRemoved += _promotionsViewModel.PromotionRemovedHandler;
            _purchaseViewModel.OfferRemoved += _bodySelectionViewModel.OfferRemovedHandler;

        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(_configService.User, _configService.Password);

            _dataQueryConfigService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Config });
            var config = await _dataQueryConfigService.Get();
            await _purchaseViewModel.InitializeAsync();
            await _promotionsViewModel.InitializeAsync();
            await _bodySelectionViewModel.InitializeAsync(config);
            IsLoading = false;
        }

    }
}
