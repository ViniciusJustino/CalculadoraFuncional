using System.Diagnostics;

namespace CalculadoraFuncional.Views;

public partial class NewCalculatorPage : ContentPage
{
	public NewCalculatorPage()
	{
		InitializeComponent();
        Debug.WriteLine("NewCalculatorPage Construído");

        entryField.Behaviors.Add(newCalculatorViewModelContext);

#if ANDROID || IOS
        entryField.IsReadOnly = true;
#endif
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("NewCalculatorPage Ativo");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Debug.WriteLine("NewCalculatorPage Desativado");
    }
}