using System;
using AnorocMobileApp.ViewModels;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.Models
{
    /// <summary>
    /// Class to contain the location and date of contact with a contagion carrier
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ContagionContact : BaseViewModel
    {
        #region Public properties

        /// <summary>
        /// string containing the location of contact
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// DateTime containing the date and time of contact
        /// </summary>
        public DateTime DateTime { get; set; } 
        #endregion
    }
}