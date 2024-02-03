using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    internal class Operator
    {
        private string value;
        [StringLength(10)]
        public string OperatorValue 
        {
            get { return this.value; }
            set 
            {
                if( value.All(c => char.IsDigit(c) || c.Equals('.') || c.Equals(',')) )
                {
                    this.value = value;
                }
            } 
        }
    }
}
