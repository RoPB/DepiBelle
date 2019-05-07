using System;
using Xamarin.Forms;

namespace DepiBelleDepi.Extensions
{
    public class ViewCellTemplateBase:ViewCell
    {
        public static BindableProperty ParentBindingContextProperty = BindableProperty.Create(nameof(ParentBindingContext), typeof(object), typeof(ViewCellTemplateBase), null);

        public object ParentBindingContext
        {
            get { return GetValue(ParentBindingContextProperty); }
            set { SetValue(ParentBindingContextProperty, value); }
        }
    }
}
