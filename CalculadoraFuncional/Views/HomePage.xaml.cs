using CalculadoraFuncional.Drawables;
using System.Diagnostics;

namespace CalculadoraFuncional.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        Debug.WriteLine("HomePage Contruído");

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
        graphicView.Invalidate();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Debug.WriteLine("HomePage Desativado");
    }
}