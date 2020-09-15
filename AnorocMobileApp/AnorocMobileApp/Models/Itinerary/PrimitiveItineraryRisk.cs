using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace AnorocMobileApp.Models.Itinerary
{
    [Table("PrimitiveItineraryRisk")]
    public class PrimitiveItineraryRisk
    {
        /// <summary>
        /// Id for the itinerary
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ItineraryId { get; set; }
        
        public DateTime Created { get; set; }
        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied
        /// </summary>
        public string LocationItineraryRisks { get; set; }

        public PrimitiveItineraryRisk(ItineraryRisk risk)
        {
            Created = risk.Created;
            TotalItineraryRisk = risk.TotalItineraryRisk;
            var Dicitionary = new Dictionary<string, int>();
            for(int i = 0; i < risk.LocationItineraryRisks.Count; i++)
            {
                Dicitionary.Add(risk.LocationItineraryRisks.Keys.ElementAt(i).ToString(), risk.LocationItineraryRisks.Values.ElementAt(i));
            }
            LocationItineraryRisks = JsonConvert.SerializeObject(Dicitionary);
        }
        public PrimitiveItineraryRisk()
        {

        }
    }
}
