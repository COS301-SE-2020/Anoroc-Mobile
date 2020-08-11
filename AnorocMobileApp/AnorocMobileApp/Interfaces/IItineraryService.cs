using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Itinerary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnorocMobileApp.Interfaces
{
    public interface IItineraryService
    {
        Task<List<ItineraryRisk>> LoadMore();

        void Clear();

        Task<List<ItineraryRisk>> ProcessItinerary(Itinerary userItinerary);

        Task<List<ItineraryRisk>> GetUserItineraries();
    }
}
