using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Interface
{
    public interface IRegisterService
    {
        IRegisterService CreateFirstName(string firstName);
        IRegisterService CreateLastName(string lastName);
        IRegisterService CreateBirthday(DateTime birthday);
        IRegisterService CreateCountry(string country);
        IRegisterService CreateState(string state);
        IRegisterService CreateCity(string city);
        IRegisterService CreatePhone(string phone);
        IRegisterService CreateEmail(string email);
        IRegisterService CreatePassword(string password);

        Task<Models.UserDetails> RegisterAsync();
        Task<Models.UserDetails> Register(string username, string password, string name);
    }
}
