using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodotiMvcClient.Proxies.Models.Auth
{
    public class LoginAuthModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
