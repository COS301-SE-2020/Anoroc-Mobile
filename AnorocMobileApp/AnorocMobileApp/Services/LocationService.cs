using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Services
{
    public class LocationService : ILocationService
    {
        private bool success;

        public LocationService()
        {
            success = false;
        }
        public void Send_Locaiton_Server(Location lcoation)
        {

            success = true;
            if (!success)
            {
                throw new CantConnectToLocationServerException();
            }
        }
    }
}
