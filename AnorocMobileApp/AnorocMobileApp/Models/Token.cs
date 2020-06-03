using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class Token
    {
        public int TokenID { get; set; }
        public string access_token { get; set; }
        public string error_descriptions { get; set; }
        public DateTime expiry_date { get; set; }

        public Token() { }
    }
}
