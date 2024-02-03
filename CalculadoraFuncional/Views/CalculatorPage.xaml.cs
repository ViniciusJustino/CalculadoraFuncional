using CalculadoraFuncional.ViewModels;
using System.Diagnostics;

namespace CalculadoraFuncional.Views;

public partial class CalculatorPage : ContentView
{
	public CalculatorPage()
	{
		InitializeComponent();

		entryField.Behaviors.Add(calculatorViewModelContext);

#if ANDROID || IOS
		entryField.IsReadOnly = true;
#endif
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		Debug.WriteLine("+ Clicked");
    }
}