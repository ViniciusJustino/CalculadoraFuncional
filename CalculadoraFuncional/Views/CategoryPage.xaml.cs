using CalculadoraFuncional.ViewModels;
using ColorPicker.Maui;

namespace CalculadoraFuncional.Views;

public partial class CategoryPage : ContentPage
{
    private bool _valueChanging = false;
    public CategoryPage(CategoryViewModel categoryViewModel)
	{
		InitializeComponent();
		BindingContext = categoryViewModel;

        _valueChanging = true;
        SelectedColorValueEntry.Text = ColorPicker.PickedColor.ToHex();
        _valueChanging = false;
    }

    private void ColorPicker_PickedColorChanged(object sender, PickedColorChangedEventArgs e)
    {
        if (SelectedColorValueEntry != null && !_valueChanging)
        {
            _valueChanging = true;
            SelectedColorValueEntry.Text = e.NewPickedColorValue.ToHex();
            _valueChanging = false;
        }
    }
}