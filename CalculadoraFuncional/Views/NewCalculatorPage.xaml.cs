namespace CalculadoraFuncional.Views;

public partial class NewCalculatorPage : ContentPage
{
	public NewCalculatorPage()
	{
		InitializeComponent();

        entryField.Behaviors.Add(newCalculatorViewModelContext);

#if ANDROID || IOS
        entryField.IsReadOnly = true;
#endif
    }
}