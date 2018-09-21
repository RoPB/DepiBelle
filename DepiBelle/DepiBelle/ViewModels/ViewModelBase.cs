using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DepiBelle.Services.Dialog;
using DepiBelle.Services.Modal;
using DepiBelle.Services.Navigation;

namespace DepiBelle.ViewModels
{
    public class ViewModelBase:INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected bool SetPropertyValue<T>(ref T storageField, T newValue, Expression<Func<T>> propExpr)
        {
            if (Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            var prop = (System.Reflection.PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);

            return true;
        }

        protected bool SetPropertyValue<T>(ref T storageField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        protected void RaiseAllPropertiesChanged()
        {
            // By convention, an empty string indicates all properties are invalid.
            this.PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propExpr)
        {
            var prop = (System.Reflection.PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private bool _isInitialized;
        private bool _isLoading;
        private bool _isEnabled;

        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly IModalService ModalService;

        public bool IsInitialized { get { return _isInitialized; } set { SetPropertyValue(ref _isInitialized, value); } }
        public bool IsLoading { get { return _isLoading; } set { SetPropertyValue(ref _isLoading, value); } }
        public bool IsEnabled { get { return _isEnabled; } set { SetPropertyValue(ref _isEnabled, value); } }


        public ViewModelBase(){

            DialogService = DialogService ?? DependencyContainer.Resolve<IDialogService>();
            NavigationService = NavigationService ?? DependencyContainer.Resolve<INavigationService>();
            ModalService = ModalService ?? DependencyContainer.Resolve<IModalService>();
        }

        public virtual Task InitializeAsync(object navigationData=null)
        {
            return Task.FromResult(false);
        }

        public virtual Task Refresh(object navigationData=null)
        {
            return Task.FromResult(false);
        }
    }
}
