using CalculadoraFuncional.Interface;
using CalculadoraFuncional.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Services
{
    public partial class FirestoreService : FirebaseConfig, IHandlerDatabase
    {
        private FirestoreDb firebaseDb;

        public async ValueTask<IHandlerDatabase> CreateIntanceWithCredentialAsync(UserDetails _user)
        {
            UserDetails user;

            if (_user != null)
            {
                user = _user;
            }
            else
            {
                string pref = Preferences.Get(nameof(App.UserDetails), "Unknown");
                if (pref == "Unknown")
                    return null;

                user = (UserDetails)JsonConvert.DeserializeObject(pref);
            }

            FirestoreClientBuilder client = new FirestoreClientBuilder
            {
                Credential = GoogleCredential.FromAccessToken(user.Token)
            };

            firebaseDb = FirestoreDb.Create(
                projectId: FirebaseConfig.ProjectId,
                client: client.Build()
            );

            return this;
        }
        public async ValueTask<IEnumerable<Bill>> GetAllBills(UserDetails _user)
        {
            List<Bill> _bills = new();
            Dictionary<string, object> docDataEmpity = new Dictionary<string, object>{};

            DocumentReference bills = firebaseDb.Collection("bills").Document(_user.Id);
            DocumentSnapshot billsSnapshot = await bills.GetSnapshotAsync();

            if (!billsSnapshot.Exists)
            {
                await firebaseDb.Collection("bills").Document(_user.Id).SetAsync(docDataEmpity);
            }

            Query historyBillsQuery = bills.Collection("historyBills");

            QuerySnapshot historyBillsQuerySnapshot = await historyBillsQuery.GetSnapshotAsync();

            if (historyBillsQuerySnapshot.Count <= 0)
            {
                await firebaseDb.Collection("bills").Document(_user.Id).Collection("historyBills").Document("0").SetAsync(docDataEmpity);
            }

            foreach (DocumentSnapshot billSnapshot in historyBillsQuerySnapshot.Documents)
            {
                Dictionary<string, object> bill = billSnapshot.ToDictionary();
                Bill _bill = new Bill();

                _bill.Id = Convert.ToInt32(billSnapshot.Id);

                foreach (KeyValuePair<string, object> pair in bill)
                {
                    if(pair.Key == "Category")
                    {
                        var a = pair.Value;
                        continue;
                    }
                    try
                    {
                        _bill.GetType().GetProperty(pair.Key)?.SetValue(_bill, pair.Value, null);
                    }
                    catch (Exception ex) 
                    {
                        var propertier = _bill.GetType().GetProperty(pair.Key);

                        Timestamp timestamp = (Timestamp)pair.Value;
                        _bill.GetType().GetProperty(pair.Key)?.SetValue(_bill, timestamp.ToDateTime(), null);
                    }
                }

                _bills.Add(_bill);
            }

            return _bills;
        }
    }
}
