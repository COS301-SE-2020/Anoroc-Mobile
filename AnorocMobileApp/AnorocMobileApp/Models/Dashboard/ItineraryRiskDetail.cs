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