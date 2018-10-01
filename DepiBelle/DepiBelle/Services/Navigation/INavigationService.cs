using System;
using System.Threading.Tasks;
using DepiBelle.ViewModels;

namespace DepiBelle.Services.Navigation
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task<ViewModelBase> NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task<ViewModelBase> NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task PopAsync();
    }
}

