using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models.Itinerary
{
    class ItineraryRisk
    {
        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied
        /// </summary>
        public Dictionary<Location, int> LocationItineraryRisks { get; set; }
    }
}
