using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Services
{
    public class FireBaseLoginService : FirebaseConfig, ILoginService
    {
        
        public async Task<UserDetails> Login(string username, string password)
        {

            var config = FirebaseAuthConfig();
            var client = new FirebaseAuthClient(config);

            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var userCredential = await client.SignInWithEmailAndPasswordAsync(username, password);
                    var user = userCredential.User;

                    var refreshToken = user.Credential.RefreshToken;
                    string token = await user.GetIdTokenAsync();

                    FirebaseAuth auth = FirebaseAuth.GetAuth( FirebaseConfig.FirebaseAppServicesInit());

                    UserRecord _user = await auth.GetUserAsync(user.Uid);

                    return await Task.FromResult(new UserDetails
                    {
                        Name = _user.DisplayName,
                        Id = _user.Uid,
                        Token = token,
                        RefreshToken = user.Credential.RefreshToken,
                        User = _user
                    });
                }
                else
                {
                    return null;
                }

            }
            catch(ArgumentNullException ex)
            {
                await App.Current.MainPage.DisplayAlert("Login Error", $"Erro ao autenticar-se com a base de dados: {ex.Message}", "OK");
            }
            catch (ArgumentException ex)
            {
                await App.Current.MainPage.DisplayAlert("Login Error", $"Erro ao acessar dados do usuário: {ex.Message}", "OK");
            }

            return await Task.FromResult(new UserDetails());
        }

        public async Task<UserDetails> LoginWithGoogle()
        {
            //NOT WORK!!!
            /*var config = FirebaseAuthConfig();


            var client = new FirebaseAuthClient(config);

            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var userCredential = await client.SignInWithRedirectAsync(FirebaseProviderType.Google, async uri =>
                    {
                        return await OpenBrowser(uri);
                    });

                    var user = userCredential.User;

                    var refreshToken = user.Credential.RefreshToken; // more properties are available in user.Credential
                    var token = user.GetIdTokenAsync();

                    return await Task.FromResult(new UserDetails
                    {
                        Name = user.Info.FirstName,
                        Id = user.Uid,
                        Token = await user.GetIdTokenAsync(),
                        RefreshToken = user.Credential.RefreshToken

                    });
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

            }*/

            return await Task.FromResult(new UserDetails());
        }

        private async Task<string> OpenBrowser(string url)
        {
           /* try
            {
                Uri Url = new Uri(url);
                Uri UriCallback = new Uri($"https://{HandleAuthDomainGoogle}");

                WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    Url,
                    UriCallback);

                string accessToken = authResult?.AccessToken;

                return null;
            } catch(Exception e)
            {
                App.Current.MainPage.DisplayAlert("Falha de Login", e.Message, "OK");
            }*/
            return null;
        }

    }
}
