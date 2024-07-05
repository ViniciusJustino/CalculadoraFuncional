using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using CalculadoraFuncional.Services;
using CalculadoraFuncional.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace CalculadoraFuncional
{
    public partial class App : Application
    {
        private static UserDetails _userDetails;
        public static UserDetails UserDetails 
        { 
            get { return _userDetails; } 
            set 
            { 
                if(value != null)
                {
                    _userDetails = value;
                    ConnectDatabase(_userDetails);
                }
                 
            }
        }
        public static Stream creditialFirebase;
        public static bool isBusy;
        public static IHandlerDatabase database = new FirestoreService();
        public static LocalDatabaseSQLite localDatabase = new LocalDatabaseSQLite();
        public App()
        {
            var assembly = Assembly.GetExecutingAssembly();

            string _pathCreditialFirebase = $"{assembly.GetName().Name}.Properties.calculator-app-c2l2t1-firebase-adminsdk-n77k8-561fb7d461.json";
            creditialFirebase = assembly.GetManifestResourceStream(_pathCreditialFirebase);

            _ = localDatabase.Init();

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
        
        private static async void ConnectDatabase(UserDetails _user)
        {
            database = await database.CreateIntanceWithCredentialAsync(_user);
        }

    }
}