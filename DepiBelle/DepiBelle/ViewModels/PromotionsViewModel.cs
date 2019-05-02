using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelle.Services.Notification;
using DepiBelle.Models;
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
        private IDataCollectionService<Promotion> _promotionsDataService;
        private IDialogService _dialogService;

        private ICartNotificationService<Promotion> _cartPromotionManager;

        private ObservableCollection<PromotionItem> _promotions;

        public ObservableCollection<PromotionItem> Promotions
        {
            get { return _promotions; }
            set { SetPropertyValue(ref _promotions, value); }
        }

        public ICommand PromotionSelectedCommand { get; set; }

        public PromotionsViewModel()
        {
            IsLoading = true;
            PromotionSelectedCommand = new Command<PromotionItem>(async (promotion) => await PromotionSelected(promotion));
            _configService = _configService ?? DependencyContainer.Resolve<IConfigService>();
            _dialogService = _dialogService ?? DependencyContainer.Resolve<IDialogService>();
            _promotionsDataService = _promotionsDataService ?? DependencyContainer.Resolve<IDataCollectionService<Promotion>>();
            _promotionsDataService.Initialize(new DataServiceConfig() { Uri = _configService.ServiceUri, Key = _configService.Promotions });

            _cartPromotionManager = _cartPromotionManager ?? DependencyContainer.Resolve<ICartNotificationService<Promotion>>();
            _cartPromotionManager.ItemRemoved += PromotionRemovedHandler;
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            try
            {

                var queryLike = new QueryLike();
                queryLike.LikeField = typeof(Promotion)
                                .GetProperty(nameof(Promotion.Name))
                                .GetPropertyAttribute((Plugin.CloudFirestore.Attributes.MapToAttribute dna) => dna.Mapping);

                queryLike.LikeValue = "Promo 3";

                var queryOrderByName = new QueryOrderBy();
                queryOrderByName.OrderByField = typeof(Promotion)
                                .GetProperty(nameof(Promotion.Name))
                                .GetPropertyAttribute((Plugin.CloudFirestore.Attributes.MapToAttribute dna) => dna.Mapping);
                queryOrderByName.IsDescending = false;
                var queryOrderByPrice = new QueryOrderBy();
                queryOrderByPrice.OrderByField = typeof(Promotion)
                                .GetProperty(nameof(Promotion.Price))
                                .GetPropertyAttribute((Plugin.CloudFirestore.Attributes.MapToAttribute dna) => dna.Mapping);

                queryOrderByPrice.IsDescending = false;

                //var promotions = await _promotionsDataService.GetAll(queryLike: queryLike);

                //var promotions = await _promotionsDataService.GetAll(querysOrderBy: new List<QueryOrderBy>() { queryOrderByPrice, queryOrderByName });

                var promotions = await _promotionsDataService.GetAll();

                promotions = promotions.OrderBy(p => p.Name).ToList();

                Promotions = new ObservableCollection<PromotionItem>();

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

        private async Task PromotionSelected(PromotionItem promotion)
        {
            promotion.IsSelected = !promotion.IsSelected;

            if (promotion.IsSelected)
            {
                var cartItem = new CartItem<Promotion>()
                {
                    Item = new Promotion(promotion.Id,
                                  promotion.Price,
                                  promotion.Name,
                                  promotion.Description)
                };

                _cartPromotionManager.ItemAdded.Invoke(this, cartItem);
            }
            else
                _cartPromotionManager.ItemRemoved(this, promotion.Id);

        }

    }
}
