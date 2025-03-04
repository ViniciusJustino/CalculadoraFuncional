﻿
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
    
    internal class Calculator : ObservableObject, IObservable<object>
    {
        public List<IObserver<object>> observers = new();

        [MaxLength(10)]
        public Operator Operator1 { get; set; }

        [MaxLength(10)]
        public Operator Operator2 { get; set; }

        [MaxLength(10)]
        public Operator Result { get; set; }

        [MaxLength(1)]
        public Operation Operation { get; set; }

        [MaxLength(1)]
        public Operation NextOperation { get; set; }

        public bool CalculationCompleted { get; set; }

        
        private string _expression;

        [MaxLength(50)]
        public string Expression
         { 
             get => _expression;
             set
             {
                 _expression = value;
                 OnPropertyChanged(nameof(Expression));
             } 
         }

        private static Dictionary<char, Func<decimal, decimal, decimal>> Compute = new()
        {
            { '+', (a, b) => a + b },
            { '-', (a, b) => a - b },
            { '*', (a, b) => a * b },
            { '/', (a, b) => a / b },
            { '÷', (a, b) => a / b }
        };

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int IdBill { get; set; }

        public Calculator() 
        {

        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return null;
        }

        public bool ProcessCalculate(Operator operator1, Operator operator2, Operation operation)
        {
            if (operator1 == null && operator2 != null) 
            {
                if (this.Result != null)
                {
                    operator1 = this.Result;
                    this.Expression += operation?.OperationValue + operator2?.OperatorValue;
                }
                else
                    return false;
            }
            else if(operator1 != null && operator2 == null)
            {
                if (this.Result != null)
                {
                    operator2 = this.Result;
                    this.Expression += operation?.OperationValue + operator1?.OperatorValue;
                }
                else
                    return false;
            }
            else
            {
                if (operator1 != null && operator2 != null && operation != null)
                    this.Expression = operator1?.OperatorValue + operation?.OperationValue + operator2?.OperatorValue;
                else
                    return false;
            }

            this.Result = Calculate(operator1, operator2, operation);

            if (this.Result.OperatorValue != null)
            {
                this.CalculationCompleted = true;
                foreach (IObserver<object> observer in observers)
                {
                    observer.OnCompleted();
                }

                this.Expression = operator1?.OperatorValue + operation?.OperationValue + operator2?.OperatorValue + " = " + this.Result?.OperatorValue;
            }

            return true;
        }
        public bool ProcessCalculate()
        {
            return this.ProcessCalculate(Operator1, Operator2, Operation);
        }

        private Operator Calculate(Operator operator1, Operator operator2, Operation operation)
        {
            DataTable data = new();
            char op;

            if (operation.OperationValue == '÷')
                op = '/';
            else
                op = operation.OperationValue;

            var valueComputed = Compute[op](Convert.ToDecimal(operator1.OperatorValue), Convert.ToDecimal(operator2.OperatorValue));
            if (valueComputed == Math.Floor(valueComputed))
                valueComputed = Convert.ToInt32(valueComputed);
            string valueComputedToString = Convert.ToString(valueComputed);

            Operator result = new Operator() { OperatorValue = valueComputedToString };

            return result;
        }

        public bool IsValidCalculation()
        {
            return (this.Operator1 != null && this.Operation != null && this.Operator2 != null);
        }

    }
}
