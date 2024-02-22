using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Interface
{
    internal interface IRegisterService
    {
        Task<Models.UserDetails> Register(string username, string password, string name);
    }
}
