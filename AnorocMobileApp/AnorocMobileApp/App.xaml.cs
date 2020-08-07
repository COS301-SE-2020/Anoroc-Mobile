using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Forms;
using AnorocMobileApp.Views.Navigation;
using AnorocMobileApp.Helpers;
using AnorocMobileApp.Views.Dashboard;
using AnorocMobileApp.Views.Itinerary;
using Xamarin.Forms;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace AnorocMobileApp
{
    public partial class App
    {
        public const string NotificationReceivedKey = "NotificationRecieved";

        static public int ScreenWidth;
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        private static string syncfusionLicense = Secrets.SyncfusionLicense;
        public IFacebookLoginService FacebookLoginService { get; private set; }


        //-------------------------------------------------------------------------------------------------
        //Container Set up
        public static Container IoCContainer { get; set; }
        //-------------------------------------------------------------------------------------------------

        public App(IFacebookLoginService facebookLoginService)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);

            InitializeComponent();

            //Defualt lifestle
            IoCContainer = new Container();
           /* IoCContainer.Options.DefaultLifestyle = new AsyncScopedLifestyle();*/
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocationService>(Lifestyle.Singleton);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Singleton);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Singleton);

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
                // MainPage = new LoginWithSocialIconPage();
                MainPage = new AddItineraryPage();
            }
        }

        public App()
        {
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocationService>(Lifestyle.Scoped);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Scoped);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Scoped);

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
            Current.Properties["Tracking"] = BackgroundLocationService.Tracking;
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
                BackgroundLocationService.Tracking = value;
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
