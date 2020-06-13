using System;
using System.Collections.Generic;
using System.Text;
namespace AnorocMobileApp.Models
{
    public class Points
    {
        public Point[] PointArray { get; set; }
    }

    public class Point
    {
        public string UserID { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }
}


