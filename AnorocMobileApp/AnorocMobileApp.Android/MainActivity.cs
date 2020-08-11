using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Facebook;
using Android.Content;
using AnorocMobileApp.Droid.Resources.services;
using Android;
using System.Net;
using Android.Gms.Common;
using Firebase.Iid;
using Firebase.Messaging;
using Android.Util;
using Xamarin.Forms;
using AnorocMobileApp.Services;
using AnorocMobileApp.Interfaces;
using Plugin.CurrentActivity;
using Microsoft.Identity.Client;

namespace AnorocMobileApp.Droid
{
    [Activity(Label = "AnorocMobileApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static ICallbackManager CallbackManager;
        const int RequestLocationId = 0;

       

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            // Set Dependancy
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            DependencyService.Register<IParentWindowLocatorService, AndroidParentWindowLocatorService>();


            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CallbackManager = CallbackManagerFactory.Create();

            base.OnCreate(savedInstanceState);

            Xamarin.FormsMaps.Init(this, savedInstanceState);

            ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            
            //LoadApplication(new App(new FacebookLoginService()));

            IsPlayServicesAvailable();


            // Dependency Injection:

            LoadApplication(new App());

            WireUpBackgroundLocationTask();
        }
        
        //TODO: Add Force Refresh Token
        /// <summary>
        /// Function to check if Google Play services is correctly installed for the firebase messaging
        /// </summary>
        /// <returns>True or False</returns>
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Console.WriteLine($"Error: {GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                }
                else
                {
                    Console.WriteLine("Error: Play services not supported!");
                }
                return false;
            }
            else
            {
                Console.WriteLine("Play services available.");
                return true;
            }
        }

        void WireUpBackgroundLocationTask()
        {
            MessagingCenter.Subscribe<StartBackgroundLocationTracking>(this, "StartBackgroundLocationTracking", message =>
            {
                var intent = new Intent(this, typeof(BackgroundLocationAndroidService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopBackgroundLocationTrackingMessage>(this, "StopBackgroundLocationTrackingMessage", message =>
            {
                var intent = new Intent(this, typeof(BackgroundLocationAndroidService));
                StopService(intent);
            });
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted)) { }
                // Permissions granted - display a message.
                else { }        
             }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
            CallbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode), data);
        }




        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }
    }
}