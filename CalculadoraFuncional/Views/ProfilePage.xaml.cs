using CalculadoraFuncional.ViewModels;

namespace CalculadoraFuncional.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel profileViewModel)
	{
		InitializeComponent();
		BindingContext = profileViewModel;
	}
}