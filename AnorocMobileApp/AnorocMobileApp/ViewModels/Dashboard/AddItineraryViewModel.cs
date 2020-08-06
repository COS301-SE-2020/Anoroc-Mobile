using System;
using AnorocMobileApp.Models.Dashboard;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;
using System.Windows.Input;
using Xamarin.Forms;

namespace AnorocMobileApp.ViewModels.Dashboard
{
    /// <summary>
    /// ViewModel for Add Itinerary page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class AddItineraryViewModel : BaseViewModel, INotifyPropertyChanged
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
        /// Gets or sets a collection of value to be displayed in add itinerary page.
        /// </summary>
        [DataMember(Name = "dailyTimeline")]
        public ObservableCollection<Event> DailyTimeline { get; set; }

        #endregion
        
        #region Commands

        #endregion
        
        #region Methods

        #endregion
        
        #region Fields

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
