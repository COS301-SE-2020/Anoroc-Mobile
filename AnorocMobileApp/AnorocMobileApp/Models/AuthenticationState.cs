using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;

namespace AnorocMobileApp.Models
{
    public static class AuthenticationState
    {
        public static OAuth2Authenticator Authenticator { get; set; }
    }
}
