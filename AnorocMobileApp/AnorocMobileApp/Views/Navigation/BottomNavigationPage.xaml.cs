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
                if( newPage.GetType() == typeof(NotificationPage))
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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            if (value == null) return string.Empty;

            var current_day = DateTime.Today;
            var postedData = (DateTime)value;

            var ts = new TimeSpan(DateTime.Now.Ticks - postedData.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
            {
                if (ts.Seconds < 0)
                {
                    return "sometime ago";
                }
                return ts.Seconds == 1 ? "One second ago" : ts.Seconds + " seconds ago";
            }

            if (delta < 2 * MINUTE)
                return "A minute ago";

            if (delta < 45 * MINUTE)
            {
                if (ts.Seconds < 0)
                {
                    return "sometime ago";
                }
                return ts.Minutes + " minutes ago";
            }

            if (delta <= 90 * MINUTE)
                return "An hour ago";

            if (delta < 24 * HOUR)
            {
                if (ts.Hours < 0)
                {
                    return "sometime ago";
                }

                if (ts.Hours == 1)
                    return "1 hour ago";

                return ts.Hours + " hours ago";
            }

            if (delta < 48 * HOUR)
                return $"Yesterday at {postedData.ToString("t")}";

            if (delta < 30 * DAY)
            {
                if (ts.Days == 1)
                    return "1 day ago";

                return ts.Days + " days ago";
            }


            if (delta < 12 * MONTH)
            {
                int months = (int)(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = (int)(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
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
                        //tempModel.ReceivedTime = RelativeDate(DateTime.Now);
                        //obj.RecentList.Add(tempModel);
                        page.notificationViewModel.RecentList.Insert(0, tempModel);
                    }
                }

            }
        }
    }
}

