using CalculadoraFuncional.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Interface
{
    public interface IHandlerDatabase
    {
        ValueTask<IHandlerDatabase> CreateIntanceWithCredentialAsync(UserDetails _user);
        ValueTask<IEnumerable<Bill>> GetAllBills(UserDetails _user);
    }
}
