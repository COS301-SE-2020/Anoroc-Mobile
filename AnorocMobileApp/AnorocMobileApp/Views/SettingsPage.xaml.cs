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
        
        async Task OnToggledAsync(object sender, ToggledEventArgs e)
        {
            if(e.Value == true)
            {
                //POST
                var location = new Location();
                var json = JsonConvert.SerializeObject(location);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = "https://localhost/5000/location/GEOLocationAsync";
                using var client = new HttpClient();
                var response = await client.PostAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                await DisplayAlert("Attention", "Enabled: " + result, "OK");
            }
            else
            {
                await DisplayAlert("Attention", "Disabled", "OK");
            }
        }
    }
}
