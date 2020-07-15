using System;
using AnorocMobileApp.ViewModels;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.Models
{
    [Preserve(AllMembers = true)]
    public class ContagionContact : BaseViewModel
    {
        #region Public properties

        public Location Location { get; set; }

        public DateTime DateTime { get; set; } 
        #endregion
    }
}