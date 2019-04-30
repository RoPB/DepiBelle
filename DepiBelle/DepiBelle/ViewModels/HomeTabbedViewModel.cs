using System;
using System.Threading.Tasks;
using DepiBelle.Managers.Application;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data.DataQuery;
using DepiBelle.Services.Dialog;

namespace DepiBelle.ViewModels
{
	public class HomeTabbedViewModel:ViewModelBase
    {
        private IConfigService _configService;
        private IDialogService _dialogService;
        private IDataQueryService<Config> _dataQueryConfigService;
        private IApplicationManager _applicationMananger;

        private ViewModelBase _promotionsViewModel;
        private ViewModelBase _bodySelectionViewModel;
        private ViewModelBase _purchaseViewModel;

        public HomeTabbedViewModel()
        {
             IsLoading = true;
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _dialogService = _dialogService ?? DependencyContainer.Resolve<IDialogService>();
            _dataQueryConfigService = _dataQueryConfigService ?? DependencyContainer.Resolve<IDataQueryService<Config>>();
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();

            _promotionsViewModel = _promotionsViewModel ?? DependencyContainer.Resolve<PromotionsViewModel>();
            _bodySelectionViewModel = _bodySelectionViewModel ?? DependencyContainer.Resolve<BodySelectionViewModel>();
            _purchaseViewModel = _purchaseViewModel ?? DependencyContainer.Resolve<PurchaseViewModel>();

        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {

                await _applicationMananger.Login(_configService.User, _configService.Password);

                _dataQueryConfigService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Config });
                var config = await _dataQueryConfigService.Get();

                await _purchaseViewModel.InitializeAsync(navigationData);
                await _promotionsViewModel.InitializeAsync();
                await _bodySelectionViewModel.InitializeAsync(config);

            }
            catch(Exception ex)
            {
                await _dialogService.ShowAlertAsync(ex.Message, "ERROR", "OK");
            }
            finally
            {
                IsLoading = false;
            }

        }

    }
}
