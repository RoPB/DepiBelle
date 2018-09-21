using System;
using System.Threading.Tasks;
using DepiBelle.Configuration;
using DepiBelle.Managers.Application;

namespace DepiBelle.ViewModels
{
	public class HomeTabbedViewModel:ViewModelBase
    {
        private IApplicationManager _applicationMananger;

        private PromotionsViewModel _promotionsViewModel;
        private BodySelectionViewModel _bodySelectionViewModel;
        private PurchaseViewModel _purchaseViewModel;

        public HomeTabbedViewModel()
        {
             IsLoading = true;
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();
            _promotionsViewModel = _promotionsViewModel ?? DependencyContainer.Resolve<PromotionsViewModel>();
            _bodySelectionViewModel = _bodySelectionViewModel ?? DependencyContainer.Resolve<BodySelectionViewModel>();
            _purchaseViewModel = _purchaseViewModel ?? DependencyContainer.Resolve<PurchaseViewModel>();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(AuthenticationServiceConstants.USER, AuthenticationServiceConstants.PASSWORD);
            await _promotionsViewModel.InitializeAsync();
            await _bodySelectionViewModel.InitializeAsync();
            await _purchaseViewModel.InitializeAsync();
            IsLoading = false;
        }
    }
}
