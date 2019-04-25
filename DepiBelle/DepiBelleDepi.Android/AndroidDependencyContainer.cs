using System;
using DepiBelleDepi.Droid.Services;
using DepiBelleDepi.Models;
using DepiBelleDepi.Services;
using DepiBelleDepi.Services.Authentication;
using DepiBelleDepi.Services.Data;
using Splat;

namespace DepiBelleDepi.Droid
{
    public class AndroidDependencyContainer
    {
        public static void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new DeviceService(), typeof(IDeviceService));
        }
    }
}
