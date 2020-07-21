using System.Threading;

namespace AnorocMobileApp.Models
{
    public class GEOCoordinate
    {
        public GEOCoordinate() {
            HorizontalAccuracy = 1;
            VerticalAccuracy = 1;
            Course = 1;
        }

        public GEOCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = 0;
            HorizontalAccuracy = 1;
            VerticalAccuracy = 1;
            Course = 1;
            Speed = 1;
        }

        public GEOCoordinate(double lat, double lon, double alt, double spe) 
        {
            Latitude = lat;
            Longitude = lon;
            Altitude = alt;
            Speed = spe;
            HorizontalAccuracy = 1;
            VerticalAccuracy = 1;
            Course = 1;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double HorizontalAccuracy { get; set; }
        public double VerticalAccuracy { get; set; }
        public double Speed { get; set; }
        public double Course { get; set; }
        public bool IsUnknown { get; set; }
        public double Altitude { get; set; }
    }
}