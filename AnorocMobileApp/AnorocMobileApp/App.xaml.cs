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
using AnorocMobileApp;
using System.IO;
using Plugin.SecureStorage;

namespace AnorocMobileApp
{
    public partial class App
    {
        public const string NotificationTitleReceivedKey = "NotificationTitleRecieved";
        public const string NotificationBodyReceivedKey = "NotificationBodyRecieved";


        public const string FirebaseTokenKey = "FirebaseRecieved";



    
        static public int ScreenWidth;
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        private static string syncfusionLicense = Secrets.SyncfusionLicense;
       // public IFacebookLoginService FacebookLoginService { get; private set; }


        //-------------------------------------------------------------------------------------------------
        //Container Set up
        public static Container IoCContainer { get; set; }
        //-------------------------------------------------------------------------------------------------



        public static string FilePath;



        public App(string filePath)
        {
            Application.Current.Properties["RememberMe"] = "false";
            IoCContainer = new Container();
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocationService>(Lifestyle.Singleton);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Singleton);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Singleton);
            IoCContainer.Register<IItineraryService, ItineraryService>(Lifestyle.Singleton);
            //Register Syncfusion license
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
   
            MessagingCenter.Subscribe<object, string>(this, App.FirebaseTokenKey, OnKeyReceived);

            // MainPage = new LoginWithSocialIconPage();
            
            FilePath = filePath;
        }




        public App()
        {
            InitializeComponent();
        }
        void OnKeyReceived(object sender, string key)
        {
            Current.Properties["FirebaseToken"] = key;
            IUserManagementService userManagementService = App.IoCContainer.GetInstance<IUserManagementService>();
            userManagementService.SendFireBaseToken(key);
        }
 

        protected override void OnStart()
        {
            LoadPersistentValues();
            
            var status = CrossSecureStorage.Current.GetValue("SignedIn", null);
            if (status != null && status == "True")
            {
                MainPage = new NavigationPage(new BottomNavigationPage());
                var name = CrossSecureStorage.Current.GetValue("Name");
                var surname = CrossSecureStorage.Current.GetValue("Surname");
                var email = CrossSecureStorage.Current.GetValue("Email");
                IoCContainer.GetInstance<IUserManagementService>().UserLoggedIn(name, surname, email);
            }
            else
            {
                MainPage = new LoginWithSocialIconPage();
            }
        }

        protected override void OnSleep()
        {
            Current.Properties["Tracking"] = BackgroundLocationService.Tracking;
        }

        protected override void OnResume()
        {
            LoadPersistentValues();
            var status = CrossSecureStorage.Current.GetValue("SignedIn", null);
            if (status != null && status == "True")
            {
                MainPage = new NavigationPage(new BottomNavigationPage());
                var name = CrossSecureStorage.Current.GetValue("Name");
                var surname = CrossSecureStorage.Current.GetValue("Surname");
                var email = CrossSecureStorage.Current.GetValue("Email");
                IoCContainer.GetInstance<IUserManagementService>().UserLoggedIn(name, surname, email);
            }
            else
            {
                MainPage = new LoginWithSocialIconPage();
            }
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
                User.carrierStatus = Current.Properties["CarrierStatus"].ToString() == "Positive";
            }
            else
                User.carrierStatus = false;
        }
    }
}
