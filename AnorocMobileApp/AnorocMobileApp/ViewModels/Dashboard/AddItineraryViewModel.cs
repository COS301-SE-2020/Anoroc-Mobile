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
            OpenPopupCommand = new Command(OpenPopup);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a collection of value to be displayed in Daily timeline page.
        /// </summary>
        [DataMember(Name = "dailyTimeline")]
        public ObservableCollection<Event> DailyTimeline { get; set; }

        #endregion
        
        #region Commands
        
        public ICommand OpenPopupCommand { get; set; }

        #endregion
        
        #region Methods

        private void OpenPopup()
        {
            DisplayPopup = true;
        }

        #endregion
        
        #region Fields

        private bool displayPopup;

        public bool DisplayPopup
        {
            get => displayPopup;
            set
            {
                displayPopup = value;
                RaisePropertyChanged("DisplayPopup");
            }
        }

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
