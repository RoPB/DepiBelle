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

        private ViewModelBase _promotionsViewModel;
        private ViewModelBase _bodySelectionViewModel;
        private ViewModelBase _purchaseViewModel;

        public HomeTabbedViewModel()
        {
            IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _dataQueryConfigService = _dataQueryConfigService ?? DependencyContainer.Resolve<IDataQueryService<Config>>();

            _promotionsViewModel = _promotionsViewModel ?? DependencyContainer.Resolve<PromotionsViewModel>();
            _bodySelectionViewModel = _bodySelectionViewModel ?? DependencyContainer.Resolve<BodySelectionViewModel>();
            _purchaseViewModel = _purchaseViewModel ?? DependencyContainer.Resolve<PurchaseViewModel>();

        }

        public override async Task InitializeAsync(object navigationData)
        {
            _dataQueryConfigService.Initialize(new DataServiceConfig() { Uri = _configService.ConfigUri, Key = _configService.Config });

            var navParam = navigationData as HomeNavigationParam;
            BodySelectionNavigationParam bodySelectionNavigationParameter = null;
            PurchaseNavigationParam purchaseNavigationParam = null;
            PromotionsNavigationParam promotionNavigationParam = null;
            var config = await _dataQueryConfigService.Get();



            if (navParam == null)
            {
                bodySelectionNavigationParameter = new BodySelectionNavigationParam() { Config = config, ToAttend=true };
                promotionNavigationParam = new PromotionsNavigationParam() { ShowAddRemoveButtons = true };
                purchaseNavigationParam = new PurchaseNavigationParam() { Name = "SIN HORA", Time = DateConverter.ShortTime(DateTime.Now.TimeOfDay), ShowButtonsCancelConfirm=true };
            }
            else
            {
                bodySelectionNavigationParameter = new BodySelectionNavigationParam() { Config = config, OffersAdded = navParam.Order.Offers, ToAttend= navParam.ToAttend };
                promotionNavigationParam = new PromotionsNavigationParam() { PromotionsAdded = navParam.Order.Promotions, ShowAddRemoveButtons = navParam.ToAttend };
                purchaseNavigationParam = new PurchaseNavigationParam() {Id=navParam.Order.Id, Date=navParam.Order.Date, Time = navParam.Order.Time, Name = navParam.Order.Name, ShowButtonsCancelConfirm = navParam.ToAttend};
            }

            await _purchaseViewModel.InitializeAsync(purchaseNavigationParam);
            await _promotionsViewModel.InitializeAsync(promotionNavigationParam);
            await _bodySelectionViewModel.InitializeAsync(bodySelectionNavigationParameter);

            IsLoading = false;
        }

    }
}
