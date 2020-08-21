using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnorocMobileApp.Models.Itinerary
{
    public class ItineraryRiskWrapper
    {
        public ItineraryRiskWrapper()
        {

        }
        public DateTime Created { get; set; }
        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied
        /// </summary>
        public Dictionary<string,int> LocationItineraryRisks { get; set; }

        public ItineraryRisk toItineraryRisk()
        {
            Dictionary<Location, int> myvar = new Dictionary<Location, int>();

            for(int i = 0; i < LocationItineraryRisks.Count; i++)
            {
                myvar.Add(JsonConvert.DeserializeObject<Location>(LocationItineraryRisks.Keys.ElementAt(i)), LocationItineraryRisks.Values.ElementAt(i));
            }

            return new ItineraryRisk(Created, TotalItineraryRisk, myvar);
        }

    }
}
