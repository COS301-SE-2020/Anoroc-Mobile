using AnorocMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class User
    {
        private SignUpService signUpService = new SignUpService();

        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        public User(string _email, string _username, string _password) 
        {
            this.Email = _email;
            this.UserName = _username;
            this.Password = _password;
        }
        public User()
        {
            this.Email = "";
            this.UserName = "";
            this.Password = "";
        }

        private void EncryptPassword()
        {
            //ecrypt using sha-256?
        }

        public override string ToString()
        {
            return "User Email: " + this.Email + ", User Name: " + this.UserName; 
        }
    }
}
