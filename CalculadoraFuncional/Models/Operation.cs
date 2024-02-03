using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    internal class Operation
    {
        private static List<char> ValidOperation = new()
        {
            '*',
            '+',
            '/',
            '÷',
            '-',
            '='
        };

        private char operation;
        public char OperationValue
        { 
            get { return operation; } 
            set 
            { 
                if(IsOperation(value))
                    this.operation = value; 
            }  
        }
        
        static public bool IsOperation(char op)
        {
            return ValidOperation.Contains(op);
        }

    }
}
