using CalculadoraFuncional.ViewModels;

namespace CalculadoraFuncional.Views;

public partial class BillPage : ContentPage
{
	public BillPage(BillViewModel billViewModel)
	{
		InitializeComponent();
		BindingContext = billViewModel;
	}
}