using System;
using System.Windows.Input;
using DepiBelle.Models;

namespace DepiBelle.Utilities
{
    public static class ListItemMapper
    {
        public static OfferListItem GetOfferListItem(Offer offer, bool IsSelected, ICommand OnSelectedCommand)
        {

            return new OfferListItem()
            {
                Id = offer.Id,
                Name = offer.Name,
                Price = offer.Price,
                IsSelected = IsSelected,
                OnSelectedCommand = OnSelectedCommand
            };
        }

        public static PromotionListItem GetPromotionListItem(Promotion promotion, bool IsSelected, ICommand OnSelectedCommand)
        {

            return new PromotionListItem()
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description,
                Price = promotion.Price,
                IsSelected = IsSelected,
                OnSelectedCommand = OnSelectedCommand
            };
        }
    }
}
