using System;
using System.Threading.Tasks;
using DepiBelleDepi.ViewModels.Modals;

namespace DepiBelleDepi.Services.Modal
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
