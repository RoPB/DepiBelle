using System;
using System.Threading.Tasks;
using DepiBelle.Managers.Application;
using DepiBelle.Services.Config;

namespace DepiBelle.ViewModels
{
	public class HomeTabbedViewModel:ViewModelBase
    {
        private IConfigService _configService;
        private IApplicationManager _applicationMananger;

        private PromotionsViewModel _promotionsViewModel;
        private BodySelectionViewModel _bodySelectionViewModel;
        private PurchaseViewModel _purchaseViewModel;

        public HomeTabbedViewModel()
        {
             IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();
            _promotionsViewModel = _promotionsViewModel ?? DependencyContainer.Resolve<PromotionsViewModel>();
            _bodySelectionViewModel = _bodySelectionViewModel ?? DependencyContainer.Resolve<BodySelectionViewModel>();
            _purchaseViewModel = _purchaseViewModel ?? DependencyContainer.Resolve<PurchaseViewModel>();

            _promotionsViewModel.ItemsAddedEventHandler +=_purchaseViewModel.ItemsAddedHandler;
            _bodySelectionViewModel.ItemsAddedEventHandler += _purchaseViewModel.ItemsAddedHandler;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(_configService.User, _configService.Password);
            await _promotionsViewModel.InitializeAsync();
            await _bodySelectionViewModel.InitializeAsync();
            await _purchaseViewModel.InitializeAsync();
            IsLoading = false;
        }

    }
}
