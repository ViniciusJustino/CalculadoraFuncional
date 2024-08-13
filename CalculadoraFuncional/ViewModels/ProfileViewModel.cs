using CalculadoraFuncional.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    
    public partial class ProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _userNickName = "Teste";
        [ObservableProperty]
        private bool _isRefreshing;
        public ObservableCollection<Option> listOptions { get; set; }
        public ICommand GoToOptionCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public ProfileViewModel() 
        {
            
            Init();
            GoToOptionCommand = new AsyncRelayCommand<Option>(GotoOptionAsync);
            RefreshCommand = new AsyncRelayCommand(RefreshOptionsAsync);
        }

        private async Task Init()
        {
            IEnumerable<Option> _listOptions = await Option.LoadAll();

            this.listOptions = new ObservableCollection<Option>(_listOptions.OrderBy( o => o.Id));
            OnPropertyChanged(nameof(this.listOptions));
        }


        private async Task GotoOptionAsync(Option option)
        {
            try 
            { 
                await Shell.Current.GoToAsync(option.NamePage);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
        }

        private async Task RefreshOptionsAsync()
        {
            IsRefreshing = true;

            await Init();

            IsRefreshing = false;
        }
    }
}
