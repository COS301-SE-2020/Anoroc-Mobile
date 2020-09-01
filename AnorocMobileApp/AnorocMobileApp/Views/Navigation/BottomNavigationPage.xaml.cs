using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Notification;
using AnorocMobileApp.Services;
using AnorocMobileApp.ViewModels.Notification;
using AnorocMobileApp.Views.Dashboard;
using AnorocMobileApp.Views.Notification;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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

            MessagingCenter.Subscribe<object, string>(this, App.NotificationTitleReceivedKey, OnTitleMessageReceived);

            MessagingCenter.Subscribe<object, string>(this, App.NotificationBodyReceivedKey, OnBodyMessageReceived);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            autoUpdate();
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
                //notificaitons = conn.Table<NotificationDB>().ToList();
                conn.Close();
                var newPage = Navigation.NavigationStack.LastOrDefault();
                NotificationPage page = newPage as NotificationPage;               
                if( newPage.GetType() == typeof(NotificationPage))
                {
                    NotificationModel tempModel = new NotificationModel();
                    tempModel.Name = notificationDB.Body;
                    tempModel.IsRead = false;
                    tempModel.ReceivedTime = DateTime.Now.ToString();
                    tempModel.ReceivedTime = RelativeDate(DateTime.Now);
                    page.notificationViewModel.RecentList.Insert(0, tempModel);
                }
            }


            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                DependencyService.Get<NotificationServices>().CreateNotification(title, msg);
            });

        }

        public static string RelativeDate(DateTime theDate)
        {            

            Dictionary<long, string> thresholds = new Dictionary<long, string>();
            int minute = 60;
            int hour = 60 * minute;
            int day = 24 * hour;
            thresholds.Add(60, "{0} seconds ago");
            thresholds.Add(minute * 2, "a minute ago");
            thresholds.Add(45 * minute, "{0} minutes ago");
            thresholds.Add(120 * minute, "an hour ago");
            thresholds.Add(day, "{0} hours ago");
            thresholds.Add(day * 2, "yesterday");
            thresholds.Add(day * 30, "{0} days ago");
            thresholds.Add(day * 365, "{0} months ago");
            thresholds.Add(long.MaxValue, "{0} years ago");
            long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
            foreach (long threshold in thresholds.Keys)
            {
                if (since < threshold)
                {
                    TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
                    return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
                }
            }
            return "";
        }

        public void autoUpdate()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<NotificationDB>();
                var notifications = conn.Table<NotificationDB>().ToList();                
                //notificaitons = conn.Table<NotificationDB>().ToList();
                conn.Close();
                var newPage = Navigation.NavigationStack.LastOrDefault();
                //NotificationPage page = newPage as NotificationPage;
                if (newPage is NotificationPage page)
                {
                    page.notificationViewModel.RecentList.Clear();

                    foreach (var n in notifications)
                    {
                        NotificationModel tempModel = new NotificationModel();
                        tempModel.Name = n.Body;
                        tempModel.IsRead = false;
                        tempModel.ReceivedTime = RelativeDate(DateTime.Now);
                        //obj.RecentList.Add(tempModel);
                        page.notificationViewModel.RecentList.Insert(0, tempModel);
                    }
                }
               
            }           
        }
    }
}