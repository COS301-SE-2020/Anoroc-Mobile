using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Forms;
using AnorocMobileApp.Views.Navigation;
using Xamarin.Forms;
using SimpleInjector;


namespace AnorocMobileApp
{
    public partial class App : Application
    {
        public const string NotificationReceivedKey = "NotificationRecieved";

        static public int ScreenWidth;
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        private static string syncfusionLicense = "";
        readonly bool mapDebug = false;
        public IFacebookLoginService FacebookLoginService { get; private set; }


        //-------------------------------------------------------------------------------------------------
        //Container Set up
        public static SimpleInjector.Container IoCContainer { get; set; } = new SimpleInjector.Container();
        //-------------------------------------------------------------------------------------------------

        public App(IFacebookLoginService facebookLoginService)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);

            InitializeComponent();

            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocaitonService>(Lifestyle.Scoped);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Scoped);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Scoped);

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

                //MainPage = new NavigationPage(new BottomNavigationPage());
                //MainPage = new Views.Map();

                MainPage = new LoginWithSocialIconPage();

            }
        }

        public App()
        {
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocaitonService>(Lifestyle.Scoped);
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
