using CalculadoraFuncional.Models;
using CalculadoraFuncional.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    public partial class MonthlyBillViewModel : BaseViewModel, IQueryAttributable
    {
        public ObservableCollection<BillViewModel> MonthlyAllBill { get; set; }
        [ObservableProperty]
        private BillViewModel _billViewModel;
        [ObservableProperty]
        private bool _isRefreshing;
        private MonthlyBills monthlyBills;
        public ICommand RefreshCommand { get; private set; }
        public ICommand DeleteBillCommand { get; private set; }
        public ICommand GoToBillCommand { get; private set; }

        public MonthlyBillViewModel()
        {
            RefreshCommand = new AsyncRelayCommand(RefreshListViewAsync);
            DeleteBillCommand = new AsyncRelayCommand<string>(DeleteBillAsync);
            GoToBillCommand = new AsyncRelayCommand<BillViewModel>(GoToBillAsync);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("MonthlyBill"))
            { 
                var _monthlyBills = query["MonthlyBill"] as MonthlyBills;

                if (_monthlyBills != null)
                    SetMonthlyAllBill(_monthlyBills);
            }
            /*if (query.ContainsKey("load"))
            {
                _note = Models.Note.Load(query["load"].ToString());
                RefreshProperties();
            }*/
        }

        private void SetMonthlyAllBill(MonthlyBills monthlyBills)
        {
            MonthlyAllBill = new ObservableCollection<BillViewModel>(monthlyBills.Bills);
            OnPropertyChanged(nameof(MonthlyAllBill));
        }

        private async Task RefreshListViewAsync()
        {
            if (this.IsRefreshing)
            {


            }

            this.IsRefreshing = false;
        }

        private async Task DeleteBillAsync(string IdBill)
        {
            
            BillViewModel _delete = MonthlyAllBill.Single(b => b.IdBill == IdBill);

            bool isDeleted = MonthlyAllBill.Remove(_delete);

            if(isDeleted)
            {
                await _delete.DeleteBillAsync();
                OnPropertyChanged(nameof(MonthlyAllBill));
            }

            SemanticScreenReader.Announce("Compra excluída com sucesso!");
        }

        private async Task GoToBillAsync(BillViewModel billViewModel)
        {
            Dictionary<string, object> navigationData = new Dictionary<string, object>
            {
                { "Bill", billViewModel.bill }
            };

            await Shell.Current.GoToAsync(nameof(BillPage), true, navigationData);
        }
    }
}
