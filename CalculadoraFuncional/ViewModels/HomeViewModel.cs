using CalculadoraFuncional.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace CalculadoraFuncional.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<MonthlyBills> HistoryBills { get; set; }

        public MonthlyBills ItemBillsSelected
        {
            get => _itemSelected;
            set
            {
                if (_itemSelected != value && value != null)
                {
                    SelectItem(value);
                    ItemBillsSelected = null;

                }
            }
        }
        public Drawables.GraphicsHandler GraphicsHandler { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public bool IsRefreshing { get; set; }
        public double MaxValue { get; set; }
        private int MonthSelected { get; set; }
        private int YearSelected { get; set; }
        private MonthlyBills _itemSelected;

        public HomeViewModel() 
        {
            MonthSelected = DateTime.Now.Month;
            YearSelected = DateTime.Now.Year;

            Init();
            RefreshCommand = new AsyncRelayCommand(RefreshListViewAsync);
        }

        private async void Init()
        {
            _ = RefreshHomeAsync();
        }

        private async Task RefreshHomeAsync()
        {
            IEnumerable<Bill> bills = await LoadBillsAsync();
            _ = LoadGraphicsAsync(bills);
            _ = LoadListBillsAsync(bills);
        }

        private async Task RefreshListViewAsync()
        {
            IEnumerable<Bill> bills = await LoadBillsAsync();
            bool RefreshGraphic = !this.HistoryBills.Equals(bills);

            if (IsRefreshing)
            {
                if (RefreshGraphic)
                    _ = LoadGraphicsAsync(bills);

                _ = LoadListBillsAsync(bills);

            }

            IsRefreshing = false;
            OnPropertyChanged(nameof(IsRefreshing));
        }

        private async Task LoadListBillsAsync(IEnumerable<Bill> _bills)
        {
            
            ObservableCollection<BillViewModel> tempHistoryBills = new ObservableCollection<BillViewModel>(_bills.Select(n => new BillViewModel(n)));

            var grouped = tempHistoryBills
                        .GroupBy(b => b.Date.Month)
                        .Select(g => new MonthlyBills { MonthNumber = g.Key, Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key), Bills = g.ToList(), Total = g.Sum(x => x.Total).ToString("C") });


            this.HistoryBills = new ObservableCollection<MonthlyBills>(grouped);
            OnPropertyChanged(nameof(this.HistoryBills));
        }

        private async Task LoadGraphicsAsync(IEnumerable<Bill> _bills)
        {
            IEnumerable<Bill> groupedGraphic = _bills.Where(b => b.Date.Month == MonthSelected && b.Date.Year == YearSelected);
            this.GraphicsHandler = new Drawables.GraphicsHandler(ref groupedGraphic);

            OnPropertyChanged(nameof(this.GraphicsHandler));
        }

        private async ValueTask<IEnumerable<Bill>> LoadBillsAsync()
        {
            var _bills = await Models.Bill.LoadAllAsync();

            return _bills.OrderBy(b => b.Date);
        }

        private void SelectItem(MonthlyBills value)
        {
            _itemSelected = value;

            MonthSelected = _itemSelected.MonthNumber;
            YearSelected = _itemSelected.Year;

            OnPropertyChanged(nameof(this.ItemBillsSelected));

            IEnumerable<Bill> _bills = _itemSelected.Bills.Select(billViewModel => billViewModel.bill);

            MaxValue = _bills.MaxBy(maxValue => maxValue.Value).Value;

            OnPropertyChanged(nameof(this.MaxValue));

            _ = LoadGraphicsAsync(_bills);
        }
    }
}
