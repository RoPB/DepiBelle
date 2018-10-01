using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PartSelectionViewModel : ViewModelBase
    {
        private static List<string> _selectedOffers = new List<string>();
        private ObservableCollection<OfferListItem> _offers;

        public ICommand OfferSelectedCommand { get; set; }
        public EventHandler<bool> ItemsAddedEventHandler { get; set; }

        public ObservableCollection<OfferListItem> Offers
        {
            get { return _offers; }
            set { SetPropertyValue(ref _offers, value); }
        }


        public PartSelectionViewModel()
        {
            OfferSelectedCommand = new Command<OfferListItem>(async (offer) => await OfferSelected(offer));
            IsLoading = true;
        }


        public override async Task InitializeAsync(object navigationData = null)
        {
            await Task.Run(() =>
            {
                IsLoading = true;

                var param = navigationData as List<Offer>;

                Offers = new ObservableCollection<OfferListItem>();

                param.ForEach(o => Offers.Add(new OfferListItem()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Price = o.Price,
                    IsSelected = _selectedOffers.Contains(o.Id)
                }));

                IsLoading = false;
            });

        }

        private async Task OfferSelected(OfferListItem offer)
        {
            offer.IsSelected = !offer.IsSelected;
            if (offer.IsSelected)
            {
                _selectedOffers.Add(offer.Id);
                ItemsAddedEventHandler.Invoke(this, true);
            }
            else
            {
                _selectedOffers.Remove(offer.Id);
                ItemsAddedEventHandler.Invoke(this, false);
            }

        }

    }
}
