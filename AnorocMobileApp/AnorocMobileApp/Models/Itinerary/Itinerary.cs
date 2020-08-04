using System;
using System.Collections.Generic;
using AnorocMobileApp.ViewModels;
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

        #endregion
    }
}