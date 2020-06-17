using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp
{
    public partial class App : Application
    {
        readonly bool mapDebug = false;
        public IFacebookLoginService FacebookLoginService { get; private set; }

        public App(IFacebookLoginService facebookLoginService)
        {
            InitializeComponent();

                FacebookLoginService = facebookLoginService;
                if (facebookLoginService.isLoggedIn())
                {
                    User.FirstName = facebookLoginService.FirstName;
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

        public App()
        {
            MainPage = new NavigationPage(new Login());
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
