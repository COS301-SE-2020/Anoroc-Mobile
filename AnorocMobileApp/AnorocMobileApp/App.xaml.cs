using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp
{
    public partial class App : Application
    {
        public IFacebookLoginService FacebookLoginService { get; private set; }

        public App(IFacebookLoginService facebookLoginService)
        {
            InitializeComponent();

            FacebookLoginService = facebookLoginService;
            if(facebookLoginService.isLoggedIn())
            {
                User.UserName = facebookLoginService.FirstName;
                User.UserSurname = facebookLoginService.LastName;
                User.UserID = facebookLoginService.UserID;
                User.loggedInFacebook = true;
                MainPage = new NavigationPage(new HomePage(facebookLoginService));
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
        }

        /*public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
        }*/

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
