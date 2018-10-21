using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data;
using DepiBelleDepi.Services.Dialog;
using DepiBelleDepi.Services.Notification.Cart;
using DepiBelleDepi.Utilities;
using Xamarin.Forms;

namespace DepiBelleDepi.ViewModels
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
            _promotionsDataService.Initialize(new DataServiceConfig() { Uri = _configService.Uri, Key = _configService.Promotions });

            _cartPromotionManager = _cartPromotionManager ?? DependencyContainer.Resolve<ICartNotificationService<Promotion>>();
            _cartPromotionManager.ItemRemoved += PromotionRemovedHandler;
        }

        public override async Task InitializeAsync(object navigationData = null)
        {
            try
            {

                var promotions = await _promotionsDataService.GetAll();
                promotions = promotions.OrderBy(p => p.Name).ToList();

                Promotions = new ObservableCollection<PromotionItem>();

                promotions.ForEach(p => Promotions.Add(ListItemMapper.GetPromotionListItem(p, false, PromotionSelectedCommand)));

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
