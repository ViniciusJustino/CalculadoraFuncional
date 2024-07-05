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
        private string _messageerror;
        [ObservableProperty]
        private string _iserror = "False";
        [ObservableProperty]
        private string _isrunning = "False";
        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private string _messageErrorLogin;
        [ObservableProperty]
        private bool _isErrorLogin;
        [ObservableProperty]
        private bool _isCheck;
        readonly ILoginService loginService = new FireBaseLoginService();


        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RecoveryCommand = new AsyncRelayCommand(RecoveryAsync);
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
            LoginGoogleCommand = new AsyncRelayCommand(LoginWithGoogleAsync);
            IsCheck = Preferences.Default.Get("direct_login", false);

            IsDirectLoginAsync();
        }

        private async Task LoginAsync()
        {
             if(!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                Isrunning = "True";
                UserDetails _userDatails = await loginService.Login(Username, Password);

                if (_userDatails.IsNull())
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Unknown)
                        Messageerror = "Acesso a internet indefinido.";
                    else if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
                        Messageerror = "Sem conexão a internet.";
                    else
                        Messageerror = "Erro ao logar o usuário.";

                    Iserror = "True";
                    Isrunning = "False";
                    return;
                }

                AccessAppAsync(_userDatails);
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

        private async Task LoginWithTokenAsync(string _token)
        {
            if (_token != null && _token != "")
            {
                UserDetails _userDatails = await loginService.LoginWithToken(_token);

                if (_userDatails.IsNull())
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Unknown)
                        Messageerror = "Acesso a internet indefinido.";
                    else if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
                        Messageerror = "Sem conexão a internet.";
                    else
                        Messageerror = "Erro ao logar o usuário.";

                    Iserror = "True";
                    Isrunning = "False";
                    return;
                }

                AccessAppAsync(_userDatails);
            }
            else
            {
                if (string.IsNullOrEmpty(Username))
                {
                    MessageErrorLogin = string.IsNullOrEmpty(Password) ? "Usuário e senha não preenchidos." : "Usuário não preenchido.";
                }
                else if (string.IsNullOrEmpty(Password))
                {
                    MessageErrorLogin = "Senha não preenchida.";
                }
            }
        }

        private async Task LoginWithGoogleAsync()
        {
            UserDetails _userDatails = await loginService.LoginWithGoogle();
        }

        private async void IsDirectLoginAsync()
        {
            if (IsCheck)
            {
                string oauthToken = await SecureStorage.Default.GetAsync("token_access");

                if (oauthToken != null)
                {
                    //_ = LoginWithTokenAsync(oauthToken);
                }
            }
        }

        private async Task RecoveryAsync()
        {
            string a = ".";
        }

        private async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        private async void AccessAppAsync(UserDetails _userDatails)
        {
            if (Preferences.ContainsKey(nameof(App.UserDetails)))
            {
                Preferences.Remove(nameof(App.UserDetails));
            }

            string userDatails = JsonConvert.SerializeObject(_userDatails);

            Preferences.Set(nameof(App.UserDetails), userDatails);

            App.UserDetails = _userDatails;

            if (IsCheck)
            {
                await SecureStorage.Default.SetAsync("token_access", _userDatails.Token);
                Preferences.Default.Set("direct_login", true);
            }

            App.isBusy = true;

            Isrunning = "False";
            await Shell.Current.GoToAsync("//app");
        }
    }
}
