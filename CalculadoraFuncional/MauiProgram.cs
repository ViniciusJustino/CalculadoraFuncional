using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Services;
using CalculadoraFuncional.ViewModels;
using CalculadoraFuncional.Views;
using Microsoft.Extensions.Logging;

namespace CalculadoraFuncional
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            /*builder.Services.AddSingleton<IRegisterService>();
            builder.Services.AddSingleton<ILoginService>();*/

            builder.Services.AddSingleton<FirebaseConfig>();
            builder.Services.AddSingleton<LocalDatabaseSQLite>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<MonthlyBillPage>();
            builder.Services.AddSingleton<MonthlyBillViewModel>();
            builder.Services.AddSingleton<BillPage>();
            builder.Services.AddSingleton<BillViewModel>();

            return builder.Build();
        }
    }
}