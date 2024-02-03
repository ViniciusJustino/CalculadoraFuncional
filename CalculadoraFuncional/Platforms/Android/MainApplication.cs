using Android.App;
using Android.Runtime;

namespace CalculadoraFuncional
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

    /*HANDLING EVENTS ON ANDROID EXAMPLE
     * 
     * public static MauiApp CreateMauiApp()
{
   var builder = MauiApp.CreateBuilder();
   builder
       .UseMauiApp<App>()
       .ConfigureLifecycleEvents(events =>
       {
#if ANDROID
           events.AddAndroid(android => android
               .OnActivityResult((activity, requestCode, resultCode, data) => LogEvent(nameof(AndroidLifecycle.OnActivityResult), requestCode.ToString()))
               .OnStart((activity) => LogEvent(nameof(AndroidLifecycle.OnStart)))
               .OnCreate((activity, bundle) => LogEvent(nameof(AndroidLifecycle.OnCreate)))
               .OnBackPressed((activity) => LogEvent(nameof(AndroidLifecycle.OnBackPressed)) && false)
               .OnStop((activity) => LogEvent(nameof(AndroidLifecycle.OnStop))));
#endif
       });

   return builder.Build();
}
*/
}