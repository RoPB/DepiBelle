using System;
using System.Threading.Tasks;

namespace DepiBelleDepi.Managers.Application
{
    public interface IAuthenticableApplicationManager
    {
        Task Login(string email, string password);
    }
}
