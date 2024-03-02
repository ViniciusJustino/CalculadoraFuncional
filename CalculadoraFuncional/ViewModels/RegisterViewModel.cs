using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using CalculadoraFuncional.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    public partial class RegisterViewModel: BaseViewModel
    {
        public ICommand RegisterCommand { get; private set;  }
        [ObservableProperty]
        private string _messageerror;
        [ObservableProperty]
        private bool _iserror = false;
        [ObservableProperty]
        private bool _isrunning = false;
        [ObservableProperty]
        private string _firstname;
        [ObservableProperty]
        private string _lastname;
        [ObservableProperty]
        private string _phone;
        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private string _repeatpassword;
        [ObservableProperty]
        private DateTime _birthday;

        readonly IRegisterService registerService = new FirebaseRegisterService();

        public RegisterViewModel()
        {
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        }

        private async Task RegisterAsync()
        {
            Isrunning = true;
            UserDetails _userDatails = await registerService.CreateFirstName(Firstname)
                                                   .CreateLastName(Lastname)
                                                   .CreatePhone(Phone)
                                                   .CreateEmail(Username)
                                                   .CreatePassword(Password)
                                                   .CreateBirthday(Birthday)
                                                   .RegisterAsync();

            if (_userDatails.IsNull() || _userDatails == null)
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Unknown)
                    Messageerror = "Acesso a internet indefinido.";
                else if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
                    Messageerror = "Sem conexão a internet.";
                else
                    Messageerror = "Erro ao registrar o usuário.";

                Iserror = true;
                Isrunning = false;
                return;
            }

            if (Preferences.ContainsKey(nameof(App.UserDetails)))
            {
                Preferences.Remove(nameof(App.UserDetails));
            }

            string userDatails = JsonConvert.SerializeObject(_userDatails);

            Preferences.Set(nameof(App.UserDetails), userDatails);

            App.UserDetails = _userDatails;
            Isrunning = false;
            await Shell.Current.GoToAsync("//app");

        }
    }
}
