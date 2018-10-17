using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using DepiBelle.ViewModels.Modals;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace DepiBelle.Services.Modal
{
    public class ModalService : IModalService
    {
        public Task<ModalViewModelBase> PushAsync<TModalViewModel>() where TModalViewModel : ModalViewModelBase
        {
            return InternalPushAsync(typeof(TModalViewModel));
        }

        public Task<ModalViewModelBase> PushAsync<TModalViewModel>(object parameter) where TModalViewModel : ModalViewModelBase
        {
            return InternalPushAsync(typeof(TModalViewModel), parameter);
        }

        public Task<ModalViewModelBase> PushAsync<TModalViewModel>(Func<object, Task> func) where TModalViewModel : ModalViewModelBase
        {
            return InternalPushAsync(typeof(TModalViewModel), func);
        }

        public async Task PopAsync()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception)
            {

            }

        }

        public async Task PopAllAsync()
        {
            try
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
            catch (Exception)
            {

            }
        }

        private async Task<ModalViewModelBase> InternalPushAsync(Type modalViewModelType, object parameter = null)
        {
            PopupPage popupPage = CreatePopupPage(modalViewModelType);
            await PopupNavigation.Instance.PushAsync(popupPage);
            var modalViewModel = (popupPage.BindingContext as ModalViewModelBase);
            await modalViewModel.InitializeAsync(parameter);

            return modalViewModel;
        }

        private Type GetPageTypeForModalViewModel(Type modalViewModelType)
        {
            var modalViewName = modalViewModelType.FullName.Replace("Model", string.Empty);
            var modalViewModelAssemblyName = modalViewModelType.GetTypeInfo().Assembly.FullName;
            var modalViewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", modalViewName, modalViewModelAssemblyName);
            var modalViewType = Type.GetType(modalViewAssemblyName);
            return modalViewType;
        }

        private PopupPage CreatePopupPage(Type modalViewModelType)
        {
            Type popupPageType = GetPageTypeForModalViewModel(modalViewModelType);
            if (popupPageType == null)
            {
                throw new Exception($"Cannot locate page type for {modalViewModelType}");
            }

            PopupPage popupPage = Activator.CreateInstance(popupPageType) as PopupPage;
            return popupPage;
        }
    }
}
