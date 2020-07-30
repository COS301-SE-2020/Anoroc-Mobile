using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using System;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;
using AnorocMobileApp.Helpers;
using Newtonsoft.Json;
using Xamarin.Forms;

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
        public void Send_Locaiton_ServerAsync(Location location)
        {
            
            PostLocationAsync(location);
            if (!success)
            {
                //throw new CantConnectToLocationServerException();
            }
        }
        /// <summary>
        /// Function to send user locaton to server
        /// </summary>
        /// <param name="location">Location Object</param>
        /// 
        protected async void PostLocationAsync(Location location)
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
            catch(Exception e) when (e is CantConnectToLocationServerException)
            {
               //TODO:
               // retry logic for sending to the server
            }
        }


    
    }
}
