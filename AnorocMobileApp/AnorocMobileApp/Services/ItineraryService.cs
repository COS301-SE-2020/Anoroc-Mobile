using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Itinerary;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Services
{
    class ItineraryService : IItineraryService
    {
        public static int Pagination { get; private set; }
        public ItineraryRisk UserItineraries { get; private set; }
        public void Clear()
        {
            Pagination = 10;
            UserItineraries.LocationItineraryRisks.Clear();
            UserItineraries.LocationItineraryRisks = null;
            UserItineraries = null;
        }

        public ItineraryRisk GetUserItineraries()
        {
            throw new NotImplementedException();
        }

        public void ProcessItinerary(Itinerary userItinerary)
        {
            throw new NotImplementedException();
        }

        public ItineraryRisk Refresh()
        {
            Pagination += 10;
            

            return UserItineraries;
        }
    }
}
