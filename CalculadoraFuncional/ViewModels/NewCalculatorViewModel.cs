using CalculadoraFuncional.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls.Compatibility;

namespace CalculadoraFuncional.ViewModels
{
    internal class NewCalculatorViewModel : Behavior<Entry>, IObserver<object>
    {
        //Properties definition
        private Calculator _calculator;
        private Bill _bill;
        public Calculator calculator
        {
            get { return _calculator; }
            set
            {
                if (_calculator != value)
                {
                    _calculator = value;
                    OnPropertyChanged(nameof(calculator));
                    OnPropertyChanged(nameof(EntryText));
                }

            }
        }

        public Category CategorySelected;

        public string EntryText
        {
            get => _calculator.Expression;
            set
            {
                _calculator.Expression = value;
                OnPropertyChanged(nameof(EntryText));
            }
        }
        public string Result
        {
            get => _calculator?.Result;
        }
        public ObservableCollection<Calculator> historyCalc { get; set; }
        public ObservableCollection<Category> categories { get; set; }

        //Commands definition
        public ICommand TextChanged { get; private set; }
        public ICommand CalculatorButtonClickCommmand { get; private set; }
        public ICommand DeteleButtonClickCommmand { get; private set; }
        public ICommand EqualsButtonClickCommmand { get; private set; }
        public ICommand ScrollCommand { get; private set; }
        public ICommand SelectCalculationCommand { get; private set; }
        public ICommand SaveBillCommand { get; private set; }
        public ICommand DeleteBillCommand { get; private set; }

        public NewCalculatorViewModel()
        {
            calculator ??= new Models.Calculator();
            historyCalc = new ObservableCollection<Calculator>();
            CalculatorButtonClickCommmand = new AsyncRelayCommand<string>(CalculatorButtonClickTask);
            DeteleButtonClickCommmand = new AsyncRelayCommand(DeteleButtonClickTask);
            EqualsButtonClickCommmand = new AsyncRelayCommand(EqualsButtonClickTask);
            SelectCalculationCommand = new AsyncRelayCommand<Calculator>(SelectCalculationTask);
            SaveBillCommand   = new AsyncRelayCommand(SaveBillAsync);
            DeleteBillCommand = new AsyncRelayCommand(DeleteBillAsync);

            Init();

            historyCalc.Add(calculator);
            Subscribe(calculator);
        }

        void on_TextChanged(object sender, TextChangedEventArgs text)
        {
            int newTextValueLength = string.IsNullOrEmpty(text.NewTextValue) ? 0 : text.NewTextValue.Length;
            int oldTextValueLength = string.IsNullOrEmpty(text.OldTextValue) ? 0 : text.OldTextValue.Length;
            Entry entry = (Entry)sender;

            if(oldTextValueLength < newTextValueLength)
            {
                entry.Text = FilterCharacter(entry?.Text);

                int indexNewChar = AddDifferentCharactersIndex(ref entry, ref text);
                char newChar = entry.Text[indexNewChar];

                if (char.IsDigit(newChar) || Operation.IsOperation(newChar) || newChar.Equals(','))
                {
                    if(Operation.IsOperation(newChar))
                    {
                        if (!text.OldTextValue.Contains(newChar) || calculator.IsValidCalculation())
                            HandleOperations(ref entry, newChar);
                        else
                            entry.Text = entry.Text.Remove(indexNewChar, 1);

                        if (calculator.IsValidCalculation())
                            calculator.ProcessCalculate();
                    }
                    else if(char.IsDigit(newChar) || newChar.Equals(','))
                    {
                        HandleOperators(ref entry, newChar);
                    }
                }

                //TextToSpeech.SpeakAsync(entry.Text, SpeechOptions).Wait();
            }
            else if(text.OldTextValue.Contains(text.NewTextValue))
            {
                int indexRemoveChar = RemoveDifferentCharactersIndex(ref entry, ref text);
                char removedChar = text.OldTextValue[indexRemoveChar];

                if (char.IsDigit(removedChar) || removedChar.Equals(','))
                {
                    HandleRemoveOperators(ref entry, removedChar);
                }
                else
                {
                    HandleRemoveOperations(ref entry, removedChar);
                }
            }
        }

        private void HandleOperations(ref Entry entry, char newChar)
        {
            if ( !string.IsNullOrEmpty(calculator.Operation))
            {
                if (!calculator.IsValidCalculation())
                {
                    int indexOldOperation = entry.Text.IndexOf(calculator.Operation);

                    if (entry.Text.Contains(calculator.Operation) && !calculator.Operation.Equals(newChar))
                        entry.Text = entry.Text.Remove(indexOldOperation, 1);
                    else if (!entry.Text.Contains(newChar))
                        entry.Text += newChar;

                    calculator.Operation = char.ToString(newChar);
                    return;
                }
                else
                {
                    if ( !string.IsNullOrEmpty(calculator.NextOperation) )
                    {
                        int indexOldOperation = entry.Text.IndexOf(calculator.NextOperation);

                        entry.Text = entry.Text.Remove(indexOldOperation, 1);

                        calculator.NextOperation = char.ToString(newChar);
                        return;
                    }
                    else
                    {
                        calculator.NextOperation =  char.ToString(newChar) ;
                        return;
                    }
                }
            }
            calculator.Operation =  char.ToString(newChar) ;
        }

        private void HandleOperators(ref Entry entry, char newChar)
        {
            int indexOperator = 0;

            if(!string.IsNullOrEmpty(calculator.Operation))
                indexOperator = entry.Text.IndexOf((calculator.Operation));

            if(!string.IsNullOrEmpty(calculator.Operator1)) 
            {
                if (indexOperator > 0)
                {
                    if (!calculator.Operator1.Equals(entry.Text[..(indexOperator)]))
                    {
                        string value = entry.Text[..(indexOperator)];

                        if(value.Length == 1 && value == ",")
                        {
                            value.Insert(0, "0");
                            entry.Text.Insert(0, "0");
                            indexOperator++;
                        }

                        Regex regex = new Regex(",");
                        MatchCollection matches = regex.Matches(value);
                        int countComma = matches.Count;

                        while (countComma > 1)
                        {
                            int indexComma = value.LastIndexOf(',');

                            value = value.Remove(indexComma, 1);
                            entry.Text = entry.Text.Remove(indexComma, 1);
                            countComma--;
                            indexOperator--;
                        }

                        calculator.Operator1 = value;
                    }
                }
                else
                {
                    if (!calculator.Operator1.Equals(entry.Text))
                    {
                        if (entry.Text.Length == 1 && entry.Text == ",")
                            entry.Text.Insert(0, "0");
                        
                        
                        string value = entry.Text;

                        Regex regex = new Regex(",");
                        MatchCollection matches = regex.Matches(value);
                        int countComma = matches.Count;

                        while (countComma > 1)
                        {
                            int indexComma = value.LastIndexOf(',');

                            value = value.Remove(indexComma, 1);
                            entry.Text = entry.Text.Remove(indexComma, 1);
                            countComma--;
                        }

                        calculator.Operator1 = value;
                    }
                }
            }
            if(!string.IsNullOrEmpty(calculator.Operator2)) 
            {
                if (indexOperator > 0)
                {
                    if (!calculator.Operator2.Equals(entry.Text[(indexOperator + 1)..]))
                    {
                        string value = entry.Text[(indexOperator + 1)..];

                        if(value.Length == 1 && value == ",")
                        { 
                            value.Insert(0, "0");
                            entry.Text.Insert(indexOperator + 1, "0");
                        }

                        Regex regex = new Regex(",");
                        MatchCollection matches = regex.Matches(value);
                        int countComma = matches.Count;

                        while (countComma > 1)
                        {
                            int indexComma = value.LastIndexOf(',');

                            value = value.Remove(indexComma, 1);
                            entry.Text = entry.Text.Remove(indexOperator + 1 + indexComma, 1);
                            countComma--;
                            indexOperator--;
                        }

                        calculator.Operator2 = value;
                    }
                }
            }
            if(string.IsNullOrEmpty( calculator.Operator1 ))
            {
                if (indexOperator > 0)
                {
                    if(entry.Text[..(indexOperator)].Length == 1 && entry.Text[..(indexOperator)] == ",")
                    {
                        entry.Text = entry.Text.Insert(0, "0");
                        indexOperator++;
                    }

                    calculator.Operator1 =  entry.Text[..(indexOperator)];
                }
                else
                {
                    if (entry.Text.Length == 1 && entry.Text == ",")
                    {
                        entry.Text = entry.Text.Insert(0, "0");
                    }

                    calculator.Operator1 = entry.Text;
                }
            }
            if(string.IsNullOrEmpty( calculator.Operator2 ))
            {
                if (indexOperator > 0)
                {
                    string value = entry.Text[(indexOperator + 1)..];

                    if(value.Length == 1 && value == ",")
                    {
                        value.Insert(0, "0");
                        entry.Text = entry.Text.Insert(indexOperator + 1, "0");
                    }

                    calculator.Operator2 = value;
                }
            }
        }

        private void HandleRemoveOperators(ref Entry entry, char removedChar)
        {

            if( !string.IsNullOrEmpty(calculator.Operation))
            {
                int indexOperation = entry.Text.IndexOf(calculator.Operation);

                
                string alterOperator1 = entry.Text[..indexOperation];

                if (!calculator.Operator1.Equals(alterOperator1))
                    calculator.Operator1 = alterOperator1;

                if((entry.Text.Length - 1) > indexOperation)
                {
                    string alterOperator2 = entry.Text[(indexOperation + 1)..];

                    if (!calculator.Operator2.Equals(alterOperator2))
                        calculator.Operator2 = alterOperator2;
                }
                else
                    calculator.Operator2 = string.Empty;

            }
            else
            {
                if(entry.Text == null || entry.Text == "")
                {
                    calculator.Operator1 = string.Empty;
                }
                else
                {
                    if (!calculator.Operator1.Equals(entry.Text))
                        calculator.Operator1 = entry.Text;
                }
            }

        }

        private void HandleRemoveOperations(ref Entry entry, char removedChar)
        {
            if(!calculator.CalculationCompleted)
            {
                if ( !string.IsNullOrEmpty(calculator.Operation) &&
                    calculator.Operation.Equals(removedChar.ToString()))
                {
                    calculator.Operation = string.Empty;
                }
            }
        }

        private string FilterCharacter(string entryText)
        {
            var text = new StringBuilder();

            if (entryText != null && entryText != "")
            {
                foreach (var c in entryText)
                {
                    if (char.IsDigit(c) || Operation.IsOperation(c) || c.Equals(',') || c.Equals('.'))
                    {
                       
                        text.Append(c);
                    }
                }
            }

            return text.ToString();
        }

        private int AddDifferentCharactersIndex(ref Entry entry, ref TextChangedEventArgs text)
        {
            string newTextValue = text.NewTextValue;
            string oldTextValue = text.OldTextValue;

            var alterText = new StringBuilder();

            int newTextValueLength = newTextValue?.Length != null ? newTextValue.Length : 0;
            int oldTextValueLength = oldTextValue?.Length != null ? oldTextValue.Length : 0;


            for (int i = 0; i < newTextValueLength; i++)
            {
                if (i < oldTextValueLength)
                { 
                    if (newTextValue?[i] != oldTextValue?[i])
                        alterText.Append('_');
                    else
                        alterText.Append(oldTextValue?[i]);
                }
                else
                    alterText.Append('_');
            }

            oldTextValue = alterText.ToString();

            int newCharIndex = -1;

            for (int i = 0; i < newTextValue.Length; i++)
            {
                if (newTextValue?[i] != oldTextValue?[i])
                    newCharIndex = i;
            }

            return newCharIndex;
        }

        private int RemoveDifferentCharactersIndex(ref Entry entry, ref TextChangedEventArgs text)
        {
            string newTextValue = text.NewTextValue;
            string oldTextValue = text.OldTextValue;

            var alterText = new StringBuilder();

            int newTextValueLength = newTextValue?.Length != null ? newTextValue.Length : 0;
            int oldTextValueLength = oldTextValue?.Length != null ? oldTextValue.Length : 0;


            for (int i = 0; i < oldTextValueLength; i++)
            {
                if (i < newTextValueLength)
                {
                    if (newTextValue?[i] != oldTextValue?[i])
                        alterText.Append('_');
                    else
                        alterText.Append(newTextValue?[i]);
                }
                else
                    alterText.Append('_');
            }

            newTextValue = alterText.ToString();

            int removeCharIndex = -1;

            for (int i = 0; i < newTextValue.Length; i++)
            {
                if (newTextValue?[i] != oldTextValue?[i])
                    removeCharIndex = i;
            }

            return removeCharIndex;
        }

        private async Task CalculatorButtonClickTask(string text)
        {
            Debug.WriteLine($"Button click '{text}'");
            this.EntryText += text;
            OnPropertyChanged(nameof(EntryText));
        }

        private async Task DeteleButtonClickTask()
        {
            if (this.EntryText?.Length > 0)
            {
                this.EntryText = this.EntryText?.Remove(this.EntryText.Length - 1, 1);
            }
        }

        private async Task EqualsButtonClickTask()
        {
            calculator?.ProcessCalculate();
            OnPropertyChanged(nameof(EntryText));
        }

        private async Task SelectCalculationTask(Calculator select)
        {
            Calculator newCalculator = new();

            NewCalculator(ref select, ref newCalculator, true);

            foreach (IObserver<object> observer in calculator.observers)
            {
                observer.OnNext(newCalculator);
            }

            calculator = newCalculator;
        }

        private async Task SaveBillAsync()
        {
            
            await App.localDatabase.UpdateBillAsync(this._bill);

            string name = "bill_" + DateTime.Now.ToString();
            this._bill = new Bill() { Name = name, Date = DateTime.Now };

            await App.localDatabase.AddBill(this._bill);

            this._calculator.IdBill = this._bill.Id;
        }

        private async Task DeleteBillAsync()
        {

        }

        public virtual void Subscribe(IObservable<object> provider)
        {
            provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            Calculator newCalculator = new();

            _bill.Value = Convert.ToDouble(_calculator.Result);
            _= App.localDatabase.UpdateCalculatorAsync(_calculator);

            NewCalculator(ref _calculator, ref newCalculator);

            foreach (IObserver<object> observer in calculator.observers)
            {
                observer.OnNext(newCalculator);
            }

            calculator = newCalculator;
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(object value)
         {
            Calculator newCalculator = (Calculator)value;

            if (!historyCalc.Contains(newCalculator))
                historyCalc.Add(newCalculator);
        }

        public void NewCalculator(ref Calculator oldCalculator, ref Calculator newCalculator, bool isCloneValue = false)
        {
            if (oldCalculator.CalculationCompleted && !isCloneValue)
            {
                newCalculator.Operator1 = oldCalculator.Result;
                newCalculator.Operation = oldCalculator.NextOperation;
                newCalculator.Expression = oldCalculator.Result + oldCalculator.NextOperation;

                foreach (IObserver<object> observer in oldCalculator.observers)
                {
                    newCalculator.Subscribe(observer);
                }
            }
            else
            {
                newCalculator.Operator1 = oldCalculator.Operator1;
                newCalculator.Operator2 = oldCalculator.Operator2;
                newCalculator.Operation = oldCalculator.Operation;
                newCalculator.Expression = oldCalculator.Operator1 + oldCalculator.Operation + oldCalculator.Operator2;

                foreach (IObserver<object> observer in oldCalculator.observers)
                {
                    newCalculator.Subscribe(observer);
                }
            }

            newCalculator.IdBill = this._bill.Id;

            _= App.localDatabase.AddCalculator(newCalculator);

            Subscribe(newCalculator);
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            throw new NotImplementedException();
        }

        private async void Init()
        {
            string name = "bill_" + DateTime.Now.ToString();
            this._bill = new Bill() { Name = name , Date = DateTime.Now };

            this._calculator.IdBill = this._bill.Id;

            categories = new ObservableCollection<Category>(await App.localDatabase.GetAllCategories());
            OnPropertyChanged(nameof(categories));
        }

        public async Task ValidationInDataBaseAsync()
        {
            if (await App.localDatabase.GetBillAsync(this._bill.Id) == null)
            {
                if(this.historyCalc.Count > 1 || this._calculator.CalculationCompleted)
                { 
                    await App.localDatabase.AddBill(this._bill);

                    foreach (Calculator calculator in historyCalc)
                    {
                        calculator.IdBill = this._bill.Id;

                        if(calculator.IsValidCalculation())
                            await App.localDatabase.UpdateCalculatorAsync(calculator);
                    }
                   
                    return;
                }

                return;
            }

            await App.localDatabase.UpdateBillAsync(this._bill);

            foreach (Calculator calculator in historyCalc)
            {
                calculator.IdBill = this._bill.Id;

                if (calculator.IsValidCalculation())
                    await App.localDatabase.AddCalculator(calculator);
            }
            await App.localDatabase.UpdateCalculatorAsync(this._calculator);
        }
    }
}
