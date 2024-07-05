using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{

    public class Bill
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public DateTime Date { get; set; }

        //[FirestoreProperty]
        public double Value { get; set; }

        //[FirestoreProperty]
        public int IdCategory { get; set; }


        public static async ValueTask<IEnumerable<Bill>> LoadAllAsync()
        { 
            if(App.isBusy)
                return await App.database.GetAllBills(App.UserDetails);

            return await App.localDatabase.GetAllBills();

        }

    }
}
