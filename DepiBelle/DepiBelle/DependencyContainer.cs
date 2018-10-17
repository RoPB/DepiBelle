using System;
using DepiBelle.Managers.Application;
using DepiBelle.Managers.Cart;
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

namespace DepiBelle
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
            Locator.CurrentMutable.RegisterConstant(new CartManager<Promotion>(), typeof(ICartManager<Promotion>));
            Locator.CurrentMutable.RegisterConstant(new CartManager<Offer>(), typeof(ICartManager<Offer>));

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
