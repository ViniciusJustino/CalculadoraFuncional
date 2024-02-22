using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Interface
{
    internal interface ILoginService
    {
        Task<Models.UserDetails> Login(string username, string password);
    }
}
