using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected bool IsPulled;
    }
}
