using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
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
            catch(Exception ex)
            {

            }

            return await Task.FromResult(new UserDetails());
        }

        public async Task<UserDetails> LoginWithGoogle()
        {
            //NOT WORK!!!
            var config = FirebaseAuthConfig();


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

            }

            return await Task.FromResult(new UserDetails());
        }

        private async Task<string> OpenBrowser(string url)
        {
            try
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
            }
            return null;
        }

    }
}
