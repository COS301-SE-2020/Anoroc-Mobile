using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Json;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Notification;
using AnorocMobileApp.ViewModels.Notification;
using SQLite;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.DataService
{
    /// <summary>
    /// The Encounter Data Service
    /// </summary>
    //[Preserve(AllMembers = true)]
    public class EncounterDataService
    {
        #region fields

        private static EncounterDataService _instance;

        private NotificationViewModel notificationViewModel;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets an instance of <see cref="EncounterDataService"/>
        /// </summary>
        //public static EncounterDataService Instance => _instance ?? (_instance = new EncounterDataService());

        public NotificationViewModel NotificationViewModel =>
            this.notificationViewModel ??
            (this.notificationViewModel = PopulateData<NotificationViewModel>("notification.json"));

        #endregion
        
        #region Methods

        /// <summary>
        /// Populates the data for view model from json file.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="fileName">Json file to fetch data.</param>
        /// <returns>Returns the view model object.</returns>
        private static NotificationViewModel PopulateData<T>(string fileName)
        {
            var file = "AnorocMobileApp.Data." + fileName;

            var assembly = typeof(App).GetTypeInfo().Assembly;

            NotificationViewModel obj;
            
            using (var stream = assembly.GetManifestResourceStream(file))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                obj = (NotificationViewModel)serializer.ReadObject(stream);                
            }
           
            return loadNotifications(obj);
        }

        #endregion

        private static NotificationViewModel loadNotifications(NotificationViewModel obj)
        {
            //NotificationViewModel newObj = new NotificationViewModel();

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<NotificationDB>();
                var notifications = conn.Table<NotificationDB>().ToList();
                obj.RecentList.Clear();
                foreach (var n in notifications)
                {
                    NotificationModel tempModel = new NotificationModel();
                    tempModel.Name = n.Body;
                    tempModel.IsRead = false;
                    tempModel.ReceivedTime = RelativeDate(DateTime.Now);
                    //obj.RecentList.Add(tempModel);
                    obj.RecentList.Insert(0, tempModel);
                }
                conn.Close();                
            } 

            return obj;
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


    }
}