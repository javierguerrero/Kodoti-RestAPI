using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodotiMvcClient.Proxies.Models.Auth
{
    public class AccessTokenAuthModel
    {
        public string access_token { get; set; }
        public string user_email { get; set; }
        public string user_id { get; set; }
    }
}
