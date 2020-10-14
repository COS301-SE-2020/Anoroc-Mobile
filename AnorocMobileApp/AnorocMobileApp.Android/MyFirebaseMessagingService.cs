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
        public static string time = "";

        public override void  OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Console.WriteLine("Received: " + message);
            try
            {

                base.OnMessageReceived(message);
                try
                {
                    var data = message.Data;
                    var location = JsonConvert.DeserializeObject<Models.Location>(data["Location"]);
                    var risk = Convert.ToInt32(data["Risk"]);
                    var dT = data["DateTime"];
                    var title = message.GetNotification().Title;


                    var outRisk = "";
                    switch(risk)
                    {
                        case 4:
                            outRisk = "HIGH RISK";
                            break;
                        case 3:
                            outRisk = "MEDIUM_RISK";
                            break;
                        case 2:
                            outRisk = "MODERATE_RISK";
                            break;
                        case 1:
                            outRisk = "LOW_RISK";
                            break;
                        case 0:
                            outRisk = "NO_RISK";
                            break;
                    }

                    var body = outRisk + ": You have come into contact in " + location.Region.Suburb + " at " + dT.ToString();
                    string[] notificationMessage = { title, body };
                    Console.WriteLine("Testing Data output: " + message.Data.Values);



                    // Passing Message onto xamarin forms
                    MessagingCenter.Send<object, string>(this, AnorocMobileApp.App.NotificationTitleReceivedKey, title);
                    MessagingCenter.Send<object, string[]>(this, AnorocMobileApp.App.NotificationBodyReceivedKey, notificationMessage);
                }
                catch(Exception e)
                {
                    var body = message.GetNotification().Body;
                    var title = message.GetNotification().Title;
                    var msg = message.GetNotification().Body;
                     string[] notificationMessage = { title, body };
                    Console.WriteLine("Testing Data output: " + message.Data.Values);



                    // Passing Message onto xamarin forms
                    MessagingCenter.Send<object, string>(this, AnorocMobileApp.App.NotificationTitleReceivedKey, title);
                    MessagingCenter.Send<object, string[]>(this, AnorocMobileApp.App.NotificationBodyReceivedKey, notificationMessage);
                    MessagingCenter.Send<object, string[]>(this, AnorocMobileApp.App.NotificationOnMePageKey, notificationMessage);
                }     
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errorr extracting message: " + ex);
            }
        }
    }
}