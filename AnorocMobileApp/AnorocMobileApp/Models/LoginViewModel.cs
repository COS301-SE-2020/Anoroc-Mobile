using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;


namespace AnorocMobileApp.Models
{
    public class LoginViewModel
    {
        readonly IFacebookLoginService facebookLoginService;

        public Command FacebookLogoutCmd { get; }
        public ICommand OnFacebookLoginSuccessCmd { get; }
        public ICommand OnFacebookLoginErrorCmd { get; }
        public ICommand OnFacebookLoginCancelCmd { get; }
        public LoginViewModel()
        {
            facebookLoginService = (Application.Current as App).FacebookLoginService;
            facebookLoginService.AccessTokenChanged = (string oldToken, string newToken) => FacebookLogoutCmd.ChangeCanExecute();

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

        void Success(string title, string authToken)
        {
            (Application.Current as App).MainPage.DisplayAlert(title, authToken, "OK");
            Login.FacebookSuccess(title, authToken);
        }

        void DisplayAlert(string title, string msg)
        {
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "OK");
        }
    }
}
