using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    [Table("Category")]
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set;  }
        public string NameCategory { get; set; }
        [MaxLength(7)]
        public string ColorHex { get; set; }

        public static async ValueTask<IEnumerable<Category>> LoadAllAsync()
        {
            /*if (App.isBusy)
                return await App.database.GetAllBills(App.UserDetails);*/

            return await App.localDatabase.GetAllCategories();

        }
    }
}
