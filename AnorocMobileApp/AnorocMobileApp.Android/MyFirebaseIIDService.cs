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
using AnorocMobileApp.Interfaces;
using Firebase.Iid;
using Xamarin.Forms;

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
            Console.WriteLine($"Token wire up: {refreshedToken}");
            MessagingCenter.Send<object, string>(this, AnorocMobileApp.App.FirebaseTokenKey, refreshedToken);
           // SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            IUserManagementService userManagementService = App.IoCContainer.GetInstance<IUserManagementService>();
            userManagementService.SendFireBaseToken(token);
            // TODO: Still need to be implemented
            Log.Debug(PackageName, token);
        }
    }

}