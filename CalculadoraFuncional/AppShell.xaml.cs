namespace CalculadoraFuncional
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.LoginPage), typeof(Views.LoginPage));
            Routing.RegisterRoute(nameof(Views.RegisterPage), typeof(Views.RegisterPage));
            Routing.RegisterRoute(nameof(Views.MonthlyBillPage), typeof(Views.MonthlyBillPage));
            Routing.RegisterRoute(nameof(Views.BillPage), typeof(Views.BillPage));
        }

    }
}