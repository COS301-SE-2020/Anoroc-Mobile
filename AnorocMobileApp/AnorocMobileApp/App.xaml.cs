using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using AnorocMobileApp.Views.Forms;
using AnorocMobileApp.Views.Navigation;
using System;
using System.Net;
using Microsoft.Identity.Client;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp
{
    public partial class App : Application
    {
        public const string NotificationReceivedKey = "NotificationRecieved";
        
        public static IPublicClientApplication AuthenticationClient { get; private set; }

        public static object UIParent { get; set; } = null;

         
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        private static string syncfusionLicense =
            "";
        readonly bool mapDebug = false;
        public IFacebookLoginService FacebookLoginService { get; private set; }
 
        public App(IFacebookLoginService facebookLoginService, IBackgroundLocationService backgroundLocationService)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);

            InitializeComponent();
            
            AuthenticationClient = PublicClientApplicationBuilder.Create(Constants.Adb2C.ClientId)
                .WithB2CAuthority(Constants.Adb2C.AuthoritySignin)
                .WithRedirectUri($"msal{Constants.Adb2C.ClientId}://auth")
                .Build();
            
            // Dependancy Injections:
            Container.BackgroundLocationService = backgroundLocationService;
            Container.LocationService = new LocationService();
            Container.userManagementService = new UserManagementService();

            FacebookLoginService = facebookLoginService;

            Current.Properties["TOKEN"] = "thisisatoken";

            if (facebookLoginService.isLoggedIn())
            {
                User.FirstName = facebookLoginService.FirstName;
                User.UserSurname = facebookLoginService.LastName;
                User.UserID = facebookLoginService.UserID;
                User.loggedInFacebook = true;
                MainPage = new NavigationPage(new BottomNavigationPage());
                //MainPage = new Views.Map();
            }
            else
            {
                MainPage = new NavigationPage(new BottomNavigationPage());
                //MainPage = new Views.Map();
            }
        }

        public App(IFacebookLoginService facebookLoginService)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
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
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);

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

            if (Current.Properties.ContainsKey("CarrierStatus"))
            {
                // Use Carrier status
                User.carrierStatus = Current.Properties["CarrierStatus"].ToString();
            }
        }
    }
}
