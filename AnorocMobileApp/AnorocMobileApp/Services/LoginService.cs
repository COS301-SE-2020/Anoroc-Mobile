using AnorocMobileApp.Models;
using AnorocMobileApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public async static Task GetUserEmailAsync(string accessToken)
        {
            var httpClient = new HttpClient();

            string url = $"https://graph.facebook.com/{User.UserID}?fields=email&access_token={accessToken}";

            var json = await httpClient.GetStringAsync(url);

            UserDetails userDetails = JsonConvert.DeserializeObject<UserDetails>(json);
            if (string.IsNullOrEmpty(userDetails.Email))
                _ = (Application.Current as App).MainPage.DisplayAlert("Error", "Could not fetch email address", "OK");
            else
                User.Email = userDetails.Email;
        }
    }
}
