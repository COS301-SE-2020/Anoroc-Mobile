using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models.Itinerary
{
    public class ItineraryWrap
    {
        /// <summary>
        /// DateTime containing the date and time of the commencement of the travel
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// A list of locations to be visited.
        /// The locations will be in order of visitations.
        /// E.g. index 0 is the start location. index 1 follows start location.
        /// </summary>
        public List<Location> Locations;

        public ItineraryWrap()
        {

        }
        public ItineraryWrap(Itinerary userItinerary)
        {
            DateTime = userItinerary.DateTime;
            Locations = new List<Location>();
            userItinerary.Locations.ForEach(location =>
            {
                Locations.Add(location);
            });
        }
    }
}
