using System;
using DepiBelleDepi.Services;
using Xamarin.Forms;

namespace DepiBelleDepi.Droid.Services
{
    public class DeviceService:IDeviceService
    {
        string IDeviceService.DeviceId => Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
    }
}
