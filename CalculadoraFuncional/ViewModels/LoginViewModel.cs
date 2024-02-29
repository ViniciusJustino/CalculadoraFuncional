using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using CalculadoraFuncional.Services;
using CalculadoraFuncional.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; private set; }
        public ICommand RecoveryCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand LoginGoogleCommand { get; private set; }
        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private string _messageErrorLogin;
        [ObservableProperty]
        private bool _isErrorLogin;
        readonly ILoginService loginService = new FireBaseLoginService();


        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RecoveryCommand = new AsyncRelayCommand(RecoveryAsync);
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
            LoginGoogleCommand = new AsyncRelayCommand(LoginWithGoogleAsync);
        }

        private async Task LoginAsync()
        {
             if(!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                UserDetails _userDatails = await loginService.Login(Username, Password);

                if (_userDatails.IsNull())
                    return;

                if (Preferences.ContainsKey(nameof(App.UserDetails)))
                {
                    Preferences.Remove(nameof(App.UserDetails));
                }

                string userDatails = JsonConvert.SerializeObject(_userDatails);

                Preferences.Set(nameof(App.UserDetails), userDatails);

                App.UserDetails = _userDatails;

                await Shell.Current.GoToAsync("//app");
            }
            else
            {
                if(string.IsNullOrEmpty(Username))
                {
                    MessageErrorLogin = string.IsNullOrEmpty(Password) ? "Usuário e senha não preenchidos." : "Usuário não preenchido.";
                }
                else if(string.IsNullOrEmpty(Password))
                {
                    MessageErrorLogin = "Senha não preenchida.";
                }
            }
        }

        private async Task LoginWithGoogleAsync()
        {
            UserDetails _userDatails = await loginService.LoginWithGoogle();
        }

        private async Task RecoveryAsync()
        {
            string a = ".";
        }

        private async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}
