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
    /// <summary>
    /// Use this class to log the user in again once our Token.token has expireed and we need a new one
    /// </summary>
    class LoginService
    {
        public bool LoginAnoroc()
        {
            return true;
        }
        /// <summary>
        /// Retrieve the user's data based on their Facebook login
        /// </summary>
        /// <param name="facebookLoginService">Service used to manage Facebook login Authentication</param>
        /// <returns></returns>
        public static async Task fillUserDetails(IFacebookLoginService facebookLoginService)
        {
            await Task.Factory.StartNew(async () =>
            {
                Thread.Sleep(4000);
                await fillDetailsAsync(facebookLoginService);
            });
        }
        /// <summary>
        /// Asynchronously collect Facebook data in background
        /// </summary>
        /// <param name="facebookLoginService">Service used to manage Facebook login Authentication</param>
        /// <returns>A Task</returns>
        static async Task fillDetailsAsync(IFacebookLoginService facebookLoginService)
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


