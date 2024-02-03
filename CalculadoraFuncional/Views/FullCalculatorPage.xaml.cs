using CalculadoraFuncional.Models;
using CalculadoraFuncional.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CalculadoraFuncional.Views;

public partial class FullCalculatorPage : ContentPage
{
	public FullCalculatorPage()
	{
		InitializeComponent();

		CalculatorViewModel calculatorViewModelContext = (CalculatorViewModel) calculatorPage.BindingContext;

		historyCalculatorViewModelContext.currentCalculator = calculatorViewModelContext.calculator;
		historyCalculatorViewModelContext.calculatorPage = calculatorPage;
		calculatorViewModelContext.historyCalculator = historyCalculatorViewModelContext;

		historyCollection.ScrollTo(historyCalculatorViewModelContext.historyCalc.Last());
    }
}