using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using DepiBelle.ViewModels;
using DepiBelle.Views;
using Xamarin.Forms;

namespace DepiBelle.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task InitializeAsync()
        {
            await NavigateToAsync<HomeViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public async Task PopAsync()
        {
            var navigationPage = Application.Current.MainPage as CustomNavigationView;
            await navigationPage.PopAsync();
            await (navigationPage.CurrentPage.BindingContext as ViewModelBase).Refresh();
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);
            object viewModel = DependencyContainer.Resolve(viewModelType);

            if (viewModelType.Equals(typeof(HomeViewModel)))
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }
            else
            {
                var navigationPage = Application.Current.MainPage as CustomNavigationView;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new CustomNavigationView(page);
                }
            }

            page.BindingContext = viewModel;
            ViewModelBase baseViewModel = (page.BindingContext as ViewModelBase);
            if (!baseViewModel.IsInitialized)
                await baseViewModel.InitializeAsync(parameter);
            else
                await baseViewModel.Refresh();
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }


    }
}
