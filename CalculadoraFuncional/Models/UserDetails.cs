﻿using Firebase.Auth;
using Firebase.Auth.Repository;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraFuncional.Models
{
    public partial class UserDetails
    {
       
        public string Id { get; set; }
        public string Name { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public UserRecord User { get; set; }

        public bool IsNull()
        {
            return string.IsNullOrEmpty(Id);
        }
    }
}
