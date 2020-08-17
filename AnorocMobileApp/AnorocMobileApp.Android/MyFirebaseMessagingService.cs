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
using Firebase.Messaging;
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

                var body = message.GetNotification().Body;
                var title = message.GetNotification().Title;
                string[] notificationMessage = { title, body };
                var data = message.GetNotification().ToString();
                var msg = message.GetNotification().Body;
                Console.WriteLine("Testing Data output: "  + message.Data.Values);



                // Passing Message onto xamarin forms
                MessagingCenter.Send<object, string>(this, AnorocMobileApp.App.NotificationTitleReceivedKey, title);
                MessagingCenter.Send<object, string>(this, AnorocMobileApp.App.NotificationBodyReceivedKey, body);
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
        }   
    }
}