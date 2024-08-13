using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    [Table("Option")]
    public class Option
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NickName { get; set; }
        public string NamePage { get; set; }

        public async static ValueTask<IEnumerable<Option>> LoadAll()
        {
            return await App.localDatabase.GetAllOptions();
        }
    }
}
