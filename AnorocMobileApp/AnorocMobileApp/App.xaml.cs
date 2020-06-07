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
