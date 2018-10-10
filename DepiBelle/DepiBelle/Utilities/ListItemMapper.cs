using System;
using System.Windows.Input;
using DepiBelle.Models;

namespace DepiBelle.Utilities
{
    public static class ListItemMapper
    {
        public static OfferListItem GetOfferListItem(Offer offer, int discount, bool isSelected, ICommand onSelectedCommand)
        {

            return new OfferListItem()
            {
                Id = offer.Id,
                Name = offer.Name,
                Price = offer.Price,
                IsSelected = isSelected,
                OnSelectedCommand = onSelectedCommand,
                Discount = discount
            };
        }

        public static PromotionListItem GetPromotionListItem(Promotion promotion, bool isSelected, ICommand onSelectedCommand)
        {

            return new PromotionListItem()
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description,
                Price = promotion.Price,
                IsSelected = isSelected,
                OnSelectedCommand = onSelectedCommand
            };
        }
    }
}
