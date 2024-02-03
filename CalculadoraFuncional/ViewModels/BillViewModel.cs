using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.ViewModels
{
    internal class BillViewModel
    {
        private Models.Bill bill;
        public double Total;
        public string NameCategory { get; private set; }
        public DateTime Date {  get; private set; }
        public string Name { get; private set; }
        public BillViewModel() { }
        public BillViewModel(Models.Bill _bill)
        {
            bill = _bill;
            NameCategory = _bill.Category.NameCategory;
            Total = _bill.Value;
            Date = _bill.Date;
            Name = _bill.Name;
        }
    }
}
