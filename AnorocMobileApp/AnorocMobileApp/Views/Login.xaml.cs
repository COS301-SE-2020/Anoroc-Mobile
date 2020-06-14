using AnorocMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Newtonsoft.Json.Linq;

using AnorocMobileApp.Services;
using System.Net.Http;
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

        private async void loginGoogle(object sender, EventArgs e)
        {
            var authenticator = new OAuth2Authenticator(
                clientId: Constants.clientID,
                clientSecret: null,
                scope: "email profile",
                authorizeUrl: new System.Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                redirectUrl: new Uri(Constants.redirectUrl),
                accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                getUsernameAsync: null,
                isUsingNativeUI: true
                );
            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += onAuthError;

            AuthenticationState.Authenticator = authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }


        public async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs obj)
        {
            var authenticator = sender as OAuth2Authenticator;
            if(authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= onAuthError;
            }

            if (obj.IsAuthenticated)
            {
                //await DisplayAlert("Testing", obj.Account.Properties["access_token"], "OK");
                var clientData = new HttpClient();
                var resData = await clientData.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo?access_token=" + obj.Account.Properties["access_token"]);
                var json = await resData.Content.ReadAsStringAsync();
                //await DisplayAlert("Testing", json, "OK");
                var myJObject = JObject.Parse(json);
                await DisplayAlert("Welcome", (myJObject.SelectToken("name").Value<string>()+"\n"+myJObject.SelectToken("email").Value<string>()), "OK");
                GoogleAuthClass googleObject = JsonConvert.DeserializeObject<GoogleAuthClass>(json);
                loginSuccessfull();
            }
            else
            {
                await DisplayAlert("Testing", obj.IsAuthenticated.ToString(), "OK");
            }
        }

        public void onAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            DisplayAlert("Google Auth Error", e.Message, "OK");
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
        private async void btn_notification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }
    }
}

public class GoogleAuthClass
{
    public string email { get; set; }
    public string photo { get; set; }
    public string name { get; set; }
    public string email_verified { get; set; }
}