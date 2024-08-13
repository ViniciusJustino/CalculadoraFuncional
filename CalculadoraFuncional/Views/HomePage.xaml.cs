using CalculadoraFuncional.Drawables;
using CalculadoraFuncional.ViewModels;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace CalculadoraFuncional.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel homeViewModel)
	{
		InitializeComponent();
        Debug.WriteLine("HomePage Contruído");

        BindingContext = homeViewModel;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("HomePage Ativo");

        _= ((HomeViewModel)BindingContext).RefreshListViewAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Debug.WriteLine("HomePage Desativado");
    }
}