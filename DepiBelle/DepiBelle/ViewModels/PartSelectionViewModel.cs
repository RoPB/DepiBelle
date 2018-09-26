using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DepiBelle.Models;

namespace DepiBelle.ViewModels
{
    public class PartSelectionViewModel : ViewModelBase
    {
        private ObservableCollection<OfferListItem> _offers;

        public ObservableCollection<OfferListItem> Offers
        {
            get { return _offers; }
            set { SetPropertyValue(ref _offers, value); }
        }


        public PartSelectionViewModel()
        {
            IsLoading = true;
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            await Task.Run(() =>
            {
                IsLoading = true;

                var param = navigationData as List<Offer>;

                Offers = new ObservableCollection<OfferListItem>();

                param.ForEach(o => Offers.Add(new OfferListItem() { Name = o.Name, Price = o.Price }));

                IsLoading = false;
            });

        }

    }
}
