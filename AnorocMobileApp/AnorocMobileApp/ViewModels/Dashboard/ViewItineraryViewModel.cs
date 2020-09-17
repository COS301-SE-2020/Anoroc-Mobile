using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Dashboard;
using AnorocMobileApp.Models.Itinerary;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.ViewModels.Dashboard
{
    /// <summary>
    /// ViewModel for stock overview page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ViewItineraryViewModel : BaseViewModel
    {
        #region Field

        /// <summary>
        /// To store the health care data collection.
        /// </summary>
        private ObservableCollection<ItineraryRiskDetail> cardItems;

        /// <summary>
        /// To store the health care data collection.
        /// </summary>
        private ObservableCollection<ItineraryRiskDetail> listItems;

        /// <summary>
        /// To store the heart rate data collection.
        /// </summary>
        private ObservableCollection<ChartDataPoint> heartRateData;

        /// <summary>
        /// To store the calories burned data collection.
        /// </summary>
        private ObservableCollection<ChartDataPoint> caloriesBurnedData;

        /// <summary>
        /// To store the sleep time data collection.
        /// </summary>
        private ObservableCollection<ChartDataPoint> sleepTimeData;

        /// <summary>
        /// To store the water consumed data collection.
        /// </summary>
        private ObservableCollection<ChartDataPoint> waterConsumedData;

        /// <summary>
        /// To store the navigation object for navigating
        /// </summary>
        private INavigation Navigation { get; set; }

        /// <summary>
        /// To store locations along with their associated risk
        /// </summary>
        private Dictionary<Location, int> Locations { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ViewItineraryViewModel" /> class.
        /// </summary>
        public ViewItineraryViewModel(INavigation navigation, ItineraryRisk itineraryRisk)
        {
            var locations = itineraryRisk.LocationItineraryRisks;

            Navigation = navigation;
            Locations = locations;
            cardItems = new ObservableCollection<ItineraryRiskDetail>();
            var sortedLocations = from entry in locations orderby entry.Value descending select entry;
            foreach (var risk in sortedLocations)
            {
                cardItems.Add(new ItineraryRiskDetail(risk.Key.Region.Suburb, risk.Value));
            }

            AverageRisk = itineraryRisk.TotalItineraryRisk;
            CardColour = ItineraryRiskDetail.RiskGradientEnd[AverageRisk];
            Date = itineraryRisk.Created;
            Risk = ItineraryRiskDetail.RiskDescription[AverageRisk];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the profile image path.
        /// </summary>
        public string ProfileImage { get; set; }

        /// <summary>
        /// Gets or sets the health care items collection.
        /// </summary>
        public ObservableCollection<ItineraryRiskDetail> CardItems
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
        /// Gets or sets the health care items collection.
        /// </summary>
        public ObservableCollection<ItineraryRiskDetail> ListItems
        {
            get
            {
                return this.listItems;
            }

            set
            {
                this.listItems = value;
                this.NotifyPropertyChanged();
            }
        }

        public int AverageRisk { get; set; }

        public string CardColour { get; set; }

        public DateTime Date { get; set; }

        public string Risk { get; set; }

        #endregion

        #region Comments

        /// <summary>
        /// Gets or sets the command that will be executed when the menu button is clicked.
        /// </summary>
        public Command MenuCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the menu button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void MenuClicked(object obj)
        {
            // Do something
        }

        #endregion
    }
}
