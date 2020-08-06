using AnorocMobileApp.Models.Dashboard;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.ViewModels.Dashboard
{
    /// <summary>
    /// ViewModel for Add Itinerary page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class AddItineraryViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="AddItineraryViewModel"/> class.
        /// </summary>
        public AddItineraryViewModel()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a collction of value to be displayed in Daily timeline page.
        /// </summary>
        [DataMember(Name = "dailyTimeline")]
        public ObservableCollection<Event> DailyTimeline { get; set; }

        #endregion
    }
}
