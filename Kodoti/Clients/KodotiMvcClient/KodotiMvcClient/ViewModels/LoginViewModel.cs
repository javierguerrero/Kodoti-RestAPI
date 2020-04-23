using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodotiMvcClient.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
