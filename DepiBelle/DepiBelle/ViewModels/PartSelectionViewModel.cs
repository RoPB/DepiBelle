using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Services.Notification;
using DepiBelle.Utilities;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PartSelectionViewModel : ViewModelBase
    {
        private ICartNotificationService<Offer> _cartOfferManager;
        private ObservableCollection<OfferItem> _offers;
        private string _title;

        public ObservableCollection<OfferItem> Offers
        {
            get { return _offers; }
            set { SetPropertyValue(ref _offers, value); }
        }

        public ICommand OfferSelectedCommand { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetPropertyValue(ref _title, value); }
        }

        public PartSelectionViewModel()
        {
            OfferSelectedCommand = new Command<OfferItem>(async (offer) => await OfferSelected(offer));
            IsLoading = true;
            _cartOfferManager = _cartOfferManager ?? DependencyContainer.Resolve<ICartNotificationService<Offer>>();
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            await Task.Run(() =>
            {
                IsLoading = true;

                var param = navigationData as PartSelectionNavigationParam;
                var _selectedOffers = param.SelectedOffers;
                var offers = param.Offers;
                var discount = param.Discount;
                var title = $"{param.Title.First().ToString().ToUpper()}{param.Title.Substring(1)}";

                offers = offers.OrderBy(o => o.Name).ToList();

                Title = title;

                Offers = new ObservableCollection<OfferItem>();

                offers.ForEach(o => Offers.Add(ListItemMapper.GetOfferListItem(o, discount, _selectedOffers.Contains(o.Id), OfferSelectedCommand)));

                IsLoading = false;
            });

        }

        private async Task OfferSelected(OfferItem offer)
        {
            offer.IsSelected = !offer.IsSelected;

            if (offer.IsSelected)
            {
                var cartItem = new CartItem<Offer>()
                {
                    Discount = offer.Discount,
                    Item = new Offer(offer.Id,
                                 offer.Price,
                                 offer.Name)
                };

                _cartOfferManager.ItemAdded.Invoke(this, cartItem);
            }
            else
                _cartOfferManager.ItemRemoved(this,offer.Id);
        }

    }
}
