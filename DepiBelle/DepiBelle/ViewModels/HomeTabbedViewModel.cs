using System;
using System.Threading.Tasks;

namespace DepiBelle.ViewModels
{
	public class HomeTabbedViewModel:ViewModelBase
    {
        private HomeViewModel _homeViewModel;

        public HomeTabbedViewModel()
        {
            _homeViewModel = _homeViewModel ?? DependencyContainer.Resolve<HomeViewModel>();
        }

        public override async Task InitializeAsync(object navigationData)
        {

            await _homeViewModel.InitializeAsync(null);
        }
    }
}
