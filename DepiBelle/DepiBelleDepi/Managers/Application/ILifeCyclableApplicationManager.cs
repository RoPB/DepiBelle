using System;
namespace DepiBelleDepi.Managers.Application
{
    public interface ILifeCyclableApplicationManager
    {
        void OnStart();
        void OnSleep();
        void OnResume();
    }
}
