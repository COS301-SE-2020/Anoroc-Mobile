using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Itinerary;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Interfaces
{
    public interface IItineraryService
    {
        ItineraryRisk Refresh();

        void Clear();

        void ProcessItinerary(Itinerary userItinerary);

        ItineraryRisk GetUserItineraries();
    }
}
