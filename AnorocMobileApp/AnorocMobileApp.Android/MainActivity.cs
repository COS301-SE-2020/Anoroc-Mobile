using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
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
using Xamarin.Essentials;
using System.IO;
using Plugin.CurrentActivity;
using Microsoft.Identity.Client;
using SQLite;
using System.Threading.Tasks;

namespace AnorocMobileApp.Droid
{
    [Activity(Label = "Anoroc", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // public static ICallbackManager CallbackManager;
        const int RequestLocationId = 0;

        internal static MainActivity Instance { get; private set; }

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

            //  For killed app state
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        if (key == "title")
                        {
                            var value = Intent.Extras.GetString(key);
                            Preferences.Set("title", value);
                            
                        }
                        else if (key == "body")
                        {
                            var value = Intent.Extras.GetString(key);
                            Preferences.Set("body", value);
                        }

                    }
                }
            }

            // CallbackManager = CallbackManagerFactory.Create();

            base.OnCreate(savedInstanceState);

            //Added battery feature
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);            
            //end of added battery feature

            Xamarin.FormsMaps.Init(this, savedInstanceState);

            ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();
            
            IsPlayServicesAvailable();


            // Dependency Injection:

            string fileName = "notification_db.db3";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string completePath = Path.Combine(folderPath, fileName);            
            LoadApplication(new App(completePath));

            Instance = this;

            WireUpBackgroundLocationTask();
            //WireUpBackgroundUsermanagementTask();

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

        protected override void OnNewIntent(Intent intent)
        {

            if (intent != null)
            {
                var title = intent.GetStringExtra("title");
                var body = intent.GetStringExtra("body");

                Preferences.Set("title", title);
                Preferences.Set("body", body);
            }
            
        }

        void WireUpBackgroundUsermanagementTask()
        {
            MessagingCenter.Subscribe<UserLoggedIn>(this, "UserLoggedIn", message =>
            {
                var intent = new Intent(this, typeof(BackgroundUserManagementService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopBackgroundUserManagementService>(this, "StopBackgroundUserManagementService", message =>
            {
                var intent = new Intent(this, typeof(BackgroundUserManagementService));
                StopService(intent);
            });
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

        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
            // CallbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode), data);
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