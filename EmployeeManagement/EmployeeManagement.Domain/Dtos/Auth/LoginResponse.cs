﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Domain.Dtos.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string Role { get; set; }
    }
}
