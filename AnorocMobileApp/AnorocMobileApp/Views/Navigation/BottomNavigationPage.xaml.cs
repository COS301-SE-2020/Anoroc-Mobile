using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Navigation
{
    //[Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomNavigationPage : TabbedPage
    {

        private string title; 
        public BottomNavigationPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            
            MessagingCenter.Subscribe<object, string>(this, App.NotificationTitleReceivedKey, OnTitleMessageReceived);

            MessagingCenter.Subscribe<object, string>(this, App.NotificationBodyReceivedKey, OnBodyMessageReceived);

            /*  if(title != null && body != null)
              {
                  SaveMessageToSQLite(title, body);
              }
              if (title != null && body != null)
              {

              }*/



        }


        void OnTitleMessageReceived(object sender, string msg)
        {
            title = msg;
        }

        void OnBodyMessageReceived(object sender, string msg)
        {
            //body = msg;

            NotificationDB notificationDB = new NotificationDB()
            {
                Title = title,
                Body = msg
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<NotificationDB>();
                var notificaitons = conn.Table<NotificationDB>().ToList();
                int rowsAdded = conn.Insert(notificationDB);
            }


            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                DependencyService.Get<NotificationServices>().CreateNotification(title, msg);
            });

        }
    }
}