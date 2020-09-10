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
        /// <summary>
        ///  Gets the next 10 itineraries from the server for the user
        /// </summary>
        /// <returns> List of the User's itineraries that are stored on the server, null if none found. </returns>
        Task<List<ItineraryRisk>> LoadMore();

        /// <summary>
        ///  Clears the list of user Itineraries and resets the pagination
        /// </summary>
        void Clear();

        /// <summary>
        ///  Sends the Itinerary to the server to have it analysied for he risk.
        /// </summary>
        /// <param name="userItinerary"> The Itinerary to be analysed. </param>
        /// <returns> The itinerary with associated risks for each location, null if error occured. </returns>
        Task<ItineraryRisk> ProcessItinerary(Itinerary userItinerary);

        /// <summary>
        ///  Gets the first pagination itineries from the server for the user.
        /// </summary>
        /// <returns> List of the user's itineraries, null if none found. </returns>
        Task<List<ItineraryRisk>> GetUserItineraries();

        Task<List<ItineraryRisk>> GetAllUserItineraries();

        void DeleteCloudItinerary(int id);
    }
}
