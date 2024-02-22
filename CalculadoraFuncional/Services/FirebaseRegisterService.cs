using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Services
{
    internal class FirebaseRegisterService : FirebaseConfig, IRegisterService
    {
        public async Task<UserDetails> Register(string username, string password, string name)
        {
            var config = FirebaseAuthConfig();


            var client = new FirebaseAuthClient(config);

            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var userCredential = await client.CreateUserWithEmailAndPasswordAsync(username, password);
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

            }
            catch (Exception ex)
            {

            }

            return await Task.FromResult(new UserDetails());
        }
    }
}
