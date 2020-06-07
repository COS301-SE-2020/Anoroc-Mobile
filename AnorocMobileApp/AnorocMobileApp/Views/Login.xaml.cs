using AnorocMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void loginGoogle(object sender, EventArgs e)
        {
            login_heading.Text = "Clicked worked";
            loginSuccessfull();
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

        private void loginFacebook(object sender, EventArgs e)
        {
            
        }

        public static void FacebookSuccess(string title, string msg)
        {
            Application.Current.MainPage = new HomePage();
        }

        private async void btn_signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}