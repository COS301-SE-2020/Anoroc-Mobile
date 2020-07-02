using System;
using System.Collections.Generic;

namespace AnorocMobileApp.Models
{
    public class Clusters
    {
        public List<Location> Coordinates { get; set; }
        public DateTime Created { get; set; }
        public bool Carrier_Data_Point { get; set; }
    }
}