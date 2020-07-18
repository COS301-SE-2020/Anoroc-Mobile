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
using Xamarin.Forms;
using AnorocMobileApp.Services;
using AnorocMobileApp.Interfaces;

namespace AnorocMobileApp.Droid
{
    [Activity(Label = "AnorocMobileApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static ICallbackManager CallbackManager;
        const int RequestLocationId = 0;

        public static IBackgroundLocationService BackgroundLocationService;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Set Dependancy
            BackgroundLocationService = new BackgroundLocaitonService();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CallbackManager = CallbackManagerFactory.Create();

            base.OnCreate(savedInstanceState);

            Xamarin.FormsMaps.Init(this, savedInstanceState);

            ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            
            // Dependency Injection:

            LoadApplication(new App(new FacebookLoginService(), BackgroundLocationService));

            WireUpBackgroundLocationTask();
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