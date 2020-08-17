using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Navigation
{
    /// <summary>
    /// Page to show the Me page.
    /// </summary>
    [SQLite.Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MePage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MePage" /> class.
        /// </summary>
        /// 
        private string title = "";
        private string body = "";
        public MePage()
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