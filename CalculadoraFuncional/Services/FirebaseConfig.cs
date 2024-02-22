using Firebase.Auth.Providers;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Services
{
    internal class FirebaseConfig
    {
        public static readonly string APIKey = "AIzaSyDFin1UVmNWPGvGPntdMUCCURFOfqESk0I";
        public static readonly string AuthDomian = "calculator-app-c2l2t1.firebaseapp.com";

        public static FirebaseAuthConfig FirebaseAuthConfig()
        {
            return new FirebaseAuthConfig
            {
                ApiKey = APIKey,
                AuthDomain = AuthDomian,
                Providers = new FirebaseAuthProvider[]
                            {
                                new GoogleProvider().AddScopes("email"),
                                new EmailProvider()
                            }
            };
        }
    }
}
