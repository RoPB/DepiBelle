using System;
using DepiBelleDepi.Droid.Services.GoogleFirebase.Authentication;
using DepiBelleDepi.Droid.Services.GoogleFirebase.Data;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services.Authentication;
using DepiBelleDepi.Services.Data;
using Splat;

namespace DepiBelleDepi.Droid
{
    public class AndroidDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new AuthService(), typeof(IAuthenticationService));

            //register not as singleton so you can change the configuration whenever you need
            Locator.CurrentMutable.Register(() => new DataCollectionSecuredService<Offer>(), typeof(IDataCollectionService<Offer>));
            Locator.CurrentMutable.Register(() => new DataCollectionSecuredService<Order>(), typeof(IDataCollectionService<Order>));
            Locator.CurrentMutable.Register(() => new DataCollectionSecuredService<Promotion>(), typeof(IDataCollectionService<Promotion>));
        }
    }
}
