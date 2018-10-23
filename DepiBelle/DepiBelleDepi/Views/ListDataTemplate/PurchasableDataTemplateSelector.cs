using System;
using DepiBelleDepi.Models;
using Xamarin.Forms;

namespace DepiBelleDepi.Views.ListDataTemplate
{
    public class PurchasableDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OfferDataTemplate { get; set; }
        public DataTemplate PromotionDataTemplate { get; set; }
        public DataTemplate OrdersDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is OfferItem)
                return OfferDataTemplate;
            else if (item is PromotionItem)
                return PromotionDataTemplate;
            else
                return OrdersDataTemplate;
        }
    }
}
