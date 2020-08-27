using AnorocMobileApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models.Itinerary
{
    public class ItineraryRisk
    {
        private Dictionary<Location, int> dictionary;


        public ItineraryRisk(DateTime created, int totalItineraryRisk, Dictionary<Location, int> dictionary)
        {
            Created = created;
            TotalItineraryRisk = totalItineraryRisk;
            LocationItineraryRisks = dictionary;
        }

        public ItineraryRisk()
        {
            LocationItineraryRisks = new Dictionary<Location, int>();
            TotalItineraryRisk = 0;
            Created = DateTime.Now;
        }
        public DateTime Created { get; set; }
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
