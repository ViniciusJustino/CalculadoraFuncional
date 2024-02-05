using Android.Views;
using CalculadoraFuncional.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalculadoraFuncional.ViewModels
{
    internal class MonthlyBills
    {
        public string Month {  get; set; }
        public double Total { get; set; }
        public List<BillViewModel> Bills { get; set; }
    }

    internal class AllBillsViewModel
    {
        public ObservableCollection<MonthlyBills> historyBills { get; set; }
        public Drawables.GraphicsHandler graphicsHandler { get; private set; }
        public AllBillsViewModel()
        {
            IEnumerable<Bill> bills = Models.Bill.LoadAll().OrderBy(b => b.Date);

            ObservableCollection<BillViewModel>  tempHistoryBills = new ObservableCollection<BillViewModel>(bills.Select(n => new BillViewModel(n)) );
            this.graphicsHandler = new Drawables.GraphicsHandler(ref bills);

            var grouped = tempHistoryBills
                        .GroupBy(b => b.Date.Month)
                        .Select(g => new MonthlyBills { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName( g.Key), Bills = g.ToList() , Total = g.Sum(x => x.Total)});

            historyBills = new ObservableCollection<MonthlyBills>(grouped);

        }

    }
}
