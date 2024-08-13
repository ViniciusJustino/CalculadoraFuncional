using Firebase.Auth;
using Firebase.Auth.Repository;
using FirebaseAdmin.Auth;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    public partial class UserDetails
    {
        [Ignore]
        public string Id { get; set; }
        public string LocalId { get; set; }
        public string Name { get; set; }
        [Ignore]
        public string RefreshToken { get; set; }
        [Ignore]
        public string Token { get; set; }
        [Ignore]
        public UserRecord User { get; set; }

        public bool IsNull()
        {
            return string.IsNullOrEmpty(Id);
        }
    }
}
