using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DepiBelle.Views.ListDataTemplates
{
    public partial class PromotionListDataTemplate : ContentView
    {
        public static BindableProperty ParentBindingContextProperty =
            BindableProperty.Create(nameof(ParentBindingContext), typeof(object), typeof(PromotionListDataTemplate), null, BindingMode.TwoWay);

        public object ParentBindingContext
        {
            get { return GetValue(ParentBindingContextProperty); }
            set { SetValue(ParentBindingContextProperty, value); }
        }

        public static BindableProperty JojoProperty =
            BindableProperty.Create(nameof(Jojo), typeof(object), typeof(PromotionListDataTemplate), null, BindingMode.TwoWay);

        public object Jojo
        {
            get { return GetValue(JojoProperty); }
            set { SetValue(JojoProperty, value); }
        }

        public PromotionListDataTemplate()
        {
            InitializeComponent();
        }
    }
}
