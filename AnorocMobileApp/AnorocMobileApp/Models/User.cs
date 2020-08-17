using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using Newtonsoft.Json;
using System;

namespace AnorocMobileApp.Models
{
    public class User
    {
        private SignUpService signUpService = new SignUpService();

        public static string Email { get; set; }
        public static string FirstName { get; set; }
        public string Password { get; set; }

        public static string UserID { get; set; }
        public static string AccessToken { get; set; }
        public static string UserSurname { get; set; }

        public static bool loggedInFacebook { get; set; }
        public static bool loggedInGoogle { get; set; }
        public static bool loggedInAnoroc { get; set; }
        public static bool carrierStatus { get; set; }

        public User()
        { 
        }

        public async void registerAsync()
        {
            bool registered = await signUpService.registerUserAsync(EncryptPassword());
            if(registered)
            {
                SignupPage.registerSuccessfull();
            }
        }

        private string EncryptPassword()
        {
            // TODO:
            //ecrypt using sha-256?
            return "";
        }

        internal static string toString()
        {
            return JsonConvert.SerializeObject(new UserWrapper(Email, FirstName, UserSurname, loggedInAnoroc, loggedInFacebook, loggedInGoogle, carrierStatus));
        }

        /*public static async Task<string> GetUserEmail()
        {
            return "";
        }*/

    }
}
