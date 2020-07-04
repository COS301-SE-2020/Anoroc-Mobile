using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class Location
    {
        public Location()
        {

        }
   
        public DateTime Created { get; set; }
        public GEOCoordinate Coordinate { get; set; }
        public bool Carrier_Data_Point { get; set; }

    }
}
