using System;
using System.Threading.Tasks;
using DepiBelle.ViewModels;

namespace DepiBelle.Services.Navigation
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task PopAsync();
    }
}

