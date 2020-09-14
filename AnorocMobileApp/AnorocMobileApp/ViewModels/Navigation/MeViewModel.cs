using System.Collections.ObjectModel;
using Xamarin.Forms.Internals;
using AnorocMobileApp.Models;
using Xamarin.Forms;
using AnorocMobileApp.Services;
using System.IO;

namespace AnorocMobileApp.ViewModels.Navigation
{
    /// <summary>
    /// ViewModel for Me page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MeViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// To store the health profile data collection.
        /// </summary>
        private ObservableCollection<Me> cardItems;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="MeViewModel" /> class.
        /// </summary>
        public MeViewModel()
        {
            cardItems = new ObservableCollection<Me>()
            {
                
            };
            //cardItems.Add();                            
            //CardItem button onPress={() => Alert.alert('hi')}

            this.ProfileImage = App.BaseImageUrl + "profilepicture.jpg";
            this.ProfileName = "Anoroc Van Looi";
            this.State = "Gauteng";
            this.Country = "South Africa";
            this.Age = "35";
            this.Location = "Disabled";
            if (BackgroundLocationService.Tracking)
            {
                this.Location = "Enabled";
            }
           
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the health profile items collection.
        /// </summary>
        public ObservableCollection<Me> CardItems
        {
            get
            {
                return this.cardItems;
            }

            set
            {
                this.cardItems = value;
                this.NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>
        public string ProfileImage { get; set; }

        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string ProfileName { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the incidents.
        /// </summary>
        public string Incidents { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the preventions.
        /// </summary>
        public string Preventions { get; set; }

        

        #endregion
    }
}