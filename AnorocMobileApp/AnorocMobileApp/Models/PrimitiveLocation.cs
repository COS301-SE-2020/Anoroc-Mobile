using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class PrimitiveLocation
    {
        public PrimitiveLocation()
        {

        }
        public PrimitiveLocation(Location customLocation)
        {
            Created = customLocation.Created;
            Carrier_Data_Point = customLocation.Carrier_Data_Point;
            Latitude = customLocation.Latitude;
            Longitude = customLocation.Longitude;
            Region = JsonConvert.SerializeObject(customLocation.Region);
        }

        public Location toCustomLocation()
        {
            var customArea = JsonConvert.DeserializeObject<Area>(Region);
            return new Location(Latitude, Longitude, Created, Carrier_Data_Point, customArea);
        }

        [PrimaryKey, AutoIncrement]
        public int LocationId { get; set; }
        public DateTime Created { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public string Region { get; set; }
    }
}
