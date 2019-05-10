using DepiBelleDepi.Droid.Services;
using DepiBelleDepi.Droid.Services.PushNotifications;
using DepiBelleDepi.Services;
using DepiBelleDepi.Services.PushNotifications;
using Splat;

namespace DepiBelleDepi.Droid
{
    public class AndroidDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new DeviceService(), typeof(IDeviceService));
            Locator.CurrentMutable.Register(() => new PushNotificationsService(), typeof(IPushNotificationsService));
        }
    }
}
