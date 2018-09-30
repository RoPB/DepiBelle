﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PromotionsViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private IDataService<Promotion> _promotionsDataService;

        private ObservableCollection<PromotionListItem> _promotions;

        public ICommand PromotionSelectedCommand { get; set; }

        public ObservableCollection<PromotionListItem> Promotions
        {
            get { return _promotions; }
            set { SetPropertyValue(ref _promotions, value); }
        }

        public EventHandler<int> ItemsAddedEventHandler { get; set; }

        public PromotionsViewModel()
        {
            IsLoading = true;
            PromotionSelectedCommand = new Command<PromotionListItem>(async (promotion) => await PromotionSelected(promotion));
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _promotionsDataService = _promotionsDataService ?? DependencyContainer.Resolve<IDataService<Promotion>>();
            _promotionsDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Promotions });
        }

        public override async Task InitializeAsync(object navigationData=null)
        {

            var promotions = await _promotionsDataService.GetAll();

            Promotions = new ObservableCollection<PromotionListItem>();

            promotions.ForEach(p => Promotions.Add(new PromotionListItem() { Name = p.Name, Description = p.Description, Price = p.Price }));

            IsLoading = false;
        }

        private async Task PromotionSelected(PromotionListItem promotion)
        {
            promotion.IsSelected = !promotion.IsSelected;
        }

    }
}
