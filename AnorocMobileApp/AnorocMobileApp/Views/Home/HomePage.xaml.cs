using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Home
{
    /// <summary>
    /// Home page for the app to display a summary of information
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage" /> class.
        /// </summary>
        private string title = "";
        private string body = "";
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<object, string>(this, App.NotificationTitleReceivedKey, OnTitleRecieved);
            MessagingCenter.Subscribe<object, string>(this, App.NotificationBodyReceivedKey, OnMessageReceived);

        }

        void OnTitleRecieved(object sender, string msg)
        {
            title = msg;
            SaveMessagetoSqLite(title);
        }

        void OnMessageReceived(object sender, string msg)
        {
            body = msg;
        }

        void SaveMessagetoSqLite(string title)
        {

            NotificationDB notificationDB = new NotificationDB()
            {
                Title = title,
                Body = body
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<NotificationDB>();
                int rowsAdded = conn.Insert(notificationDB);

                var notifications = conn.Table<NotificationDB>().ToList();
            }


        }
    }
}