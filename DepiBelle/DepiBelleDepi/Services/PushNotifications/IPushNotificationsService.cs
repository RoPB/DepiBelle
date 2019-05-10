using System;
namespace DepiBelleDepi.Services.PushNotifications
{
    public interface IPushNotificationsService
    {
        string GetToken();

        bool IsValidToken();
    }
}
