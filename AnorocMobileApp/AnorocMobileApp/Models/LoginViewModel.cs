using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;



namespace AnorocMobileApp.Models
{
    public class LoginViewModel
    {
        public UserDetails userDetails = null;
        public readonly IFacebookLoginService facebookLoginService;

        public Command FacebookLogoutCmd { get; }
        public ICommand OnFacebookLoginSuccessCmd { get; }
        public ICommand OnFacebookLoginErrorCmd { get; }
        public ICommand OnFacebookLoginCancelCmd { get; }
        public LoginViewModel()
        {
            facebookLoginService = (Application.Current as App).FacebookLoginService;
            facebookLoginService.AccessTokenChanged = (string oldToken, string newToken) => FacebookLogoutCmd.ChangeCanExecute();

            //Test if the user has logged in already
            if(facebookLoginService.isLoggedIn())
            {
                Login.FacebookLoggedInAlready(facebookLoginService);
            }


            FacebookLogoutCmd = new Command(() =>
                facebookLoginService.Logout(),
                () => !string.IsNullOrEmpty(facebookLoginService.AccessToken));

            OnFacebookLoginSuccessCmd = new Command<string>(
                (authToken) => Success("Success", $"Authentication succeed: { authToken }"));

            OnFacebookLoginErrorCmd = new Command<string>(
                (err) => DisplayAlert("Error", $"Authentication failed: { err }"));

            OnFacebookLoginCancelCmd = new Command(
                () => DisplayAlert("Cancel", "Authentication cancelled by the user."));
        }

        public void Success(string title, string authToken)
        {
            User.loggedInFacebook = true;
            Login.FacebookSuccess(title, authToken, facebookLoginService);
        }

        public async void GetUserDetailsAsync(string accessToken)
        {
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(
                $"https://graph.facebook.com/me?fields=email&name&access_token={accessToken}");

            userDetails = JsonConvert.DeserializeObject<UserDetails>(json);
            await (Application.Current as App).MainPage.DisplayAlert("User details", userDetails.ToString(), "OK");
        }

        public void DisplayAlert(string title, string msg)
        {
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "OK");
        }
    }
}
