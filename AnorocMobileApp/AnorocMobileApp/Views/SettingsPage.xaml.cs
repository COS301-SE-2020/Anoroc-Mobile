using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace AnorocMobileApp.Views
{ 
    public partial class SettingsPage : ContentPage
    {        
        public SettingsPage()
        {
            InitializeComponent();
        }

        class Location
        {
            public double Latitude = 25.754110;
            public double Longitude = 28.232022;
            public double Altitude = 1300;
        }

        async void OnToggledAsync(object sender, ToggledEventArgs e)
        {
            if(e.Value == true)
            {
                //POST
                try
                {

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
                //await DisplayAlert("Attention", "Disabled", "OK");
            }
        }
        //"{"+$"'Latitude':'{location.Latitude}', 'Longitude Longitude':'{location.Longitude}','Altitude':'{location.Altitude}'"+"}"

        public async void postRequestAsync()
        {
            var location = new Location();
            var json = JsonConvert.SerializeObject(location);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://10.0.2.2:44384/location/GEOLocationAsync";
            //await DisplayAlert("Attention", "Enabled: " + json, "OK");
            //await DisplayAlert("Attention", "Enabled: " + json, "OK");


            //var client = new HttpClient(new System.Net.Http.HttpClientHandler());

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);

            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;
            await DisplayAlert("Attention", "Enabled: " + result, "OK");
        }
    }
}
