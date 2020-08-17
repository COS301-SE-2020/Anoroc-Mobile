using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.ViewModels.Itinerary
{
    /// <summary>
    /// ViewModel for location denied page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ItineraryPageViewModel : BaseViewModel
    {
        #region Fields

        private string imagePath;

        private string header;

        private string content;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ItineraryPageViewModel" /> class.
        /// </summary>
        public ItineraryPageViewModel()
        {
            this.ImagePath = "EmptyItinerary.svg";
            this.Header = "EMPTY ITINERARY";
            this.Content = "You currently have no itineraries";
            this.GoBackCommand = new Command(this.GoBack);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the Go back button is clicked.
        /// </summary>
        public ICommand GoBackCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ImagePath.
        /// </summary>
        public string ImagePath
        {
            get => this.imagePath;

            set
            {
                this.imagePath = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Header.
        /// </summary>
        public string Header
        {
            get => this.header;

            set
            {
                this.header = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public string Content
        {
            get => this.content;

            set
            {
                this.content = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Go back button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void GoBack(object obj)
        {
            // Do something
        }

        #endregion      
    }
}
