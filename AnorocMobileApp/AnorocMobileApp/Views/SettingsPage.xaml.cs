using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Essentials;

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
        public SettingsPage()
        {
            var status= new Label();
            status.SetBinding(Label.TextProperty, new Binding("SelectedItem", source: status));

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Lowest);
            InitializeComponent();
        }
        /*We need to fix this, Only 1 class allowed per file!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
        //public class Location
        //{
        //    public string Latitude;
        //    public string Longitude;
        //    public string Altitude;
        //}
        /// <summary>
        /// Asynchronous function that returns the current User location
        /// </summary>
        /// <returns>Location of the user based on the phones Geolocation</returns>
        public async Task<Models.GEOCoordinate> getLocationAsync()
        {
            //Location loc = new Location();
            Models.GEOCoordinate loc = new Models.GEOCoordinate();
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Lowest);
                Location location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    loc.Latitude = location.Latitude;
                    loc.Longitude = location.Longitude;
                    loc.Altitude = (double)location.Altitude;


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
        async void OnToggledAsync(object sender, ToggledEventArgs e)
        {
            if(e.Value == true)
            {
                try
                {
                    //POST
                    postRequestAsync();

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        await DisplayAlert("Attention", ":( " + ex.InnerException.Message, "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Attention", "Disabled", "OK");
            }
        }
        /// <summary>
        /// Function to get the user's Geolocation when the location permission is enabled 
        /// </summary>
        public async void postRequestAsync()
        {

            Models.GEOCoordinate location = await getLocationAsync();

            string url = "https://10.0.2.2:5001/location/GEOLocation";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient(clientHandler);

            string data  = JsonConvert.SerializeObject(location);
            StringContent c = new StringContent(data, Encoding.UTF8, "application/json");
            c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync(url, c);
            string result = response.Content.ReadAsStringAsync().Result;

            await DisplayAlert("Attention", "Enabled: " + result, "OK");
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                //status.Text = (string)picker.ItemsSource[selectedIndex];
            }
        }
    }
}
