using CalculadoraFuncional.Resources.Theme;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    internal class ConfigurationViewModel : INotifyPropertyChanged
    {
        public string ThemeText { get; set; }

        private static ConfigurationViewModel _instance;
        private Theme _theme;
        public static ConfigurationViewModel Instance => _instance ??= new ConfigurationViewModel();
        public event PropertyChangedEventHandler PropertyChanged;

        public Theme Theme
        {
            get => _theme;
            set
            {
                if (_theme == value) return;
                _theme = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Theme)));
        }
        private ConfigurationViewModel() 
        {
            Theme = Theme.System;
        }


    }
}
