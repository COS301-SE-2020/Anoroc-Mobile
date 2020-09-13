using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task GetRegion()
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(Latitude, Longitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark.SubLocality != null)
                Region = new Area(placemark.CountryCode, placemark.AdminArea, placemark.Locality, placemark.SubLocality);
            else
                Region = new Area(placemark.CountryCode, placemark.AdminArea, placemark.Locality, placemark.SubAdminArea);

        }
        public Location(Xamarin.Essentials.Location loc)
        {
            this.Created = DateTime.Now;
            Carrier_Data_Point = User.carrierStatus;
            Latitude = loc.Latitude;
            Longitude = loc.Longitude;
            GetRegion();
        }

        public Location(double lat, double longC, DateTime dateTime, bool carrier)
        {

            Latitude = lat;
            Longitude = longC;
            Created = dateTime;
            Carrier_Data_Point = carrier;
            GetRegion();
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
