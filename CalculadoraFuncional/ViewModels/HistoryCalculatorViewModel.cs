using CalculadoraFuncional.Models;
using CalculadoraFuncional.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    //Criar um command no momento em que os objetos estiverem renderizados e enviar o bindingcontext do full e o calcultor como parametro e fazer a inscrição por fora
    internal class HistoryCalculatorViewModel : IObserver<object>
    {
        public ObservableCollection<Calculator> historyCalc { get; set; }

        private Calculator _currentCalculator;

        private readonly Dictionary<string, ICommand> functionsCommand;

        public CalculatorPage calculatorPage { get; set; }

        public Calculator currentCalculator 
        {
            get => _currentCalculator;
            set
            {
                if (_currentCalculator != value) 
                {
                    _currentCalculator = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ICommand SelectCalculationCommand { get; }
        
        public virtual void Subscribe(IObservable<object> provider)
        {
            provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
            Debug.WriteLine("Calculation Completed");
        }

        public virtual void OnError(Exception error)
        {
            Debug.WriteLine("Calculation Error");
        }

        public virtual void OnNext(object value)
        {
            Calculator newCalculator = (Calculator)value;

            if (!historyCalc.Contains(newCalculator))
                historyCalc.Add(newCalculator);
        }

        public HistoryCalculatorViewModel()
        {
            historyCalc = new ObservableCollection<Calculator>();
            
            //SelectCalculationCommand = new AsyncRelayCommand<ViewModels.CalculatorViewModel>(SelectCalculationClickAsync);

            foreach (Calculator calc in historyCalc)
            {
                Subscribe(calc);
            }
        }

        public void KeyPressed(string key)
        {
            if(functionsCommand.ContainsKey(key))
            {
                functionsCommand[key].Execute(null);
            }
        }

        public void NewCalculator(ref Calculator oldCalculator, ref Calculator newCalculator)
        {
            if(historyCalc.Contains(newCalculator))
                historyCalc.Add(newCalculator);

            if(oldCalculator.CalculationCompleted)
            {
                newCalculator.Operator1 = oldCalculator.Result;
                newCalculator.Operation = oldCalculator.NextOperation;
                newCalculator.Expression = oldCalculator.Result?.OperatorValue + oldCalculator.NextOperation?.OperationValue;
            }

            Subscribe(newCalculator);
            _currentCalculator = newCalculator;
        }

        private void OnPropertyChanged()
        {
            if (!historyCalc.Contains(_currentCalculator))
            {
                Subscribe(_currentCalculator);
                historyCalc.Add(_currentCalculator);
            }
        }
    }
}
