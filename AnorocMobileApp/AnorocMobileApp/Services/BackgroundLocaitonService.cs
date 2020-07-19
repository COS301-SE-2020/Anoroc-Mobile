using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Views;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AnorocMobileApp.Services
{
    public class BackgroundLocaitonService : IBackgroundLocationService
    {

        Models.Location User_Location;
        GeolocationRequest request;
        Xamarin.Essentials.Location Previous_request;
        ILocationService LocationService;
        private int request_count;

        public static bool Tracking;

        public BackgroundLocaitonService()
        {
            Tracking = false;
            Initial_Backoff = 15;
            _Backoff = Initial_Backoff;
            Modifier = 1.6;
            request_count = 0;
            User_Location = new Models.Location();
            
            Track_Retry = 0;
        }

        /// <summary>
        /// Start the Background tracking service
        /// </summary>
        ///

        public void Start_Tracking()
        {
            Tracking = true;
            var message = new StartBackgroundLocationTracking();
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
              },CancellationToken.None);
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
            LocationService = new LocationService();
            bool success = false;
            int retry = 0;
            while (retry < 3 && !success)
            {
                retry = 0;
                try
                {
                    request = new GeolocationRequest(GeolocationAccuracy.Best);
                    Xamarin.Essentials.Location location = null;

                    if (Previous_request != null)
                    {
                        location = await Geolocation.GetLocationAsync(request);
                        if (location.CalculateDistance(Previous_request, DistanceUnits.Kilometers) >= 0.005)
                        {
                            _Backoff = Initial_Backoff;
                            Track_Retry = 0;
                            LocationService.Send_Locaiton_ServerAsync(new Models.Location(location));
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
                        LocationService.Send_Locaiton_ServerAsync(new Models.Location(location));
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
            if(retry == 3 || !success)
            {
                Stop_Tracking();
                // TODO:
                // Failed to Track, need to make a handler - copuld be a manual retry button that starts the tracking again
            }
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
            var message = new StopBackgroundLocationTrackingMessage();
            MessagingCenter.Send(message, "StopBackgroundLocationTrackingMessage");
        }

        public bool isTracking()
        {
            return Tracking;
        }
    }
}
