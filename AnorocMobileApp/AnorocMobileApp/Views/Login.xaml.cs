using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AnorocMobileApp.Services;
using System.Net.Http;
using System.Text;
using AnorocMobileApp.Models;
using Newtonsoft.Json;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        IFacebookLoginService facebookLoginService;
        public Login()
        {
            InitializeComponent();
        }

        private void loginGoogle(object sender, EventArgs e)
        {
            login_heading.Text = "Clicked worked";
            loginSuccessfull();
        }

        public static void FacebookLoggedInAlready(IFacebookLoginService facebookLoginService)
        {
            Application.Current.MainPage = new HomePage(facebookLoginService);
        }

        private void loginSuccessfull()
        {
            /*var authenticator = OAuth2Authenticator
                {
                    Constants.GoogleClientID,
                    "email profile",
                    new System.Uri("https://accounts.google.com/o/oauth2/auth"),
                    new System.Uri("https://www.google.com")
                }*/

            Application.Current.MainPage = new HomePage();
        }

        /*private void loginFacebook(object sender, EventArgs e)
        {

            OAuth2Authenticator auth = new OAuth2Authenticator(
                    clientId: Constants.FacebookAppID,
                    scope: "",
                    authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                    redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")
                );

            auth.Completed += Facebook_Auth_Completed;

            //AuthenticationState.Authenticator = auth;
      
        }

        private void Facebook_Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if(e.IsAuthenticated)
            {
                var token = new User()
                {
                    AccessToken = e.Account.Properties["access_token"]
                };
            }
            else
            {

            }
        }*/

        public static void FacebookSuccess(string title, string msg, IFacebookLoginService facebookLoginService)
        { 
            Application.Current.MainPage = new HomePage(facebookLoginService);
        }

        private async void btn_signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}