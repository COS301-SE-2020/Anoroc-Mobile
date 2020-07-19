using System.Threading;

namespace AnorocMobileApp.Models
{
    public class GEOCoordinate
    {
        public GEOCoordinate() { }
        public GEOCoordinate(double lat, double lon, double alt, double spe) 
        {
            Latitude = lat;
            Longitude = lon;
            Altitude = alt;
            Speed = spe.ToString();
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string HorizontalAccuracy { get; set; }
        public string VerticalAccuracy { get; set; }
        public string Speed { get; set; }
        public string Course { get; set; }
        public bool IsUnknown { get; set; }
        public double Altitude { get; set; }
    }
}