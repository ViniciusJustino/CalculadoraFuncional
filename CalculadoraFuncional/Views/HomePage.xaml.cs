using CalculadoraFuncional.Drawables;
using CalculadoraFuncional.ViewModels;
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

    /*protected override void OnDisappearing()
    {
		base.OnDisappearing();

        NewDrawable dr =  new NewDrawable();
        graphicView.WidthRequest = dr.Width;
        graphicView.Drawable = dr;
    }*/

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("HomePage Ativo");
        //graphicView.Invalidate();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Debug.WriteLine("HomePage Desativado");
    }
}