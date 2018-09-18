using System;
using System.Threading.Tasks;

namespace DepiBelle.Managers.Application
{
    public interface IApplicationManager
    {
        Task Login(string email, string password);
    }
}
