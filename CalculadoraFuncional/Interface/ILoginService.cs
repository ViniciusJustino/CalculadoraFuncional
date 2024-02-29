using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Interface
{
    public interface ILoginService
    {
        Task<Models.UserDetails> Login(string username, string password);
        Task<Models.UserDetails> LoginWithGoogle();
    }
}
