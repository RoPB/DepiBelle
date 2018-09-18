using System;
using System.Threading.Tasks;
using DepiBelle.Configuration;
using DepiBelle.Managers.Application;
using DepiBelle.ViewModels.Modals;

namespace DepiBelle.ViewModels
{
    public class HomeViewModel:ViewModelBase
    {
        private IApplicationManager _applicationMananger;

        public HomeViewModel()
        {
            _applicationMananger = _applicationMananger ?? DependencyContainer.Resolve<IApplicationManager>();
            IsLoading = true;

        }

        public override async Task InitializeAsync(object navigationData)
        {
            await _applicationMananger.Login(AuthenticationServiceConstants.USER, AuthenticationServiceConstants.PASSWORD);
            IsLoading = false;
        }
    }
}
