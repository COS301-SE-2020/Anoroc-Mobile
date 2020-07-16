using AnorocMobileApp.Interfaces;
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
            var message = new StartBackgroundLocationTracking();
            MessagingCenter.Send(message, "StartLongRunningTaskMessage");
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
                    await Task.Delay(ConvertSec(_Backoff));
                }
            });
        }

        /// <summary>
        /// Used to run the Timer ever _Backoff seconds by converting the seconds to milisecondss
        /// </summary>
        /// <param name="seconds"> The seconds to convert </param>
        /// <returns> The Miliseconds </returns>
        private int ConvertSec(int seconds)
        { 
            return seconds * 1000;
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
            var message = new StopBackgroundLocationTrackingMessage();
            MessagingCenter.Send(message, "StopLongRunningTaskMessage");
        }
    }
}
