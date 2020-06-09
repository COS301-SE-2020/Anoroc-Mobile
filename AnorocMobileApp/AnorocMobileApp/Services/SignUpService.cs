using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AnorocMobileApp.Services
{
    //This class interacts with the API server on behalf of the User class
    class SignUpService
    {
        HttpClient http = new HttpClient();
        public SignUpService()
        {

        }

        public bool registerUser(string password)
        {
            return false;
        }
    }
}
