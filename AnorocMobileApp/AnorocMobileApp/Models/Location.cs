using System;

namespace AnorocMobileApp.Models
{
    public class Location
    {
        public Location()
        {

        }
        public Location(Xamarin.Essentials.Location loc)
        {
            this.Created = DateTime.Now;
            Carrier_Data_Point = false;
            Coordinate = new GEOCoordinate(loc.Latitude, loc.Longitude, loc.Altitude.GetValueOrDefault(), loc.Speed.GetValueOrDefault());
            Coordinate.HorizontalAccuracy = 1;
            Coordinate.VerticalAccuracy = 1;
            Coordinate.Course = 1;
        }

        public Location(GEOCoordinate coord, DateTime dateTime, bool carrier)
        {
            Coordinate = new GEOCoordinate(coord.Latitude, coord.Longitude);
            Created = dateTime;
            Carrier_Data_Point = carrier;
        }
   
        public DateTime Created { get; set; }
        public GEOCoordinate Coordinate { get; set; }
        public bool Carrier_Data_Point { get; set; }


        public Location clone()
        {
            return new Location(Coordinate, Created, Carrier_Data_Point);
        }
    }
}
