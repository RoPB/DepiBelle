using System;
using DepiBelleDepi.Models;
using Xamarin.Forms;

namespace DepiBelleDepi.Views.ListDataTemplate
{
    public class PurchasableDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OfferDataTemplate { get; set; }
        public DataTemplate PromotionDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            DataTemplate selectedDataTemplate ;

            if (item is OfferItem)
                selectedDataTemplate =  OfferDataTemplate;
            else
                selectedDataTemplate =  PromotionDataTemplate;

            selectedDataTemplate.SetValue(ViewCellTemplateBase.ParentBindingContextProperty, container.BindingContext);

            return selectedDataTemplate;
        }
    }
}
