using System;
using System.Linq;
using AnorocMobileApp.Models.Itinerary;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace AnorocMobileApp.Models
{
    public class Location
    {
        public Location()
        {

        }

        public Location(Position position)
        {
            Latitude = position.Lat;
            Longitude = position.Lon;

            GetRegion();

        }
        public async void GetRegion()
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(Latitude, Longitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                var geocodeAddress =
                    $"AdminArea:       {placemark.AdminArea}\n" +
                    $"CountryCode:     {placemark.CountryCode}\n" +
                    $"CountryName:     {placemark.CountryName}\n" +
                    $"FeatureName:     {placemark.FeatureName}\n" +
                    $"Locality:        {placemark.Locality}\n" +
                    $"PostalCode:      {placemark.PostalCode}\n" +
                    $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                    $"SubLocality:     {placemark.SubLocality}\n" +
                    $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                    $"Thoroughfare:    {placemark.Thoroughfare}\n";
            }
            Region = new Area(placemark.CountryName, placemark.AdminArea, placemark.Locality);
        }
        public Location(Xamarin.Essentials.Location loc)
        {
            this.Created = DateTime.Now;
            Carrier_Data_Point = false;

            Latitude = loc.Latitude;
            Longitude = loc.Longitude;
           
        }

        public Location(double lat, double longC, DateTime dateTime, bool carrier)
        {

            Latitude = lat;
            Longitude = longC;
            Created = dateTime;
            Carrier_Data_Point = carrier;
        }
   
        public DateTime Created { get; set; }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public Area Region { get; set; }

        public Location clone()
        {
            return new Location(Latitude, Longitude, Created, Carrier_Data_Point);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
