using System;
using System.Threading.Tasks;
using DepiBelle.ViewModels.Modals;

namespace DepiBelle.Services.Modal
{
    public interface IModalService
    {
        Task<ModalViewModelBase> PushAsync<TModalViewModel>() where TModalViewModel : ModalViewModelBase;
        Task<ModalViewModelBase> PushAsync<TModalViewModel>(object parameter) where TModalViewModel : ModalViewModelBase;
        Task<ModalViewModelBase> PushAsync<TModalViewModel>(Func<object, Task> func) where TModalViewModel : ModalViewModelBase;
        Task PopAsync();
        Task PopAllAsync();
    }
}
