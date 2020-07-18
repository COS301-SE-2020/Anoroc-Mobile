using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

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
        }
   
        public DateTime Created { get; set; }
        public GEOCoordinate Coordinate { get; set; }
        public bool Carrier_Data_Point { get; set; }

    }
}
