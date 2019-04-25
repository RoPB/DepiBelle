using System;
using System.Threading.Tasks;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data.DataQuery;
using System.Linq;
using System.Collections.Generic;
using DepiBelleDepi.Utilities;
using DepiBelleDepi.Services;
using DepiBelleDepi.Services.Data;

namespace DepiBelleDepi.ViewModels
{
    public class HomeTabbedViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private IDataQueryService<Config> _dataQueryConfigService;
        private IDeviceService _deviceService;
        private IDataCollectionService<Order> _ordersDataService;
        private IApplicationManager _applicationMananger;

        private ViewModelBase _promotionsViewModel;
        private ViewModelBase _bodySelectionViewModel;
        private ViewModelBase _purchaseViewModel;

        public HomeTabbedViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _dataQueryConfigService = _dataQueryConfigService ?? DependencyContainer.Resolve<IDataQueryService<Config>>();
            _deviceService = _deviceService ?? DependencyContainer.Resolve<IDeviceService>();
            _ordersDataService = _ordersDataService = _ordersDataService ?? DependencyContainer.Resolve<IDataCollectionService<Order>>();
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();

            _promotionsViewModel = _promotionsViewModel ?? DependencyContainer.Resolve<PromotionsViewModel>();
            _bodySelectionViewModel = _bodySelectionViewModel ?? DependencyContainer.Resolve<BodySelectionViewModel>();
            _purchaseViewModel = _purchaseViewModel ?? DependencyContainer.Resolve<PurchaseViewModel>();

        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(_configService.User, _configService.Password);

            _dataQueryConfigService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Config });

            var navParam = navigationData as HomeNavigationParam;
            List<PurchasableItem> promotionsItem = null;
            BodySelectionNavigationParam bodySelectionNavigationParameter = null;
            PurchaseNavigationParam purchaseNavigationParam = null;
            var config = await _dataQueryConfigService.Get();

            if (navParam == null)
            {

                bodySelectionNavigationParameter = new BodySelectionNavigationParam() { Config = config };
                purchaseNavigationParam = new PurchaseNavigationParam() { Name = "SIN HORA", Time = DateConverter.ShortTime(DateTime.Now.TimeOfDay), CanConfirm=true };
            }
            else
            {
                bodySelectionNavigationParameter = new BodySelectionNavigationParam() { Config = config, OffersAdded = navParam.Order.Offers };
                promotionsItem = navParam.Order.Promotions;
                purchaseNavigationParam = new PurchaseNavigationParam() {Time = navParam.Order.Time, Name = navParam.Order.Name, CanConfirm = navParam.ToAttend};

                if (navParam.ToAttend)
                {
                    navParam.Order.AttendedBy = _deviceService.DeviceId;
                    var key = _configService.OrdersInProcess;
                    _ordersDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = $"{key}<{navParam.Order.Date}>" });
                    await _ordersDataService.AddOrReplace(navParam.Order);
                }
            
            }

            await _purchaseViewModel.InitializeAsync(purchaseNavigationParam);
            await _promotionsViewModel.InitializeAsync(promotionsItem);
            await _bodySelectionViewModel.InitializeAsync(bodySelectionNavigationParameter);

            IsLoading = false;
        }

    }
}
