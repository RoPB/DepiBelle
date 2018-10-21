using System;
using System.Windows.Input;
using DepiBelleDepi.Models;

namespace DepiBelleDepi.Utilities
{
    public static class ListItemMapper
    {
        public static OfferItem GetOfferListItem(Offer offer, int discount, bool isSelected, ICommand onSelectedCommand)
        {

            return new OfferItem()
            {
                Id = offer.Id,
                Name = offer.Name,
                Price = offer.Price,
                IsSelected = isSelected,
                OnSelectedCommand = onSelectedCommand,
                Discount = discount
            };
        }

        public static PromotionItem GetPromotionListItem(Promotion promotion, bool isSelected, ICommand onSelectedCommand)
        {

            return new PromotionItem()
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
