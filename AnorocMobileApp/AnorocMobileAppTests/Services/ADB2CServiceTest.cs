using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnorocMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using AnorocMobileApp.Interfaces;
using Xamarin.Forms;
using AnorocMobileApp.Models;

namespace AnorocMobileApp.Services.Tests
{
    [TestClass()]
    public class ADB2CServiceTest
    {
        
        [TestMethod()]
        public void CheckIfTenantIsCorrect()
        {
            Assert.AreEqual(B2CConstants.Tenant,"anorocb2c.onmicrosoft.com");
        }
          

        [TestMethod()]
        public void CheckIfAzureADB2CHostnameIsCorrect()
        {
            Assert.AreEqual(B2CConstants.AzureADB2CHostname, "anorocb2c.b2clogin.com");
        }


        [TestMethod()]
        public void CheckIfClientIDCorrect()
        {
            Assert.AreEqual(B2CConstants.ClientID, "841e1d06-26d5-4092-a0b0-c75823ed2671");
        }


        [TestMethod()]
        public void CheckIfPolicySignUpSignInCorrect()
        {
            Assert.AreEqual(B2CConstants.PolicySignUpSignIn, "b2c_1_SignUpAndSignIn");
        }


        [TestMethod()]
        public void CheckIfPolicyResetPasswordCorrect()
        {
            Assert.AreEqual(B2CConstants.PolicyResetPassword, "b2c_1_PasswordReset");
        }
    }
}