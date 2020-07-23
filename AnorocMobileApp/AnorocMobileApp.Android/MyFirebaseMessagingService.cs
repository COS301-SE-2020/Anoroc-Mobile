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

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Console.WriteLine("Received: " + message);
            try
            {
                var msg = message.GetNotification().Body;
                Console.WriteLine("Testing Data output: "  + message.Data.Values);


                // Passing Message onto xamarin forms
                MessagingCenter.Send<object, string>(this, AnorocMobileApp.App.NotificationReceivedKey, msg);
                Console.WriteLine("Received Message: " + msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errorr extracting message: " + ex);
            }
        }

    }
}