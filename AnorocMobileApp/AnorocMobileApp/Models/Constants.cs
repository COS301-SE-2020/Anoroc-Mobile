using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class Constants
    {

        /*public static bool IsDev = true;
        public static readonly string GoogleClientID = "googleID on google project .apps.googleusercontent.com";
        public static readonly string GoogleClientSecret = "google secret on google project";
       
        public static readonly string FacebookAppID = "985395151878298";*/
        public static readonly string googleAPIKey = "//KEY HERE//";
        public static readonly string clientID = $"{googleAPIKey}.apps.googleusercontent.com";
        public static readonly string redirectUrl = $"com.googleusercontent.apps.{googleAPIKey}:/oauth2redirect";

         public static bool IsDev = true;
        public static readonly string GoogleClientID = "googleID on google project .apps.googleusercontent.com";
        public static readonly string GoogleClientSecret = "google secret on google project";
       
        public static readonly string FacebookAppID = "985395151878298";

        public static class Adb2C
        {
            public static readonly string TenantName = "anorocmob";
            public static readonly string TenantId = "anorocmob.onmicrosoft.com";
            public static readonly string ClientId = "d09df4e8-8c35-4d67-b446-57098d21ce5d";
            public static readonly string PolicySignin = "B2C_1_signupsignin1";
            public static readonly string PolicyPassword = "B2C_1_passwordreset";
            public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
            public static readonly string[] Scopes = { "" };
            public static string AuthoritySignin => $"{AuthorityBase}{PolicySignin}";
            public static string AuthorityPasswordReset => $"{AuthorityBase}{PolicyPassword}";
        }
    }

}
