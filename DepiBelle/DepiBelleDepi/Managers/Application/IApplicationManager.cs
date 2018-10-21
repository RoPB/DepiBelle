using System;
using System.Threading.Tasks;

namespace DepiBelleDepi.Managers.Application
{
    public interface IApplicationManager
    {
        Task Login(string email, string password);
    }
}
