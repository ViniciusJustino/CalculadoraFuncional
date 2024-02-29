using Android.App;
using Android.Content.PM;

namespace CalculadoraFuncional
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
              Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
              DataScheme = CALLBACK_SCHEME)]
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
        const string CALLBACK_SCHEME = "https";
    }
    /*[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
    DataScheme = "https",
    DataHost = "calculator-app-c2l2t1.firebaseapp.com",
    DataPath = "/__/auth/handler")]
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
 
    }*/
}
