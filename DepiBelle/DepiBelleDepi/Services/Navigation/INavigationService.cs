using System;
using System.Threading.Tasks;
using DepiBelleDepi.ViewModels;

namespace DepiBelleDepi.Services.Navigation
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task<ViewModelBase> NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase;
        Task NavigateToAsync(Type vm, object parameter = null);
        Task PopAsync();
    }
}
