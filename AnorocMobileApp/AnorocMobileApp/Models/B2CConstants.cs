using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AnorocMobileApp.Models
{
    public static class B2CConstants
    {
        // Azure AD B2C Coordinates
        public static string Tenant = "anorocb2c.onmicrosoft.com";
        public static string AzureADB2CHostname = "anorocb2c.b2clogin.com";
        public static string ClientID = "841e1d06-26d5-4092-a0b0-c75823ed2671";
        public static string PolicySignUpSignIn = "b2c_1_SignUpAndSignIn";
        public static string PolicyResetPassword = "b2c_1_PasswordReset";
        public static string Instance = "https://login.microsoftonline.com/{0}";
        public static string TenantId = "f5f6b221-9383-4138-9841-789904213bd8";
        public static string ClientId = "841e1d06-26d5-4092-a0b0-c75823ed2671";
        public static string ClientSecret = "SP_f6~Ny_s81.8v4ZfFO5Xfv9~0.jT05tT";
        public static string BaseAddress = "https://localhost:5001/Notification/test";
        public static string ResourceId = "https://anorocb2c.onmicrosoft.com/dbaaace6-f645-4e57-9f08-8f809470175c";

        public static string[] Scopes = { "https://anorocb2c.onmicrosoft.com/cc5bb58e-b34f-41db-80d4-1d8b33d40038/read" };

        public static string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
        public static string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";
        public static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";
        public static string IOSKeyChainGroup = "com.microsoft.adalcache";

        public static string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture,
                                     Instance, TenantId);
            }
        }
    }
}
