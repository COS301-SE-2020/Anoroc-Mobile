using System;
using System.Collections.Generic;
using AnorocMobileApp.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.Models.Itinerary
{
    /// <summary>
    /// Class representing an itinerary of a user
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Itinerary : BaseViewModel
    {
        #region Public properties
        public Itinerary()
        {
            
        }
        
        /// <summary>
        /// Id for the itinerary
        /// </summary>
        public int ItineraryId { get; set; }
        
        /// <summary>
        /// DateTime containing the date and time of the commencement of the travel
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// A list of locations to be visited.
        /// The locations will be in order of visitations.
        /// E.g. index 0 is the start location. index 1 follows start location.
        /// </summary>
        public List<Location> Locations;
        
        /// <summary>
        /// Number of locations is the current itinerary
        /// </summary>
        public int NumberOfLocations { get; set; }

        /// <summary>
        /// Description of the risk of this itinerary (if already analysed)
        /// </summary>
        public string RiskDescription { get; set; }
        
        /// <summary>
        /// Stores the itinerary risk that generated this itinerary (if there is)
        /// </summary>
        public ItineraryRisk ItineraryRisk { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}