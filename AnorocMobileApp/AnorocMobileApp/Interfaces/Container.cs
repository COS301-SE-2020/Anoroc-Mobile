using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Interfaces
{
    public class Container
    {
        public Container(IBackgroundLocationService backgroundLocationService)
        {
            BackgroundLocationService = backgroundLocationService;
        }
        public static IBackgroundLocationService BackgroundLocationService {get; set;}
        public static ILocationService LocationService { get; set; }
        public static IUserManagementService userManagementService { get; set; }
    }
}