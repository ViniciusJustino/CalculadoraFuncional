using CalculadoraFuncional.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Type;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    public partial class BillViewModel : ObservableObject, IQueryAttributable
    {
        public Models.Bill bill;
        
        public string CurrentTotal => _total.ToString("C");
        public string ResumeDate  => Date.ToShortDateString();
        public string IdBill => bill.Id.ToString();

        [ObservableProperty]
        private double _total;

        [ObservableProperty]
        private string _nameCategory;

        [ObservableProperty]
        private System.DateTime _date;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private Calculator _calculator;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _color;
        public ICommand RefreshCommand { get; private set; }
        public ICommand DeleteCalculatorCommand { get; private set; }
        public ObservableCollection<Calculator> ListCalculation {  get; set; }

        public BillViewModel() { InitCommands(); }
        public BillViewModel(Models.Bill _bill)
        {
            SetBill(_bill);
            InitCommands();
        }

        public async Task DeleteBillAsync()
        {
            await App.localDatabase.DeleteBillAsync(bill);
        }

        private void InitCommands()
        {
            RefreshCommand = new AsyncRelayCommand(RefreshListCalculation);
            DeleteCalculatorCommand = new AsyncRelayCommand<Calculator>(DeleteCalculatorAsync);
        }

        private async Task DeleteCalculatorAsync(Calculator calculator)
        {
            bool isDeleted = ListCalculation.Remove(calculator);

            if (isDeleted) 
            { 
                await App.localDatabase.DeleteCalculatorAsync(calculator);
                OnPropertyChanged(nameof(ListCalculation));
            }
        }

        private async Task RefreshListCalculation()
        {

            ListCalculation = new ObservableCollection<Calculator>(await App.localDatabase.GetAllCalculatorOfBillAsync(this.bill));
            OnPropertyChanged(nameof(ListCalculation));

            IsRefreshing = false;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            if (query.ContainsKey("Bill"))
            {
                var _bill = query["Bill"] as Bill;

                if (_bill != null)
                {
                    SetBill(_bill);
                    _ = SetCalculators(_bill);
                }
            }
        }

        private void SetBill(Bill _bill)
        {
            bill = _bill;
            //NameCategory = _bill?.Category?.NameCategory;
            Total = _bill.Value;
            Date = _bill.Date;
            Name = _bill.Name;
        }

        private async Task SetCalculators(Bill _bill)
        {
            IsRefreshing = true;

            ListCalculation = new ObservableCollection<Calculator>(await App.localDatabase.GetAllCalculatorOfBillAsync(_bill));
            OnPropertyChanged(nameof(ListCalculation));

            IsRefreshing = false;
        }

    }
}
