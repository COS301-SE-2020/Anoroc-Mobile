using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Services
{
    public interface IFacebookLoginService
    {
        string AccessToken { get; }
        Action<string, string> AccessTokenChanged { get; set; }
        void Logout();
    }
}
