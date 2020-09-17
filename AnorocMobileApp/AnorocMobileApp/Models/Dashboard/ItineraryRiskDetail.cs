using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.Models.Dashboard
{
    /// <summary>
    /// Model for Health care page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ItineraryRiskDetail : INotifyPropertyChanged
    {
        public ItineraryRiskDetail(string locationName, int risk)
        {
            LocationName = locationName;
            LocationRisk = RiskDescription[risk];
            BackgroundGradientStart = RiskGradientStart[risk];
            BackgroundGradientEnd = RiskGradientEnd[risk];
        }

        #region Constants
        
        public static readonly Dictionary<int, string> RiskGradientStart = new Dictionary<int, string>()
        {
            {0, "#DCE35B"},
            {1, "#ffc500"},
            {2, "#F9D423"},
            {3, "#fe8c00"},
            {4, "#F00000"}
        };

        public static readonly Dictionary<int, string> RiskGradientEnd = new Dictionary<int, string>()
        {
            {0, "#45B649"},
            {1, "#c21500"},
            {2, "#e65c00"},
            {3, "#f83600"},
            {4, "#DC281E"}
        };

        public static readonly Dictionary<int, string> RiskDescription = new Dictionary<int, string>()
        {
            {0, "NO RISK"},
            {1, "LOW RISK"},
            {2, "MODERATE RISK"},
            {3, "MEDIUM RISK"},
            {4, "HIGH RISK"}
        };
        
        #endregion
        
        #region Field

        #endregion

        #region Events

        /// <summary>
        /// The declaration of the property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that has been displays the LocationName.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the Category value.
        /// </summary>
        public string LocationRisk { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the background gradient start.
        /// </summary>
        public string BackgroundGradientStart { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the background gradient end.
        /// </summary>
        public string BackgroundGradientEnd { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}