﻿using System;
using DepiBelleDepi.Managers.Application;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Authentication;
using DepiBelleDepi.Services.Config;
using DepiBelleDepi.Services.Data;
using DepiBelleDepi.Services.Data.DataQuery;
using DepiBelleDepi.Services.Data.LocalData;
using DepiBelleDepi.Services.Dialog;
using DepiBelleDepi.Services.Modal;
using DepiBelleDepi.Services.Navigation;
using DepiBelleDepi.Services.Notification.Cart;
using DepiBelleDepi.ViewModels;
using DepiBelleDepi.ViewModels.Modals;
using Splat;

namespace DepiBelleDepi
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

            Locator.CurrentMutable.RegisterConstant(new MiyuAuthService(), typeof(IAuthenticationService));
            //register not as singleton so you can change the configuration whenever you need
            Locator.CurrentMutable.Register(() => new CFDataCollectionSecuredService<Offer>(), typeof(IDataCollectionService<Offer>));
            Locator.CurrentMutable.Register(() => new CFDataCollectionSecuredService<Order>(), typeof(IDataCollectionService<Order>));
            Locator.CurrentMutable.Register(() => new CFDataCollectionSecuredService<Promotion>(), typeof(IDataCollectionService<Promotion>));
            Locator.CurrentMutable.Register(() => new CFDataCollectionSecuredService<User>(), typeof(IDataCollectionService<User>));

            Locator.CurrentMutable.RegisterConstant(new DataQuerySecuredService<Config>(), typeof(IDataQueryService<Config>));
            Locator.CurrentMutable.Register(() => new LocalDataService(), typeof(ILocalDataService));

            //ViewModels
            RegisterRefreshableViewModelDependencies();
            Locator.CurrentMutable.Register(() => new PartSelectionViewModel());

            //Managers
            RegisterRefreshableManagersDependencies();

            var applicationManager = new ApplicationManager();
            Locator.CurrentMutable.RegisterConstant(applicationManager, typeof(IAuthenticableApplicationManager));
            Locator.CurrentMutable.RegisterConstant(applicationManager, typeof(IPushNotificableApplicationManager));

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
