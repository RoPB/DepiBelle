using System;
using DepiBelle.Managers.Application;
using DepiBelle.Services.Notification;
using DepiBelle.Models;
using DepiBelle.Services.Config;
using DepiBelle.Services.Data.DataQuery;
using DepiBelle.Services.Data.LocalData;
using DepiBelle.Services.Dialog;
using DepiBelle.Services.Modal;
using DepiBelle.Services.Navigation;
using DepiBelle.ViewModels;
using DepiBelle.ViewModels.Modals;
using Splat;
using DepiBelle.Services.Data;
using DepiBelle.Services.Authentication;

namespace DepiBelle
{
    public class DependencyContainer
    {
        public static void RegisterDependencies()
        {
            //Services
#if PRODUCTION
            Locator.CurrentMutable.RegisterConstant(new ConfigServiceProd(), typeof(IConfigService));
#else
            Locator.CurrentMutable.RegisterConstant(new ConfigServiceDev(), typeof(IConfigService));
#endif

            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new ModalService(), typeof(IModalService));
            Locator.CurrentMutable.RegisterConstant(new DialogService(), typeof(IDialogService));

            Locator.CurrentMutable.RegisterConstant(new MiyuAuthService(), typeof(IAuthenticationService));//new AuthService(), typeof(IAuthenticationService));
            //register not as singleton so you can change the configuration whenever you need
            Locator.CurrentMutable.Register(() => new CFDataCollectionSecuredService<Offer>(), typeof(IDataCollectionService<Offer>));
            Locator.CurrentMutable.Register(() => new CFDataCollectionSecuredService<Order>(), typeof(IDataCollectionService<Order>));
            Locator.CurrentMutable.Register(() => new CFDataCollectionSecuredService<Promotion>(), typeof(IDataCollectionService<Promotion>));
            Locator.CurrentMutable.RegisterConstant(new DataQuerySecuredService<Config>(), typeof(IDataQueryService<Config>));
            Locator.CurrentMutable.Register(() => new LocalDataService(), typeof(ILocalDataService));

            //ViewModels
            RegisterRefreshableViewModelDependencies();
            Locator.CurrentMutable.Register(() => new PartSelectionViewModel());

            //Managers
            RegisterRefreshableManagersDependencies();
            Locator.CurrentMutable.RegisterConstant(new ApplicationManager(), typeof(IApplicationManager));


            //ModalViewModels
            Locator.CurrentMutable.Register(() => new ConfirmationModalViewModel());
        }

        public static void Refresh()
        {
            RegisterRefreshableViewModelDependencies();
            RegisterRefreshableManagersDependencies();
        }

        private static void RegisterRefreshableViewModelDependencies()
        {

            Locator.CurrentMutable.RegisterLazySingleton(() => new WelcomeViewModel());
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
