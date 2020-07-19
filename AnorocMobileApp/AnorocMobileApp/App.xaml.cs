using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using AnorocMobileApp.Views.Forms;
using AnorocMobileApp.Views.Navigation;
using System;
using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
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
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");
            InitializeComponent();

                FacebookLoginService = facebookLoginService;
                if (facebookLoginService.isLoggedIn())
                {
                    User.FirstName = facebookLoginService.FirstName;
                    User.UserSurname = facebookLoginService.LastName;
                    User.UserID = facebookLoginService.UserID;
                    User.loggedInFacebook = true;
                    MainPage = new NavigationPage(new BottomNavigationPage());
                }
                else
                {
                    MainPage = new NavigationPage(new BottomNavigationPage());
                }
        }

        public App()
        {
            MainPage = new NavigationPage(new BottomNavigationPage());
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
        }

    }
}
