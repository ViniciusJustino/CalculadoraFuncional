
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    
    public class Calculator :  ObservableObject, IObservable<object>
    {
        
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int IdBill { get; set; }

        [SQLite.Column("Operator1")]
        [MaxLength(10)]
        public string _operator1 { get; set; }

        [SQLite.Column("Operator2")]
        [MaxLength(10)]
        public string _operator2 { get; set; }

        [SQLite.Column("Result")]
        [MaxLength(10)]
        private string _result { get; set; }

        [SQLite.Column("Operation")]
        [MaxLength(1)]
        public string _operation { get; set; }

        [SQLite.Column("NextOperation")]
        [MaxLength(1)]
        public string _nextOperation { get; set; }

        [SQLite.Column("Expression")]
        [MaxLength(50)]
        public string _expression { get; set; }

        [Ignore]
        public List<IObserver<object>> observers { get; set; } = new();

        [Ignore]
        private List<string> ValidOperation { get; set; } = new()
        {
            "*",
            "+",
            "/",
            "÷",
            "-",
            "="
        };

        [Ignore]
        public string Operator1 
        {
            get { return this._operator1; }
            set
            {
                if (value.All(c => char.IsDigit(c) || c.Equals('.') || c.Equals(',')))
                {
                    this._operator1 = value;
                }
            } 
        }

        [Ignore]
        public string Operator2
        {
            get { return this._operator2; }
            set
            {
                if (value.All(c => char.IsDigit(c) || c.Equals('.') || c.Equals(',')))
                {
                    this._operator2 = value;
                }
            }
        }
        
        [Ignore]
        public string Result
        {
            get { return this._result; }
            set
            {
                if (value.All(c => char.IsDigit(c) || c.Equals('.') || c.Equals(',')))
                {
                    this._result = value;
                }
            }
        }

        [Ignore]
        public string Operation
        {
            get { return this._operation; }
            set
            {
                if (ValidOperation.Contains(value) || value == string.Empty || value == "")
                {
                    this._operation = value;
                }
            }
        }

        [Ignore]
        public string NextOperation
        {
            get { return this._nextOperation; }
            set
            {
                if (ValidOperation.Contains(value))
                {
                    this._nextOperation = value;
                }
            }
        }

        public bool CalculationCompleted { get; set; }

        [Ignore]
        public string Expression
         { 
             get => _expression;
             set
             {
                 _expression = value;
                 OnPropertyChanged(nameof(Expression));
             } 
         }

        [Ignore]
        private static Dictionary<string, Func<decimal, decimal, decimal>> Compute { get; set; } = new()
        {
            { "+", (a, b) => a + b },
            { "-", (a, b) => a - b },
            { "*", (a, b) => a * b },
            { "/", (a, b) => a / b },
            { "÷", (a, b) => a / b }
        };

        public IDisposable Subscribe(IObserver<object> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return null;
        }

        public bool ProcessCalculate(string operator1, string operator2, string operation)
        {
            /* if (!string.IsNullOrEmpty(operator1) && !string.IsNullOrEmpty(operator2)) 
             {
                 if (!string.IsNullOrEmpty( this.Result ))
                 {
                     operator1 = this.Result;
                     this.Expression += operation + operator2;
                 }
                 else
                     return false;
             }
             else if(!string.IsNullOrEmpty(operator1) && string.IsNullOrEmpty(operator2))
             {
                 if (!string.IsNullOrEmpty(this.Result))
                 {
                     operator2 = this.Result;
                     this.Expression += operation + operator1;
                 }
                 else
                     return false;
             }
             else
             {
                 if (!string.IsNullOrEmpty(operator1) && !string.IsNullOrEmpty(operator2) && operation != '\0')
                     this.Expression = operator1 + operation + operator2;
                 else
                     return false;
             }*/

            if (!string.IsNullOrEmpty(operator1) && !string.IsNullOrEmpty(operator2) && !string.IsNullOrEmpty(operation))
            {
                this.Result = Calculate(operator1, operator2, operation);
            }
            else
                return false;

            if (!string.IsNullOrEmpty(this.Result))
            {
                this.CalculationCompleted = true;
                foreach (IObserver<object> observer in observers)
                {
                    observer.OnCompleted();
                }

                this.Expression = operator1 + operation + operator2 + " = " + this.Result;
            }

            return true;
        }
        public bool ProcessCalculate()
        {
            return this.ProcessCalculate(Operator1, Operator2, Operation);
        }

        private string Calculate(string operator1, string operator2, string operation)
        {
            DataTable data = new();
            string op;

            if (operation == "÷")
                op = "/";
            else
                op = operation;

            var valueComputed = Compute[op](Convert.ToDecimal(operator1), Convert.ToDecimal(operator2));
            if (valueComputed == Math.Floor(valueComputed))
                valueComputed = Convert.ToInt32(valueComputed);
            
            return Convert.ToString(valueComputed); ;
        }

        public bool IsValidCalculation()
        {
            return (!string.IsNullOrEmpty( this.Operator1 ) && !string.IsNullOrEmpty( this.Operation) && !string.IsNullOrEmpty( this.Operator2 ));
        }

        public static async ValueTask<IEnumerable<Calculator>> LoadAllAsync()
        {
            /*if (App.isBusy)
                return await App.database.GetAllBills(App.UserDetails);*/

            return await App.localDatabase.GetAllCalculators();

        }

    }
}
