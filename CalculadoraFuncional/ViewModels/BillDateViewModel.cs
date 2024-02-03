using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.ViewModels
{
    internal class BillDateViewModel : ContentPage
    {
        private Models.Bill _bill;
        private Models.Calculator _calculator;

        public BillDateViewModel()
        {
            _bill = new Models.Bill();
            _calculator = new Models.Calculator();
        }

    }
}
