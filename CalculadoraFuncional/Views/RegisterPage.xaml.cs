using CalculadoraFuncional.ViewModels;

namespace CalculadoraFuncional.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel registerViewModel)
	{
		InitializeComponent();

        BindingContext = registerViewModel;
    }
}