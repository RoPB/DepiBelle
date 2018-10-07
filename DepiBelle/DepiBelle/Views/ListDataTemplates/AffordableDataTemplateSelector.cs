using System;
using DepiBelle.Models;
using Xamarin.Forms;

namespace DepiBelle.Views.ListDataTemplates
{
    public class AffordableDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OfferDataTemplate { get; set; }
        public DataTemplate PromotionDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is OfferListItem)
                return OfferDataTemplate;
            else
                return PromotionDataTemplate;
        }
    }
}
