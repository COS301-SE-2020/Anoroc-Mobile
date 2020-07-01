using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Services
{
    /// <summary>
    /// Getter's and Setter's for Google's Authorization class used to authenticate our Google Login
    /// </summary>
    /// <param name="email">The Returned string which is the user's email address</param>
    /// <param name="photo">The user's Google Profile Photo</param>
    /// <param name="name">The User's name from their Google account</param>
    /// <param name="email_verified">A string that indicates whether the user is logged in through Google or not</param>
    class GoogleAuthorization
    {
        public string email { get; set; }
        public string photo { get; set; }
        public string name { get; set; }
        public string email_verified { get; set; }
    }
}
