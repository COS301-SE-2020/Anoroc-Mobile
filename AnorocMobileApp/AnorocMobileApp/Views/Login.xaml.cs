using AnorocMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using AnorocMobileApp.Services;

namespace AnorocMobileApp.Views
{
    /// <summary>
    /// Login class used to manage all forms of Login
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        //IFacebookLoginService facebookLoginService;
        public Login()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Use of the OAuth2Authenticator to validate Google Login
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Event Arguments at the instance the function is called</param>
        void loginGoogle(object sender, EventArgs e)
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


        /// <summary>
        /// A function used to control and help with Authentication Problems and/or erros
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Authentication Errors that clearly indicate errors if they occur</param>
        public void onAuthError(object sender, AuthenticatorErrorEventArgs e)
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

        /// <summary>
        /// Function used to manage and control the user's data once successfully logged in
        /// The User's data is retrived from the AuthenticatorCompletedEventArgs object
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="obj">An object of the user's profile data once completed with the Authorization</param>
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
                GoogleAuthorization googleObject = JsonConvert.DeserializeObject<GoogleAuthorization>(json);
                loginSuccessfull();
            }
            else
            {
                await DisplayAlert("Testing", obj.IsAuthenticated.ToString(), "OK");
            }
        }

   
        /*static void FacebookLoggedInAlready(IFacebookLoginService facebookLoginService)
        {
            Application.Current.MainPage = new HomePage(facebookLoginService);
        }*/
        /// <summary>
        /// Handler function that navigates to a new page once a log in is successful with Google
        /// </summary>
        void loginSuccessfull()
        {
            /*var authenticator = OAuth2Authenticator
                {
                    Constants.GoogleClientID,
                    "email profile",
                    new System.Uri("https://accounts.google.com/o/oauth2/auth"),
                    new System.Uri("https://www.google.com")
                }*/

            //Application.Current.MainPage = new HomePage();
            //Application.Current.MainPage = new SettingsPage();
        }

        /// <summary>
        /// Handler function that navigates to a new page once successfully logged in through Facebook
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="facebookLoginService"></param>
        public static void FacebookSuccess(string title, string msg, IFacebookLoginService facebookLoginService)
        { 
            Application.Current.MainPage = new HomePage_old(facebookLoginService);
        }
        /// <summary>
        /// Function called once the Sign In button is selectedin the app which opens the Sign Up page
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Event Arguments</param>
        async void btn_signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }
        /// <summary>
        /// Function called once the Notifications button is clicked which navigates to the Notifications page
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Event Arguments</param>
        async void btn_notification_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NotificationPage());
        }
    }
}