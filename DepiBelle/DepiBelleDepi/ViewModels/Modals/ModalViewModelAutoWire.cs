using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace DepiBelleDepi.ViewModels.Modals
{
    public class ModalViewModelAutoWire
    {
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelAutoWire), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelAutoWire.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelAutoWire.AutoWireViewModelProperty, value);
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var modalView = bindable as Element;
            if (modalView == null)
            {
                return;
            }

            var modalViewType = modalView.GetType();
            var modalViewName = modalViewType.FullName.Replace(".Views.Modals", ".ViewModels.Modals");
            var modalViewAssemblyName = modalViewType.GetTypeInfo().Assembly.FullName;
            var modalViewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", modalViewName, modalViewAssemblyName);

            var modalViewModelType = Type.GetType(modalViewModelName);
            if (modalViewModelType == null)
            {
                return;
            }
            var viewModel = DependencyContainer.Resolve(modalViewModelType);
            modalView.BindingContext = viewModel;
        }

    }
}
