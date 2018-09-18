using System;
using System.Threading.Tasks;
using DepiBelle.ViewModels.Modals;

namespace DepiBelle.Services.Modal
{
    public interface IModalService
    {
        Task PushAsync<TModalViewModel>() where TModalViewModel : ModalViewModelBase;
        Task PushAsync<TModalViewModel>(object parameter) where TModalViewModel : ModalViewModelBase;
        Task PushAsync<TModalViewModel>(Func<object, Task> func) where TModalViewModel : ModalViewModelBase;
        Task PopAsync();
        Task PopAllAsync();
    }
}
