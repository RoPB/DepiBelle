using System;
using System.Threading.Tasks;

namespace DepiBelle.Services.Authentication
{
    public interface IAuthenticationService
    {
        bool Initialize(string token);

        string Token { get;}

        Task<bool> Authenticate(string email, string password);

        Task<bool> RefreshSession();
    }
}

