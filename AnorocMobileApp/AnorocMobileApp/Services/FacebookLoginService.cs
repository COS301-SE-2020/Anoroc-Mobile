using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace AnorocMobileApp.Services
{
    public interface IFacebookLoginService
    {

        string FirstName { get; }
        string UserID { get; }
        string LastName { get; }
        string AccessToken { get; }
        Action<string, string> AccessTokenChanged { get; set; }
        void Logout();
    }
}
