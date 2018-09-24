using System;
using DepiBelle.Managers.Application;
using DepiBelle.Models;
using DepiBelle.Services.Data;
using DepiBelle.Services.Data.LocalData;
using DepiBelle.Services.Dialog;
using DepiBelle.Services.Modal;
using DepiBelle.Services.Navigation;
using DepiBelle.ViewModels;
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

            Locator.CurrentMutable.Register(() => new LocalDataService(), typeof(ILocalDataService));
           
            //Managers
            Locator.CurrentMutable.RegisterConstant(new ApplicationManager(), typeof(IApplicationManager));

            //ViewModels
            Locator.CurrentMutable.RegisterLazySingleton(() => new HomeTabbedViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new PromotionsViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new BodySelectionViewModel());
            Locator.CurrentMutable.RegisterLazySingleton(() => new PurchaseViewModel());


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
