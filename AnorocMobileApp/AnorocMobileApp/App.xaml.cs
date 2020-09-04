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

            MainPage = new LoginWithSocialIconPage();
            
            FilePath = filePath;
        }




        public App()
        {  
            InitializeComponent();
            /*//Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);

          

            DependencyService.Register<B2CAuthenticationService>();

          

            MessagingCenter.Subscribe<object, string>(this, App.FirebaseTokenKey, OnKeyReceived);


            MainPage = new LoginWithSocialIconPage();

            //Defualt lifestle
            IoCContainer = new Container();
            //* IoCContainer.Options.DefaultLifestyle = new AsyncScopedLifestyle();*//*
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocationService>(Lifestyle.Singleton);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Singleton);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Singleton);
            *//*
            // Dependancy Injections:
            IoCContainer.Register<IBackgroundLocationService, BackgroundLocationService>(Lifestyle.Scoped);
            IoCContainer.Register<ILocationService, LocationService>(Lifestyle.Scoped);
            IoCContainer.Register<IUserManagementService, UserManagementService>(Lifestyle.Scoped);
            *//*

            //Register Syncfusion license
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
           
            MainPage = new NavigationPage(new BottomNavigationPage());*/
        }
        void OnKeyReceived(object sender, string key)
        {
            Current.Properties["FirebaseToken"] = key;
           // IoCContainer.GetInstance<IUserManagementService>().SendFireBaseToken(key);
        }
 

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
