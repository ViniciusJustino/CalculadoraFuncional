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

        readonly IRegisterService registerService = new FirebaseRegisterService();


        public RegisterViewModel()
        {
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        }

        private async Task RegisterAsync()
        {
            UserDetails _userDatails = await registerService.CreateFirstName(Firstname)
                                                   .CreateLastName(Lastname)
                                                   .CreatePhone(Phone)
                                                   .CreateEmail(Username)
                                                   .CreatePassword(Password)
                                                   .RegisterAsync();

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
    }
}
