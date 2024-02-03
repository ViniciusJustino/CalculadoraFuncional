using CalculadoraFuncional.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace CalculadoraFuncional
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            SetTheme();

            ConfigurationViewModel.Instance.PropertyChanged += OnSettingsPropertyChanged;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window =  base.CreateWindow(activationState);

            window.Created += (s, e) => 
            {
                Debug.WriteLine("Evento OnCreate executado");
            };

            window.Activated += (s, e) =>
            {
                Debug.WriteLine("Evento OnActivate executado");
                Window win = (Window)s;
            };

            window.Deactivated += (s, e) => { Debug.WriteLine("Evento OnDeactivated executado"); };

            window.Stopped += (s, e) => { Debug.WriteLine("Evento OnStopped executado"); };

            window.Resumed += (s, e) => { Debug.WriteLine("Evento OnResumed executado"); };

            window.Destroying += (s, e) => { Debug.WriteLine("Evento OnDestroying executado"); };

            window.Backgrounding += (s, e) => { Debug.WriteLine("Evento OnBackgrounding executado"); };

           if( DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst ||
                DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                window.Height = 700;
                window.Width = 400;  
            }

           
            return window;
        }

        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConfigurationViewModel.Theme))
            {
                SetTheme();
            }
        }


        private void SetTheme()
        {
            UserAppTheme = ConfigurationViewModel.Instance?.Theme != null
                         ? ConfigurationViewModel.Instance.Theme.AppTheme
                         : AppTheme.Unspecified;
        }

    }
}