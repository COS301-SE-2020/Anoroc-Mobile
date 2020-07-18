using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Essentials;
using AnorocMobileApp.Services;
using AnorocMobileApp.Interfaces;
using System.Diagnostics;

namespace AnorocMobileApp.Views
{ 
    /// <summary>
    /// Class to manage the Settings Paged
    /// </summary>
    public partial class SettingsPage : ContentPage
    {
        /// <summary>
        /// Initializes the settings Screen
        /// </summary>
        /// 
        BackgroundLocaitonService locaitonService;
        public SettingsPage()
        {
            locaitonService = new BackgroundLocaitonService();
            var request = new GeolocationRequest(GeolocationAccuracy.Lowest);
            InitializeComponent();
        }
        /*We need to fix this, Only 1 class allowed per file!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
        public class Location
        {
            public Location()
            {
                
            }
            public string Latitude;
            public string Longitude;
            public string Altitude;
        }
        /// <summary>
        /// Asynchronous function that returns the current User location
        /// </summary>
        /// <returns>Location of the user based on the phones Geolocation</returns>
        public async Task<Location> getLocationAsync()
        {
            Location loc = new Location();
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Lowest);
                var location = await Geolocation.GetLocationAsync(request);
                
                if (location != null)
                {
                    loc.Latitude = location.Latitude.ToString();
                    loc.Longitude = location.Longitude.ToString();
                    loc.Altitude = location.Altitude.ToString();
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
                return loc;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                return null;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                return null;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                return null;
            }

        }




        /// <summary>
        /// Function to toggle Asynchronous location, when off
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Toggled Event Arguments</param>
        /// 
        async void OnToggledAsync(object sender, ToggledEventArgs e)
        {
            if(e.Value == true)
            {
                
                BackgroundLocaitonService.Tracking = true;
                Container.BackgroundLocationService.Start_Tracking();
                try
                {
                    //POST
                    postRequestAsync();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        await DisplayAlert("Attention", ":( " + ex.InnerException.Message, "OK");
                    }
                }
            }
            else
            {
                BackgroundLocaitonService.Tracking = false;
                locaitonService.Stop_Tracking();
                await DisplayAlert("Attention", "Disabled", "OK");
            }
        }
        /// <summary>
        /// Function to get the user's Geolocation when the location permission is enabled 
        /// </summary>
        public async void postRequestAsync()
        {

            var location = await getLocationAsync();

            var url = "https://10.0.2.2:5001/location/GEOLocation";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient client = new HttpClient(clientHandler))
            {

                client.Timeout = TimeSpan.FromSeconds(30);

                var data = JsonConvert.SerializeObject(location);
                var c = new StringContent(data, Encoding.UTF8, "application/json");
                c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response;

                try
                {
                    response = await client.PostAsync(url, c);
                }
                catch(Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnectToLocationServerException();
                }

                if(!response.IsSuccessStatusCode)
                {
                    throw new CantConnectToLocationServerException();
                }
                string result = response.Content.ReadAsStringAsync().Result;

                await DisplayAlert("Attention", "Enabled: " + result, "OK");
            }
        }
    }
}
