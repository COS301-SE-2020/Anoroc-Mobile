using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
        LocationService LocationService;
        private int request_count;

        public static bool Tracking;
        public BackgroundLocaitonService()
        {
            Tracking = false;
            _Backoff = 30;
            Modifier = 2;
            request_count = 0;
            User_Location = new Models.Location();
            LocationService = new LocationService();
        }

        /// <summary>
        /// Start the Background tracking service
        /// </summary>
        public void Start_Tracking()
        {
            Tracking = true;
            Run_Track();
        }
        
        /// <summary>
        /// Calls the Track function based every _Backoff amount of seconds
        /// </summary>
        protected void Run_Track()
        {
            Device.StartTimer(new TimeSpan(ConvertSecToNano(_Backoff)), () =>
            {
                Track();
                return Tracking;
            });
        }

        /// <summary>
        /// Used to run the Timer ever _Backoff seconds by converting the seconds to nanoseconds
        /// </summary>
        /// <param name="seconds"> The seconds to convert </param>
        /// <returns> The Nanoseconds </returns>
        private long ConvertSecToNano(int seconds)
        {
            long ticks = seconds * (1 ^ 9);
            return ticks;
        }

        private int _Backoff;
        private int Modifier;

        /// <summary>
        /// Sends the user's location to the server if the distance is to the last request is larger than 10m, otherwise an exponential backoff occrus
        /// </summary>
        protected async void Track()
        {
           
            request = new GeolocationRequest(GeolocationAccuracy.Best);
            Xamarin.Essentials.Location location = null;

            if (Previous_request != null)
            {
                location = await Geolocation.GetLocationAsync(request);
                if(location.CalculateDistance(Previous_request, DistanceUnits.Kilometers) >= 0.01)
                {
                    _Backoff = 30;
                    Modifier = 2;
                    LocationService.Send_Locaiton_Server(new Models.Location(location));
                }
                else
                {
                    if ((_Backoff / 60) <= 10)
                    {
                        _Backoff += (30 * Modifier);
                        Modifier *= 2;
                    }
                }
            }
            else
            {
                location = await Geolocation.GetLocationAsync(request);
            }
            Previous_request = location;
        }

        /// <summary>
        /// Stop recording the users location in the background
        /// </summary>
        public void Stop_Tracking()
        {
            Tracking = false;
        }
    }
}
