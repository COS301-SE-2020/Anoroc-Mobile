using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Interfaces
{
    public static class Container
    {
        
        public static IBackgroundLocationService BackgroundLocationService {get; set;}
        public static ILocationService LocationService { get; set; }
    }
}