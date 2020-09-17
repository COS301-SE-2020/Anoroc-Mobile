using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AnorocMobileApp.Models;
using Firebase.Messaging;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AnorocMobileApp.Droid
{
    /// <summary>
    /// Class with Firebase Messaging Services. To get messages while application is active
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public static string title = "";
        public static string body = "";

        public override void  OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Console.WriteLine("Received: " + message);
            try
            {

                base.OnMessageReceived(message);

                var title = message.GetNotification().Title;
                var data = message.Data;
                var msg = message.GetNotification().Body;
                Console.WriteLine("Testing Data output: "  + message.Data.Values);

                var location = JsonConvert.DeserializeObject<Models.Location>(data["Location"]);
                var risk = Convert.ToInt32(data["Risk"]);
                var dateTime = DateTime.Parse(data["DateTime"]);

                var body = "High Risk: You have come into contact in " + location.Region.Suburb;


                string[] notificationMessage = { title, body };


                // Passing Message onto xamarin forms
                MessagingCenter.Send<object, string>(this, AnorocMobileApp.App.NotificationTitleReceivedKey, title);
                MessagingCenter.Send<object, string[]>(this, AnorocMobileApp.App.NotificationBodyReceivedKey, notificationMessage);
                //Console.WriteLine("Received Message: " + body);



                //MessagingCenter.Send<object, string[]>(this, AnorocMobileApp.App.NotificationReceivedKey, notificationMessage);
                //SendNotification(body, message.Data);                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errorr extracting message: " + ex);
            }
        }

        private void SendNotification(string messageBody, IDictionary<string, string> data)
        {

            var intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("title", title);
            intent.PutExtra("body", body);

            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            NotificationDB notificationDB = new NotificationDB()
            {
                Title = title,
                Body = body
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {

                conn.CreateTable<NotificationDB>();
                var notifications = conn.Table<NotificationDB>().ToList();

            }
        }   
    }
}