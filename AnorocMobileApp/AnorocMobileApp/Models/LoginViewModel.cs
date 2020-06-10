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
                (authToken) => Success("Success", authToken));

            OnFacebookLoginErrorCmd = new Command<string>(
                (err) => DisplayAlert("Error", $"Authentication failed: { err }"));

            OnFacebookLoginCancelCmd = new Command(
                () => DisplayAlert("Cancel", "Authentication cancelled by the user."));
        }

        public async void Success(string title, string authToken)
        {
            User.loggedInFacebook = true;
            await LoginService.GetUserEmailAsync(authToken);
            Login.FacebookSuccess(title, authToken, facebookLoginService);
        }

        public void DisplayAlert(string title, string msg)
        {
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "OK");
        }
    }
}
