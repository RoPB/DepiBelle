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
            Locator.CurrentMutable.RegisterConstant(new FirebaseAuthService(), typeof(IAuthenticationService));

            //register not as singleton so you can change the configuration whenever you need
            Locator.CurrentMutable.Register(() => new FirebaseSecuredDataService<Offer>(), typeof(IDataService<Offer>));
            Locator.CurrentMutable.Register(()=>new FirebaseSecuredDataService<Order>(), typeof(IDataService<Order>));
            Locator.CurrentMutable.Register(() => new FirebaseSecuredDataService<Promotion>(), typeof(IDataService<Promotion>));
        }

    }
}
