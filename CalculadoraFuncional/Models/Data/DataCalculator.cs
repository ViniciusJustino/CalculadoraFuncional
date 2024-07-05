using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models.Data
{
    [Table("Calculator")]
    public class DataCalculator
    {
        public string Operator1 { get; set; }

        [MaxLength(10)]
        public string Operator2 { get; set; }

        [MaxLength(10)]
        public string Result { get; set; }

        [MaxLength(1)]
        public char Operation { get; set; }

        [MaxLength(1)]
        public char NextOperation { get; set; }

        public bool CalculationCompleted { get; set; }
    }
}
