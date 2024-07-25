using CalculadoraFuncional.ViewModels;

namespace CalculadoraFuncional.Views;

public partial class MonthlyBillPage : ContentPage
{
	public MonthlyBillPage(MonthlyBillViewModel montlhyBillViewModel)
	{
		InitializeComponent();

		BindingContext = montlhyBillViewModel;
	}
}