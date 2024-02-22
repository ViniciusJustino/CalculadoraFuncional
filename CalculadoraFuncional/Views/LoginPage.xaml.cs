using CalculadoraFuncional.ViewModels;

namespace CalculadoraFuncional.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();

		BindingContext = loginViewModel;
	}
}