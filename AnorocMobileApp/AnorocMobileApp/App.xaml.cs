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
using SQLite;
using AnorocMobileApp.Views.Notification;
using System;
using AnorocMobileApp.Models.Notification;
using Plugin.SecureStorage;
using AnorocMobileApp.DataService;
using AnorocMobileApp.ViewModels.Notification;
using System.Reflection;

namespace AnorocMobileApp
{
    public partial class App
    {
        public const string NotificationTitleReceivedKey = "NotificationTitleRecieved";
        public const string NotificationBodyReceivedKey = "NotificationBodyRecieved";
        public const string NotificationOnMePageKey = "MePageRecieved";
        public const string NotificationTimeReceivedKey = "NotificationTimeRecieved";


        public const string FirebaseTokenKey = "FirebaseRecieved";



    
        static public int ScreenWidth;
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        private static string syncfusionLicense = Secrets.SyncfusionLicense;
        NotificationDB notificationDB = new NotificationDB();

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
            MessagingCenter.Subscribe<object, string[]>(this, App.NotificationBodyReceivedKey, (object sender, string[] msg) =>
            {

                NotificationDB notificationDB = new NotificationDB();
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<NotificationServices>().CreateNotification(msg[0], msg[1]);
                    notificationDB.Title = msg[0];
                    notificationDB.Body = msg[1];


                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.CreateTable<NotificationDB>();
                        var notificaitons = conn.Table<NotificationDB>().ToList();
                        int rowsAdded = conn.Insert(notificationDB);
                        //notificaitons = conn.Table<NotificationDB>().ToList();
                        conn.Close();
                                            
                    }
                });
            });
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
            var data = new EncounterDataService();
            this.BindingContext = data.NotificationViewModel;
            var status = CrossSecureStorage.Current.GetValue("SignedIn", null);
            if (status != null && status == "True")
            {
                MainPage = new NavigationPage(new BottomNavigationPage());
                var name = CrossSecureStorage.Current.GetValue("Name");
                var surname = CrossSecureStorage.Current.GetValue("Surname");
                var email = CrossSecureStorage.Current.GetValue("Email");
                MessagingCenter.Subscribe<UserLoggedIn>(this, "UserLoggedIn", async message =>
                {
                    var userManagementServiceNotification = IoCContainer.GetInstance<IUserManagementService>();
                    var serverNotifi = await userManagementServiceNotification.GetNotifications();
                    
                    //var serverNotiList = userManagementServiceNotification;
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.DropTable<NotificationDB>();
                        conn.CreateTable<NotificationDB>();
                        var notifications = conn.Table<NotificationDB>().ToList();
                                             
                        foreach(var n in serverNotifi)
                        {
                            NotificationDB passingNotification = new NotificationDB();
                            passingNotification.Body = n.Body;                            
                            passingNotification.Time = n.Time;

                            conn.Insert(passingNotification);
                        }
                        conn.Close();
                    }

                });                
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

        void OnBodyMessageReceived(object sender, string msg)
        {
            //body = msg;


            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<NotificationDB>();
                var notificaitons = conn.Table<NotificationDB>().ToList();
                int rowsAdded = conn.Insert(notificationDB);
                //notificaitons = conn.Table<NotificationDB>().ToList();
                conn.Close();
               
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
