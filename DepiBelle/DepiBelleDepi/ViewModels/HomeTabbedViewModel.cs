using System;
using System.Threading.Tasks;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data.DataQuery;
using System.Linq;

namespace DepiBelleDepi.ViewModels
{
    public class HomeTabbedViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private IDataQueryService<Config> _dataQueryConfigService;
        private IApplicationManager _applicationMananger;

        private ViewModelBase _promotionsViewModel;
        private ViewModelBase _bodySelectionViewModel;
        private ViewModelBase _purchaseViewModel;

        public HomeTabbedViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _dataQueryConfigService = _dataQueryConfigService ?? DependencyContainer.Resolve<IDataQueryService<Config>>();
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();

            _promotionsViewModel = _promotionsViewModel ?? DependencyContainer.Resolve<PromotionsViewModel>();
            _bodySelectionViewModel = _bodySelectionViewModel ?? DependencyContainer.Resolve<BodySelectionViewModel>();
            _purchaseViewModel = _purchaseViewModel ?? DependencyContainer.Resolve<PurchaseViewModel>();

        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(_configService.User, _configService.Password);

            _dataQueryConfigService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Config });

            var config = await _dataQueryConfigService.Get();
            var order = navigationData as Order;
            var bodySelectionNavigationParameter = new BodySelectionNavigationParam() { Config = config, OffersAdded = order.Offers };

            await _purchaseViewModel.InitializeAsync(order);
            await _promotionsViewModel.InitializeAsync(order.Promotions);
            await _bodySelectionViewModel.InitializeAsync(bodySelectionNavigationParameter);

            IsLoading = false;
        }

    }
}
