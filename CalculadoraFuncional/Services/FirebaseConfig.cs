using Firebase.Auth.Providers;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace CalculadoraFuncional.Services
{
    public class FirebaseConfig
    {
        public static readonly string APIKey = "AIzaSyDFin1UVmNWPGvGPntdMUCCURFOfqESk0I";
        public static readonly string ProjectId = "calculator-app-c2l2t1";
        public static readonly string AuthDomian = "calculator-app-c2l2t1.firebaseapp.com";
        public static readonly string HandleAuthDomainGoogle = "calculator-app-c2l2t1.firebaseapp.com/__/auth/handle";

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

        public static FirebaseApp FirebaseAppServicesInit()
        {
            
                return FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromStream(App.creditialFirebase),
                    ProjectId = "calculator-app-c2l2t1"
                }) ;
            
        }
    }
}
