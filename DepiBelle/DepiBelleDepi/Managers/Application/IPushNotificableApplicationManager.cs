using System;
using System.Threading.Tasks;
using DepiBelleDepi.Models.PushNotifications;

namespace DepiBelleDepi.Managers.Application
{
    public interface IPushNotificableApplicationManager
    {
        Task Initialize(PushNotification pushNotification = null);

        Task HandlePushNotification(bool openedByTouchNotification, bool isAppInBackground, PushNotification pushNotification);

        Task TrySendToken();
    }
}
