using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using System;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;
using AnorocMobileApp.Helpers;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Essentials;
using SQLite;
using System.Diagnostics;

namespace AnorocMobileApp.Services
{
    public class LocationService : ILocationService
    {
        private bool success;

        public LocationService()
        {
            success = false;
        }
        /// <summary>
        /// Function to post location to server
        /// </summary>
        /// <param name="location">Location Object</param>        
        /// 
        public void Send_Locaiton_ServerAsync(Models.Location location)
        {
            
            PostLocationAsync(location);
            if (!success)
            {
                //throw new CantConnectToLocationServerException();
            }
        }

        public async void DontSentCurrentLocationAnymoreAsync()
        {
            GeolocationRequest request;
            request = new GeolocationRequest(GeolocationAccuracy.Best);
            Xamarin.Essentials.Location location;
            try
            {
                location = await Geolocation.GetLocationAsync(request);
                Models.Location customLocation = new Models.Location(location);
                var conn = new SQLiteAsyncConnection(App.FilePath);


                conn.CreateTableAsync<Models.Location>().Wait();
                await conn.InsertAsync(customLocation).ContinueWith((t) =>
                 {
                     Debug.WriteLine("New ID from t: {0}", t.Id);
                     Debug.WriteLine("New ID: {0} from primitiveRisk", customLocation.LocationId);
                 });
                await conn.CloseAsync();
            }
            catch(Exception e)
            {
                //TODO:
                // retry to get the location
            }
        }
        /// <summary>
        /// Function to send user locaton to server
        /// </summary>
        /// <param name="location">Location Object</param>
        /// 
        protected async void PostLocationAsync(Models.Location location)
        {
            if (App.Current.Properties.ContainsKey("TOKEN"))
            {
                const string url = Secrets.baseEndpoint + Secrets.geolocationEndpoint;

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                try
                {
                    using (HttpClient client = new HttpClient(clientHandler))
                    {

                        client.Timeout = TimeSpan.FromSeconds(30);

                        Token token = new Token();
                        token.access_token = (string)Application.Current.Properties["TOKEN"];


                        token.Object_To_Server = JsonConvert.SerializeObject(location);
                        var data = JsonConvert.SerializeObject(token);

                        var StringConent = new StringContent(data, Encoding.UTF8, "application/json");
                        StringConent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response;

                        try
                        {
                            response = await client.PostAsync(url, StringConent);
                        }
                        catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException || e is HttpRequestException)
                        {
                            throw new CantConnectToLocationServerException();
                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new CantConnectToLocationServerException();
                        }
                        string result = response.Content.ReadAsStringAsync().Result;

                        //await DisplayAlert("Attention", "Enabled: " + result, "OK");
                        success = true;
                    }

                }
                catch (Exception e) when (e is CantConnectToLocationServerException)
                {
                    //TODO:
                    // retry logic for sending to the server
                }
            }
        }


    
    }
}
