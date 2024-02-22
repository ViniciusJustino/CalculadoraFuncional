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
    internal class FireBaseLoginService : FirebaseConfig, ILoginService
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

        
    }
}
