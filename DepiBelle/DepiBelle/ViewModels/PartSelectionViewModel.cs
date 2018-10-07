using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Models.EventArgs;
using DepiBelle.Utilities;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PartSelectionViewModel : ViewModelBase
    {
        private static List<string> _selectedOffers = new List<string>();

        private ObservableCollection<OfferListItem> _offers;

        public ObservableCollection<OfferListItem> Offers
        {
            get { return _offers; }
            set { SetPropertyValue(ref _offers, value); }
        }

        public ICommand OfferSelectedCommand { get; set; }

        public EventHandler<AffordableItem<Offer>> ItemsAddedEventHandler { get; set; }

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
                param = param.OrderBy(o => o.Name).ToList();

                Offers = new ObservableCollection<OfferListItem>();

                param.ForEach(o => Offers.Add(ListItemMapper.GetOfferListItem(o, _selectedOffers.Contains(o.Id), OfferSelectedCommand)));

                IsLoading = false;
            });

        }

        private async Task OfferSelected(OfferListItem offer)
        {
            offer.IsSelected = !offer.IsSelected;


            var affordableItem = new AffordableItem<Offer>()
            {
                Added = offer.IsSelected,
                Item = new Offer(offer.Id,
                                 offer.Name,
                                 offer.Price)
            };

            if (offer.IsSelected)
                _selectedOffers.Add(offer.Id);
            else
                _selectedOffers.Remove(offer.Id);

            ItemsAddedEventHandler.Invoke(this, affordableItem);

        }

    }
}
