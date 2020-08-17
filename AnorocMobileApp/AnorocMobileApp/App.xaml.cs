using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Forms;
using AnorocMobileApp.Views.Navigation;
using AnorocMobileApp.Helpers;
using Xamarin.Forms;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using AnorocMobileApp;

namespace AnorocMobileApp
{
    public partial class App
    {
        public const string NotificationTitleReceivedKey = "NotificationTitleRecieved";
        public const string NotificationBodyReceivedKey = "NotificationBodyRecieved";
        public const string WholeNotificationReceivedKey = "WholeNotificationRecieved";

        static public int ScreenWidth;
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        private static string syncfusionLicense = Secrets.SyncfusionLicense;
        readonly bool mapDebug = false;
        public IFacebookLoginService FacebookLoginService { get; private set; }


        //-------------------------------------------------------------------------------------------------
        //Container Set up
        public static Container IoCContainer { get; set; }
        //-------------------------------------------------------------------------------------------------


        public static string FilePath;

        public App(IFacebookLoginService facebookLoginService)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
            InitializeComponent();
            Current.Properties["TOKEN"] = "thisisatoken";
            //Defualt lifestle
            IoCContainer = new Container();
            /* IoCContainer.Options.DefaultLifestyle = new AsyncScopedLifestyle();*/
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocaitonService>(Lifestyle.Singleton);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Singleton);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Singleton);

            FacebookLoginService = facebookLoginService;

            

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

                //MainPage = new Views.Navigation.SettingsPage();
                //MainPage = new Views.Map();

                MainPage = new LoginWithSocialIconPage();

            }
        }


        public App(string filePath)
        {
            IoCContainer = new Container();
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocaitonService>(Lifestyle.Singleton);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Singleton);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Singleton);

            //Register Syncfusion license
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
            Current.Properties["TOKEN"] = "thisisatoken";

            MainPage = new LoginWithSocialIconPage();

            FilePath = filePath;
        }



        public App()
        {
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocaitonService>(Lifestyle.Scoped);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Scoped);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Scoped);

            //Register Syncfusion license
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
            Current.Properties["TOKEN"] = "thisisatoken";
            MainPage = new NavigationPage(new BottomNavigationPage());
        }

        /*public App()
        {
            

            MainPage = new NavigationPage(new Login());
        }*/

        protected override void OnStart()
        {
            LoadPersistentValues();
        }

        protected override void OnSleep()
        {
            Current.Properties["Tracking"] = BackgroundLocaitonService.Tracking;
        }

        protected override void OnResume()
        {
            LoadPersistentValues();
        }

        private void LoadPersistentValues()
        {
            if(Current.Properties.ContainsKey("Tracking"))
            {
                IBackgroundLocationService backgroundLocationService = IoCContainer.GetInstance<IBackgroundLocationService>();
                var value = (bool)Current.Properties["Tracking"];
                BackgroundLocaitonService.Tracking = value;
                if(value)
                {
                    backgroundLocationService.Start_Tracking();
                }
            }

            if (Current.Properties.ContainsKey("CarrierStatus"))
            {
                // Use Carrier status
                if (Current.Properties["CarrierStatus"].ToString() == "Positive")
                    User.carrierStatus = true;
                else
                    User.carrierStatus = false;
            }
        }
    }
}
