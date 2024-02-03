using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CalculadoraFuncional.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CalculadoraFuncional.ViewModels
{
    struct ResultSearch
    {
        public int invalidCharIndex, operationCharIndex;

        public ResultSearch(int index1, int index2)
        {
            this.invalidCharIndex = index1;
            this.operationCharIndex = index2;
        }
    }

    internal class CalculatorViewModel : Behavior<Entry>, IObserver<object>, INotifyPropertyChanged
    {
        private Calculator _calculator;
        public Calculator calculator { 
            get { return _calculator; }
            set { 
                if (_calculator != value) 
                {
                    _calculator = value;
                    OnPropertyChanged(nameof(CalculatorViewModel));
                }
                 
            }
        }
        public HistoryCalculatorViewModel historyCalculator { get; set; }

        public event PropertyChangedEventHandler EntryPropertyChanged;

        public ICommand TextChanged { get; private set; }
        public ICommand CalculatorButtonClickCommmand { get; private set; }
        public ICommand DeteleButtonClickCommmand { get; private set; }
        public ICommand EqualsButtonClickCommmand { get; private set; }
        public ICommand ScrollCommand { get; private set; }

        [RegularExpression(@"^[0-9+\-*/]+$", ErrorMessage = "Somenete números e operadores numéricos")]
        public string Expression 
        {
            get => _calculator.Expression;
            set
            {
                _calculator.Expression = value;
                OnPropertyChanged(nameof(_calculator.Expression));
            }
        }
        public string Id 
        {   
            get => _calculator.Id; 
        }

        public CalculatorViewModel()
        {
            calculator ??= new Models.Calculator();
            CalculatorButtonClickCommmand = new AsyncRelayCommand<string>(CalculatorButtonClickTask);
            DeteleButtonClickCommmand = new AsyncRelayCommand(DeteleButtonClickTask);
            EqualsButtonClickCommmand = new AsyncRelayCommand(EqualsButtonClickTask);

            Subscribe(calculator);
        }
        
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += on_TextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= on_TextChanged;
            base.OnDetachingFrom(bindable);
        }

        private async Task CalculatorButtonClickTask(string text)
        {
            Debug.WriteLine($"Button click '{text}'");
            Expression += text;
            OnPropertyChanged(nameof(Expression));
        }

        private async Task DeteleButtonClickTask()
        {
            if (this.Expression?.Length > 0)
            {
               Expression = this.Expression?.Remove(this.Expression.Length - 1, 1);
            }
        }

        private async Task EqualsButtonClickTask()
        {
            calculator?.ProcessCalculate();
            OnPropertyChanged(nameof(_calculator.Expression));
        }

        void on_TextChanged(object sender, TextChangedEventArgs text)
        {
            int newTextValueLength = string.IsNullOrEmpty(text.NewTextValue) ? 0 : text.NewTextValue.Length;
            int oldTextValueLength = string.IsNullOrEmpty(text.OldTextValue) ? 0 : text.OldTextValue.Length;
            Entry entry = (Entry)sender;

            Debug.WriteLine($"Text at entry field '{entry.Text}'");
            Debug.WriteLine($"Text at old text '{text.OldTextValue}'");
            Debug.WriteLine($"Text at new text '{text.NewTextValue}'");

            if (!string.IsNullOrEmpty(text.NewTextValue) &&
                newTextValueLength > oldTextValueLength)
            {
                //Insert
                if(!IsOperatorOrOparationValue(ref entry) && !IsFirstCharAnOperation(ref entry))
                {
                    if (SelectWhatIsOperantioAndOperator(ref entry, ref text))
                        Debug.WriteLine($"Calculation: {calculator?.Operator1?.OperatorValue} {calculator?.Operation?.OperationValue} {calculator?.Operator2?.OperatorValue}");
                    //OnPropertyChanged(nameof(_calculator.Expression));
                }
                
            }
            else if(newTextValueLength < oldTextValueLength)
            {
                //Remove
                //Debug.WriteLine($"Different character: {DifferentCharacters(text.OldTextValue, text.NewTextValue)}");

                //OnPropertyChanged(nameof(Expression));
            }
        }

        private bool IsOperatorOrOparationValue(ref Entry entry)
        {
            string text = entry.Text;

            int result = SearchInvalidChar(ref text, 0, text.Length - 1);

            if (result != -1)
            {
                entry.Text = text.Remove(result, 1);
                return true;
            }

            return false;

        }

        private static bool IsFirstCharAnOperation(ref Entry entry)
        {
            string text = entry.Text;

            if(text.Length == 1 && Operation.IsOperation(text[0]))
            {
                entry.Text = text.Remove(0, 1);
                return true;
            }

            return false;
        }

        private int SearchInvalidChar(ref string text, int start, int end)
        {
            int mid = (start + end) / 2;

            int result = -1;
            if ((start + end) % 2 != 0)
            {
                result = SearchInvalidChar(ref text, start, mid);
                if (result != -1)
                    return result;
                result = SearchInvalidChar(ref text, mid + 1, end);
                if (result != -1)
                    return result;
            }
            else
            {
                if (!Operation.IsOperation(text[mid]) &&
                    !char.IsDigit(text[mid]))
                    result = mid;
                else
                {
                    result = -1;
                }

                if (result == -1 && start != end)
                {
                    result = SearchInvalidChar(ref text, start, mid);
                    if (result != -1)
                        return result;
                    result = SearchInvalidChar(ref text, mid, end);
                    if (result != -1)
                        return result;
                }
            }

            return result;
        }

        private int DifferentCharactersIndex(ref Entry entry, ref TextChangedEventArgs text)
        {
            int cursorPosition = entry.CursorPosition;

            string newTextValue = text.NewTextValue;
            string oldTextValue = text.OldTextValue;

            int newTextValueLength = newTextValue?.Length != null ? newTextValue.Length : 0;
            int oldTextValueLength = oldTextValue?.Length != null ? oldTextValue.Length : 0; 


            for (int i = 0; i < newTextValueLength - oldTextValueLength; i++)
            {
                string alterText = $"{oldTextValue?[..(cursorPosition - 1)]}_";
                if (oldTextValue?.Length > cursorPosition)
                    alterText += oldTextValue?[cursorPosition..];

                oldTextValue = alterText;
            }

            int newCharIndex = -1;

            for(int i = 0; i < newTextValue.Length; i++)
            {
                if (newTextValue?[i] != oldTextValue?[i])
                    newCharIndex = i;
            }

            return newCharIndex;
        }

        private bool SelectWhatIsOperantioAndOperator(ref Entry entry, ref TextChangedEventArgs text)
        {

            int indexNewChar = DifferentCharactersIndex(ref entry, ref text);
            int operationIndex = SearchValidOperationIndex(entry.Text, 0, entry.Text.Length);
            bool findOperationIndexOnText = operationIndex != -1;

            if (!findOperationIndexOnText && !Operation.IsOperation(entry.Text[indexNewChar]))
                return false;
            else if (Operation.IsOperation(entry.Text[indexNewChar]) && calculator.Operation != null)
            {
                if(!calculator.IsValidCalculation())
                {
                    string textEntry = entry.Text;

                    if (textEntry.Length > 0 && 
                        Operation.IsOperation(entry.Text[indexNewChar]) && 
                        indexNewChar != operationIndex)
                    {
                        entry.Text = textEntry.Remove(indexNewChar, 1);
                    }

                    return false;
                }
                calculator.NextOperation ??= new Operation() { OperationValue = entry.Text[indexNewChar] };
                calculator.ProcessCalculate();
                return true;
            }
            
            if(findOperationIndexOnText)
            {
                if(calculator.Operator1 == null && (operationIndex+1 <= entry.Text.Length))
                {
                    calculator.Operator1 = new Operator() { OperatorValue = entry.Text[..operationIndex] };
                }

                if(calculator.Operator2 == null && (entry.Text.Length > operationIndex + 1)) 
                {
                    calculator.Operator2 = new Operator() { OperatorValue = entry.Text.Substring(operationIndex + 1) };
                }
               
                if (calculator.Operator1 != null &&
                    !calculator.Operator1.OperatorValue.Equals(entry.Text.Substring(0, operationIndex)))
                {
                    calculator.Operator1.OperatorValue = entry.Text.Substring(0, operationIndex);
                }

                if (calculator.Operator2 != null &&
                    !calculator.Operator2.OperatorValue.Equals(entry.Text.Substring(operationIndex + 1)))
                {
                    calculator.Operator2.OperatorValue = entry.Text.Substring(operationIndex + 1);
                }

                calculator.Operation ??= new Operation() { OperationValue = entry.Text[operationIndex] };
                
                if(calculator.Operation != null &&
                   !calculator.Operation.OperationValue.Equals(entry.Text[operationIndex]))
                {
                    calculator.Operation.OperationValue = entry.Text[operationIndex];
                }
            }
            
            if(!calculator.Expression.Equals(entry.Text))
                calculator.Expression = entry.Text;

            return true;
        }

        private int SearchValidOperationIndex(string text, int start, int end)
        {
            int mid = (start + end) / 2;

            int result = -1;
            if ((start + end) % 2 != 0)
            {
                result = SearchValidOperationIndex(text, start, mid);
                if (result != -1)
                    return result;
                result = SearchValidOperationIndex(text, mid + 1, end);
                if (result != -1)
                    return result;
            }
            else
            {
                if (mid <= text.Length - 1)
                {
                    if (Operation.IsOperation(text[mid]))
                        result = mid;
                    else
                    {
                        result = -1;
                    }
                }
                else
                {
                    result = -1;
                }

                if (result == -1 && start != end)
                {
                    result = SearchValidOperationIndex(text, start, mid);
                    if (result != -1)
                        return result;
                    result = SearchValidOperationIndex(text, mid, end);
                    if (result != -1)
                        return result;
                }
            }

            return result;
        }

        public virtual void Subscribe(IObservable<object> provider)
        {
            provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
            Calculator newCalculator = new();

            NewCalculator(ref _calculator, ref newCalculator);

            foreach (IObserver<object> observer in calculator.observers)
            {
                observer.OnNext(newCalculator);
            }

            calculator = newCalculator;
        }

        public void NewCalculator(ref Calculator oldCalculator, ref Calculator newCalculator)
        {
            if (oldCalculator.CalculationCompleted)
            {
                newCalculator.Operator1 = oldCalculator.Result;
                newCalculator.Operation = oldCalculator.NextOperation;
                newCalculator.Expression = oldCalculator.Result?.OperatorValue + oldCalculator.NextOperation?.OperationValue;

                foreach (IObserver<object> observer in oldCalculator.observers)
                {
                    newCalculator.Subscribe(observer);
                }
            }

            Subscribe(newCalculator);
        }

        public virtual void OnError(Exception error)
        {
            
        }

        public virtual void OnNext(object value)
        {
            
        }

    }
}
