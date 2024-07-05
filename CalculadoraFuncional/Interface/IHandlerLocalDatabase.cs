using CalculadoraFuncional.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Interface
{
    public interface IHandlerLocalDatabase
    {
        ValueTask<IEnumerable<Bill>> GetAllBills();
    }
}
