using System;
using DepiBelleDepi.Models;
using Xamarin.Forms;

namespace DepiBelleDepi.Views.ListDataTemplates
{
    public class PurchasableDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OfferDataTemplate { get; set; }
        public DataTemplate PromotionDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is OfferItem)
                return OfferDataTemplate;
            else
                return PromotionDataTemplate;
        }
    }
}
