using System;

namespace AnorocMobileApp.Models
{
    public class Token
    {
        public int TokenID { get; set; }
        public string access_token { get; set; }
        public string error_descriptions { get; set; }
        public DateTime expiry_date { get; set; }

        public string Object_To_Server { get; set; }

        public Token() { }
    }
}
