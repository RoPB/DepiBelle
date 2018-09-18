using System;
using System.Threading.Tasks;
using DepiBelle.Services.Authentication;
using Firebase.Auth;

namespace DepiBelle.Droid.Services.GoogleFirebase.Authentication
{
    public class FirebaseAuthService : IAuthenticationService
    {
        private FirebaseAuthProvider App { get; set; }

        public string Token { get; private set; }
        private string RefreshToken { get; set; }


        public bool Initialize(string token)
        {
            if (App == null)
                App = new FirebaseAuthProvider(new FirebaseConfig(token));

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

                var firebaseAuth = new Firebase.Auth.FirebaseAuth(){RefreshToken= RefreshToken };
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
