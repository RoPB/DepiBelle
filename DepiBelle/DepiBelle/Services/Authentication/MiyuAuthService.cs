using System;
using System.Threading.Tasks;
using Plugin.FirebaseAuth;

namespace DepiBelle.Services.Authentication
{
    public class MiyuAuthService : IAuthenticationService
    {
        private IAuth Auth { get; set; }
        private IAuthResult Result { get; set; }
        public string Token { get; private set; }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                Result = await Auth.SignInWithEmailAndPasswordAsync(email, password);

                var loggedWell = Result.User != null;

                if (loggedWell)
                {
                    Token = await Result.User.GetIdTokenAsync(false);
                }

                return loggedWell;
            }
            catch(Exception ex)
            {
                throw ex;
            }
       }

        public bool Initialize(string key)
        {
            Auth = CrossFirebaseAuth.Current.Instance;//GetInstance(key);
            return true;
        }

        public async Task<bool> RefreshSession()
        {
            Token = await Result.User.GetIdTokenAsync(true);

            return true;
        }
    }
}
