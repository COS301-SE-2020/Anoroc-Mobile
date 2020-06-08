using AnorocMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class User
    {
        private SignUpService signUpService = new SignUpService();

        public static string Email { get; set; }
        public static string UserName { get; set; }
        public string Password { get; set; }

        public static string UserID { get; set; }
        public static string AccessToken { get; set; }
        public static string UserSurname { get; set; }

        public static bool loggedInFacebook { get; set; }
        public static bool loggedInGoogle { get; set; }
        public static bool loggedInAnoroc { get; set; }

        public User()
        {
          
        }

        private void EncryptPassword()
        {
            //ecrypt using sha-256?
        }

    }
}
