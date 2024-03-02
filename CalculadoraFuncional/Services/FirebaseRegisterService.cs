using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using Firebase.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Services
{
    internal class FirebaseRegisterService : FirebaseConfig, IRegisterService
    {
        public string _country { get; private set; }
        public string _state { get; private set; }
        public string _city { get; private set; }
        public string _email { get; private set; }
        public string _password { get; private set; }
        public string _phone { get; private set; }
        public string _firstName { get; private set; }
        public string _lastName { get; private set; }
        public DateTime _birthday { get; private set; }

        public IRegisterService CreateEmail(string email)
        {
            this._email = email;
            return this;
        }

        public IRegisterService CreatePassword(string password)
        {
            this._password = password;
            return this;
        }

        public IRegisterService CreateCountry(string country)
        {
            this._country = country;
            return this;
        }

        public IRegisterService CreateState(string state)
        {
            this._state = state;
            return this;
        }
        public IRegisterService CreateCity(string city)
        {
            this._city = city;
            return this;
        }

        public IRegisterService CreateBirthday(DateTime birthday)
        {
            this._birthday = birthday;
            return this;
        }

        public IRegisterService CreateFirstName(string firstName)
        {
            this._firstName = firstName;    
            return this;
        }

        public IRegisterService CreateLastName(string lastName)
        {
            this._lastName = lastName;
            return this;
        }

        public IRegisterService CreatePhone(string phone)
        {
            this._phone = phone;
            return this;
        }
        public async Task<UserDetails> RegisterAsync()
        {
            
            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    FirebaseApp credential = FirebaseConfig.FirebaseAppServicesInit();
                    var defaultAuth = FirebaseAuth.GetAuth(credential);

                    if (defaultAuth != null)
                    {
                        UserRecord user = await defaultAuth.CreateUserAsync(new UserRecordArgs()
                        {
                            Email = _email,
                            EmailVerified = true,
                            PhoneNumber = _phone,
                            Password = _password,
                            Disabled = false,
                            DisplayName = _firstName
                        });


                        UserDetails userDetails = new UserDetails()
                        {
                            Id = user.Uid,
                            Name = user.DisplayName,
                            Token = await defaultAuth.CreateCustomTokenAsync(user.Uid),
                            User = user
                        };

                        return userDetails;

                    }

                }
                else
                {
                    return null;
                }
            }
            catch (ArgumentNullException ex)
            {
                await App.Current.MainPage.DisplayAlert("Register Error", $"Erro ao autenticar-se com a base de dados: {ex.Message}", "OK");
            }
            catch(InvalidOperationException ex)
            {
                await App.Current.MainPage.DisplayAlert("Register Error", $"Erro ao registrar token de acesso: {ex.Message}", "OK");
            }
            catch (ArgumentException ex)
            {
                await App.Current.MainPage.DisplayAlert("Register Error", $"Erro ao registrar token de acesso: {ex.Message}", "OK");
            }
            catch (FirebaseAdmin.Auth.FirebaseAuthException ex)
            {
                await App.Current.MainPage.DisplayAlert($"Register Error {ex.ErrorCode}", $"Erro ao registrar dados do usuário: {ex.Message}", "OK");
            }

            return await Task.FromResult(new UserDetails());
        }
    }
}
