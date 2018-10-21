using System;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data.DataQuery;
using DepiBelleDepi.Services.Data.LocalData;
using DepiBelleDepi.Services.Dialog;
using DepiBelleDepi.Services.Modal;
using DepiBelleDepi.Services.Navigation;
using DepiBelleDepi.Services.Notification.Cart;
using DepiBelleDepi.ViewModels;
using Splat;

namespace DepiBelleDepi
{
    public class DependencyContainer
    {
        public static void RegisterDependencies()
        {
            //Services
            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new ModalService(), typeof(IModalService));
            Locator.CurrentMutable.RegisterConstant(new DialogService(), typeof(IDialogService));
            Locator.CurrentMutable.RegisterConstant(new DataQuerySecuredService<Config>(), typeof(IDataQueryService<Config>));
#if PRODUCTION
            Locator.CurrentMutable.RegisterConstant(new ConfigServiceProd(), typeof(IConfigService));
#else
            Locator.CurrentMutable.RegisterConstant(new ConfigServiceDev(), typeof(IConfigService));
#endif
            Locator.CurrentMutable.Register(() => new LocalDataService(), typeof(ILocalDataService));

            //ViewModels
            RegisterRefreshableViewModelDependencies();
            Locator.CurrentMutable.Register(() => new PartSelectionViewModel());

            //Managers
            RegisterRefreshableManagersDependencies();
            Locator.CurrentMutable.RegisterConstant(new ApplicationManager(), typeof(IApplicationManager));


            //ModalViewModels
            //Locator.CurrentMutable.Register(() => new ConfirmationModalViewModel());
        }

        public static void Refresh()
        {
            RegisterRefreshableViewModelDependencies();
            RegisterRefreshableManagersDependencies();
        }

        private static void RegisterRefreshableViewModelDependencies()
        {

            Locator.CurrentMutable.RegisterLazySingleton(() => new OrdersViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new HomeTabbedViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new PromotionsViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new BodySelectionViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new PurchaseViewModel());

        }

        private static void RegisterRefreshableManagersDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new CartNotificationService<Promotion>(), typeof(ICartNotificationService<Promotion>));
            Locator.CurrentMutable.RegisterConstant(new CartNotificationService<Offer>(), typeof(ICartNotificationService<Offer>));

        }

        public static T Resolve<T>()
        {
            return Locator.CurrentMutable.GetService<T>();
        }

        public static object Resolve(Type viewModelType)
        {
            return Locator.CurrentMutable.GetService(viewModelType);
        }
    }
}
