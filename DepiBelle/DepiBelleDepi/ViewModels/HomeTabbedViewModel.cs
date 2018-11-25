using System;
using System.Threading.Tasks;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data.DataQuery;
using System.Linq;
using System.Collections.Generic;
using DepiBelleDepi.Utilities;

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

            var order = navigationData as Order;
            List<PurchasableItem> promotionsItem = null;
            BodySelectionNavigationParam bodySelectionNavigationParameter = null;
            PurchaseNavigationParam purchaseNavigationParam = null;

            if (order == null)
            {
                var config = await _dataQueryConfigService.Get();
                bodySelectionNavigationParameter = new BodySelectionNavigationParam() { Config = config };
                purchaseNavigationParam = new PurchaseNavigationParam() { Name = "SIN HORA", Time = DateConverter.ShortTime(DateTime.Now.TimeOfDay) };
            }
            else
            {
                bodySelectionNavigationParameter = new BodySelectionNavigationParam() { OffersAdded = order.Offers };
                promotionsItem = order.Promotions;
                purchaseNavigationParam = new PurchaseNavigationParam() {Time = order.Time, Name = order.Name };
            }

            await _purchaseViewModel.InitializeAsync(purchaseNavigationParam);
            await _promotionsViewModel.InitializeAsync(promotionsItem);
            await _bodySelectionViewModel.InitializeAsync(bodySelectionNavigationParameter);

            IsLoading = false;
        }

    }
}
