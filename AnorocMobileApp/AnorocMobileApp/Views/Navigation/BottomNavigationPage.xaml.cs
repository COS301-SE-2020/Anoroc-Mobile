using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Notification;
using AnorocMobileApp.Services;
using AnorocMobileApp.ViewModels.Notification;
using AnorocMobileApp.Views.Dashboard;
using AnorocMobileApp.Views.Notification;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
        NotificationDB notificationDB = new NotificationDB();
        NotificationModel updateNotification = new NotificationModel();
        public BottomNavigationPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<object, string>(this, App.NotificationTitleReceivedKey, OnTitleMessageReceived);

            MessagingCenter.Subscribe<object, string>(this, App.NotificationBodyReceivedKey, OnBodyMessageReceived);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        void OnTitleMessageReceived(object sender, string msg)
        {
            title = msg;
        }

        void OnBodyMessageReceived(object sender, string msg)
        {
            //body = msg;



            notificationDB.Title = title;
            notificationDB.Body = msg;


            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<NotificationDB>();
                var notificaitons = conn.Table<NotificationDB>().ToList();
                int rowsAdded = conn.Insert(notificationDB);
                //notificaitons = conn.Table<NotificationDB>().ToList();
                conn.Close();
                var newPage = Navigation.NavigationStack.LastOrDefault();
                NotificationPage page = newPage as NotificationPage;
                if (newPage.GetType() == typeof(NotificationPage))
                {
                    NotificationModel tempModel = new NotificationModel();
                    tempModel.Name = notificationDB.Body;
                    tempModel.IsRead = false;
                    tempModel.ReceivedTime = DateTime.Now.ToString();
                    //tempModel.ReceivedTime = await RelativeDate(DateTime.Now, tempModel.ReceivedTime);                        
                    page.notificationViewModel.RecentList.Insert(0, tempModel);
                }
            }


            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                DependencyService.Get<NotificationServices>().CreateNotification(title, msg);
            });

        }

    }
}

