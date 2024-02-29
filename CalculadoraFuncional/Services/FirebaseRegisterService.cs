using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using Firebase.Auth;
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
                    var credential = FirebaseConfig.FirebaseAppServicesInit();
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
                        }) ;

                        return new UserDetails()
                        { 
                            Id = user.Uid,
                            Name = user.DisplayName,
                            User = user
                        };
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return await Task.FromResult(new UserDetails());
        }
        public async Task<UserDetails> Register(string username, string password, string name)
        {
            return await Task.FromResult(new UserDetails());
        }
    }
}
