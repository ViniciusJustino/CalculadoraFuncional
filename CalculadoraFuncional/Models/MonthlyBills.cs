using CalculadoraFuncional.ViewModels;


namespace CalculadoraFuncional.Models
{
    internal class MonthlyBills
    {
        public List<BillViewModel> _bills;
        public string Month { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
        public string Total { get; set; }
        public List<BillViewModel> Bills
        {
            get => _bills;
            set
            {
                if (_bills != value && value != null)
                {
                    _bills = value;
                    Year = _bills[0].Date.Year;
                }
            }
        }
    }
}
