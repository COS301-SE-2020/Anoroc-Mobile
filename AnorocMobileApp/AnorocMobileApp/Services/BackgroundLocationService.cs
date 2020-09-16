using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AnorocMobileApp.Services
{
    public class BackgroundLocationService : IBackgroundLocationService
    {
        GeolocationRequest request;
        Xamarin.Essentials.Location Previous_request;
        ILocationService LocationService;
        public static bool Tracking;
        protected static DateTime LastSent;
        protected readonly double MetersConsidedUserMoved;
        public BackgroundLocationService()
        {
            LocationService = App.IoCContainer.GetInstance<ILocationService>();
            Tracking = false;
            Initial_Backoff = 15;
            _Backoff = Initial_Backoff;
            Modifier = 1.6;
            Track_Retry = 0;
            MetersConsidedUserMoved = 0.011;
        }

        /// <summary>
        /// Start the Background tracking service
        /// </summary>
        ///

        public void Start_Tracking()
        {
            Tracking = true;
            var message = new StartBackgroundLocationTracking();
            Application.Current.Properties["Tracking"] = true;
            MessagingCenter.Send(message, "StartBackgroundLocationTracking");
            HandleCancel();
        }

        /// <summary>
        /// Calls the Track function based every _Backoff amount of seconds
        /// </summary>
        public async void Run_TrackAsync()
        {
            await Task.Run(async () =>
              {
                  while (Tracking)
                  {
                      Track();
                      //temp
                      Debug.WriteLine("Backoff: " + _Backoff + "; Retry: " + Track_Retry);

                      await Task.Delay(ConvertSec(_Backoff));
                  }
              }, CancellationToken.None);
        }

        /// <summary>
        /// Used to run the Timer ever _Backoff seconds by converting the seconds to milisecondss
        /// </summary>
        /// <param name="seconds"> The seconds to convert </param>
        /// <returns> The Miliseconds </returns>
        private int ConvertSec(double seconds)
        {
            return (int)(seconds * 1000);
        }

        private double _Backoff;
        private double Modifier;
        private double Initial_Backoff;
        private double Track_Retry;
        /// <summary>
        /// Sends the user's location to the server if the distance is to the last request is larger than 10m, otherwise an exponential backoff occrus
        /// Backoff = (previous backoff ^ 2)
        /// </summary>
        protected async void Track()
        {

            bool success = false;
            int retry = 0;
            retry = 0;
            try
            {
                // TODO:
                // faster they go, the more locations sent and slower they go the less
                request = new GeolocationRequest(GeolocationAccuracy.Best);
                Xamarin.Essentials.Location location = null;

                if (Previous_request != null)
                {
                    location = await Geolocation.GetLocationAsync(request);
                    Models.Location customLocation = new Models.Location(location);
                    await customLocation.GetRegion();

                    if (location.CalculateDistance(Previous_request, DistanceUnits.Kilometers) >= MetersConsidedUserMoved)
                    {
                        _Backoff = Initial_Backoff;
                        Track_Retry = 0;
                        SendUserLocation(customLocation);
                    }
                    else
                    {
                        
                        if ((_Backoff / 60) <= 10)
                        {
                            _Backoff = _Backoff + Math.Pow(Modifier, Track_Retry);
                            Track_Retry++;
                        }
                    }
                }
                else
                {
                    location = await Geolocation.GetLocationAsync(request);
                    Models.Location customLocation = new Models.Location(location);
                    await customLocation.GetRegion();

                    SendUserLocation(customLocation);
                }
                Previous_request = location;
                success = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                //retry for getting the geolocation
                retry++;
            }
        }

        protected bool TestIfCanSendLocation()
        {
            DateTime currentTime = DateTime.Now;
            if((currentTime-LastSent).TotalMinutes <= 5)
            {

            }
            return false;
        }

        protected void SendUserLocation(Models.Location customLocation)
        {
            customLocation.Carrier_Data_Point = User.carrierStatus;
            LocationService.Send_Locaiton_ServerAsync(customLocation);
            LastSent = DateTime.Now;
        }

        void HandleCancel()
        {
            MessagingCenter.Subscribe<CancelMessage>(this, "CancelMessage", message =>
            {
                Stop_Tracking();
            });
        }

        /// <summary>
        /// Stop recording the users location in the background
        /// </summary>
        public void Stop_Tracking()
        {
            Tracking = false;
            Debug.WriteLine("Stopped tracking - " + _Backoff);
            Application.Current.Properties["Tracking"] = false;
            var message = new StopBackgroundLocationTrackingMessage();
            MessagingCenter.Send(message, "StopBackgroundLocationTrackingMessage");
            _Backoff = Initial_Backoff;
            Track_Retry = 0;
        }

        public bool isTracking()
        {
            return Tracking;
        }

        void SetBackground(double level, bool charging)
        {
            Color? colour = null;
            var status = charging ? "Charging" : "Not charging";

            if (level > .5f)
                colour = Color.Green.MultiplyAlpha(level);
            else if (level > .1f)
                colour = Color.Yellow.MultiplyAlpha(level);
            else
                colour = Color.Red.MultiplyAlpha(level);

            Console.WriteLine("Colour: " + colour.Value);

        }
    }
}
