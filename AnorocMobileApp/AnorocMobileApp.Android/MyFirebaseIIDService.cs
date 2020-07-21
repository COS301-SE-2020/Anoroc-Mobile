using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;

namespace AnorocMobileApp.Droid
{

    /// <summary>
    /// Class with Firebase Instance Services. For Regristration to Firebase and token retrieval.
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        public override async void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Console.WriteLine($"Token received: {refreshedToken}");
            SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            // TODO: Still need to be implemented
            Log.Debug(PackageName, token);
        }
    }

}