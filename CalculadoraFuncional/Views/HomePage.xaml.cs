using CalculadoraFuncional.Drawables;

namespace CalculadoraFuncional.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();

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
        graphicView.Invalidate();
    }
}