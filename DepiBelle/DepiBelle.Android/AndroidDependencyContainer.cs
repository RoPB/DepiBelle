using System;
using Splat;
using DepiBelle.Droid.Services.GoogleFirebase.Data;
using DepiBelle.Droid.Services.GoogleFirebase.Authentication;
using DepiBelle.Services.Authentication;
using DepiBelle.Services.Data;
using DepiBelle.Models;

namespace DepiBelle.Droid
{
    public class AndroidDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new AuthService(), typeof(IAuthenticationService));

            //register not as singleton so you can change the configuration whenever you need
            Locator.CurrentMutable.Register(() => new DataCollectionSecuredService<Offer>(), typeof(IDataCollectionService<Offer>));
            Locator.CurrentMutable.Register(()=>new DataCollectionSecuredService<Order>(), typeof(IDataCollectionService<Order>));
            Locator.CurrentMutable.Register(() => new DataCollectionSecuredService<Promotion>(), typeof(IDataCollectionService<Promotion>));
        }

    }
}
