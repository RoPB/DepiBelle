﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Models;
using DepiBelle.Models.EventArgs;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data;
using DepiBelle.Services.Dialog;
using DepiBelle.Utilities;
using Xamarin.Forms;

namespace DepiBelle.ViewModels
{
    public class PromotionsViewModel : ViewModelBase
    {
        private IConfigService _configService;
        private IDataService<Promotion> _promotionsDataService;
        private IDialogService _dialogService;

        private ObservableCollection<PromotionListItem> _promotions;

        public ObservableCollection<PromotionListItem> Promotions
        {
            get { return _promotions; }
            set { SetPropertyValue(ref _promotions, value); }
        }

        public ICommand PromotionSelectedCommand { get; set; }

        public EventHandler<AffordableItem<Promotion>> ItemsAddedEventHandler { get; set; }

        public PromotionsViewModel()
        {
            IsLoading = true;
            PromotionSelectedCommand = new Command<PromotionListItem>(async (promotion) => await PromotionSelected(promotion));
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _dialogService = _dialogService ?? DependencyContainer.Resolve<IDialogService>();
            _promotionsDataService = _promotionsDataService ?? DependencyContainer.Resolve<IDataService<Promotion>>();
            _promotionsDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Promotions });
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            try
            {

                var promotions = await _promotionsDataService.GetAll();
                promotions = promotions.OrderBy(p => p.Name).ToList();

                Promotions = new ObservableCollection<PromotionListItem>();

                promotions.ForEach(p => Promotions.Add(ListItemMapper.GetPromotionListItem(p,false,PromotionSelectedCommand)));

            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Se produjo un problema en la descarga de datos (PROMOCIONES)", "ERROR", "OK");
            }
            finally
            {
                IsLoading = false;
            }

        }

        public void PromotionRemovedHandler(object sender, string promotionId)
        {
            Promotions.First(p => p.Id == promotionId).IsSelected = false;
        }

        private async Task PromotionSelected(PromotionListItem promotion)
        {
            promotion.IsSelected = !promotion.IsSelected;

            var affordableItem = new AffordableItem<Promotion>()
            {
                Added = promotion.IsSelected,
                Item = new Promotion(promotion.Id,
                                     promotion.Name,
                                     promotion.Description,
                                     promotion.Price)
            };

            ItemsAddedEventHandler.Invoke(this, affordableItem);
        }

    }
}
