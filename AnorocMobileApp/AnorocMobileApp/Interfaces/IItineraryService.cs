using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Itinerary;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Interfaces
{
    public interface IItineraryService
    {
        Dictionary<Location, int> Refresh();

        void ProcessItinerary(Itinerary userItinerary);

        Dictionary<Location, int> GetUserItineraries();
    }
}
