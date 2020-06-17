using AnorocMobileApp.Models;
using AnorocMobileApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AnorocMobileApp.Services
{
    //Use this class to log the user in again once our Token.token has expireed and we need a new one
    class LoginService
    {
        public bool LoginAnoroc()
        {
            return true;
        }


        public static async Task fillUserDetails(IFacebookLoginService facebookLoginService)
        {
            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(4000);
                await fillDetailsAsync(facebookLoginService);
            });
        }
        private static async Task fillDetailsAsync(IFacebookLoginService facebookLoginService)
        {
            facebookLoginService.setUserDetails();

            var httpClient = new HttpClient();
           
            string url = $"https://graph.facebook.com/{User.UserID}?fields=email&access_token={facebookLoginService.AccessToken}";

            var json = await httpClient.GetStringAsync(url);
            //_ = (Application.Current as App).MainPage.DisplayAlert("Error", json, "OK");
            UserDetails userDetails = JsonConvert.DeserializeObject<UserDetails>(json);
            if (string.IsNullOrEmpty(userDetails.Email))
                Console.WriteLine("Could not get email.");
            else
                User.Email = userDetails.Email;
        }
    }
}


