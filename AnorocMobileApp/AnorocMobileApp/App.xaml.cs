using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using System;
using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp
{
    public partial class App : Application
    {
        readonly bool mapDebug = false;
        public IFacebookLoginService FacebookLoginService { get; private set; }
 
        public App(IFacebookLoginService facebookLoginService, IBackgroundLocationService backgroundLocationService)
        {
            InitializeComponent();

            // Dependancy Injections:
            Container.BackgroundLocationService = backgroundLocationService;
            Container.LocationService = new LocationService();


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
            LoadPersistentValues();
        }

        protected override void OnSleep()
        {
            Current.Properties["Tracking"] = Container.BackgroundLocationService.isTracking();
        }

        protected override void OnResume()
        {
            LoadPersistentValues();
        }

        private void LoadPersistentValues()
        {
            if(Current.Properties.ContainsKey("Tracking"))
            {
                var value = (bool)Current.Properties["Tracking"];
                
                if(value)
                {
                    Container.BackgroundLocationService.Start_Tracking();
                    
                }
            }

            if (Current.Properties.ContainsKey("CarrierStatus"))
            {
                // Use Carrier status
                User.carrierStatus = Current.Properties["CarrierStatus"].ToString();
            }
        }
    }
}
