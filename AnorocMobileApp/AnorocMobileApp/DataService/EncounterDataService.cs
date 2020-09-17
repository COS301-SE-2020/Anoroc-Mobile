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
                    tempModel.ReceivedTime =n.Time;
                    //obj.RecentList.Add(tempModel);
                    obj.RecentList.Insert(0, tempModel);
                }
                conn.Close();                
            } 

            return obj;
        }               
    }
}