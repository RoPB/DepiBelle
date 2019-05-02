using System;
using System.Threading.Tasks;
using Firebase.Auth;

namespace DepiBelleDepi.Services.Authentication
{
    public class AuthService : IAuthenticationService
    {
        private FirebaseAuthProvider App { get; set; }

        public string Token { get; private set; }
        private string RefreshToken { get; set; }


        public bool Initialize(string key="")
        {
            if (App == null)
                App = new FirebaseAuthProvider(new FirebaseConfig(key));

            return true;
        }

        private bool IsServiceInitialized()
        {

            if (App == null)
                throw new Exception("Have to Initialize Auth Service");

            return true;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                IsServiceInitialized();

                var response = await App.SignInWithEmailAndPasswordAsync(email, password);

                Token = response.FirebaseToken;
                RefreshToken = response.RefreshToken;

                return !string.IsNullOrEmpty(Token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RefreshSession()
        {
            try
            {
                IsServiceInitialized();

                var firebaseAuth = new Firebase.Auth.FirebaseAuth() { RefreshToken = RefreshToken };
                var response = await App.RefreshAuthAsync(firebaseAuth);

                Token = response.FirebaseToken;
                RefreshToken = response.RefreshToken;

                return !string.IsNullOrEmpty(Token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
