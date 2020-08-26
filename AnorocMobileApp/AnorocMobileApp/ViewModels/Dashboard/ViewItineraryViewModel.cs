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

            var riskGradientStart = new Dictionary<int, string>()
            {
                {0, "#DCE35B"},
                {1, "#ffc500"},
                {2, "#F9D423"},
                {3, "#fe8c00"},
                {4, "#F00000"}
            };

            var riskGradientEnd = new Dictionary<int, string>()
            {
                {0, "#45B649"},
                {1, "#c21500"},
                {2, "#e65c00"},
                {3, "#f83600"},
                {4, "#DC281E"}
            };

            var riskDescription = new Dictionary<int, string>()
            {
                {0, "NO RISK"},
                {1, "LOW RISK"},
                {2, "MODERATE RISK"},
                {3, "MEDIUM RISK"},
                {4, "HIGH RISK"}
            };
            
            Navigation = navigation;
            Locations = locations;
            cardItems = new ObservableCollection<ItineraryRiskDetail>()
            {
                new ItineraryRiskDetail()
                {
                    LocationName = "University of Pretoria (Test)",
                    LocationRisk = riskDescription[0],
                    BackgroundGradientStart = riskGradientStart[0],
                    BackgroundGradientEnd = riskGradientEnd[0]
                },
                new ItineraryRiskDetail()
                {
                    LocationName = "McDonald's Hatfield (Test)",
                    LocationRisk = riskDescription[1],
                    BackgroundGradientStart = riskGradientStart[1],
                    BackgroundGradientEnd = riskGradientEnd[1]
                },
                new ItineraryRiskDetail()
                {
                    LocationName = "Centurion Gautrain (Test)",
                    LocationRisk = riskDescription[2],
                    BackgroundGradientStart = riskGradientStart[2],
                    BackgroundGradientEnd = riskGradientEnd[2]
                },
                new ItineraryRiskDetail()
                {
                    LocationName = "Loftus Versfeld Stadium (Test)",
                    LocationRisk = riskDescription[3],
                    BackgroundGradientStart = riskGradientStart[3],
                    BackgroundGradientEnd = riskGradientEnd[3]
                },
                new ItineraryRiskDetail()
                {
                    LocationName = "Menlyn Shopping Mall (Test)",
                    LocationRisk = riskDescription[4],
                    BackgroundGradientStart = riskGradientStart[4],
                    BackgroundGradientEnd = riskGradientEnd[4]
                }
            };
            var sortedLocations = from entry in locations orderby entry.Value descending select entry;
            foreach (var risk in sortedLocations)
            {
                cardItems.Add(new ItineraryRiskDetail()
                {
                    LocationName = risk.Key.Region.Suburb,
                    LocationRisk = riskDescription[risk.Value],
                    BackgroundGradientStart = riskGradientStart[risk.Value],
                    BackgroundGradientEnd = riskGradientEnd[risk.Value]
                    
                });
            }

            AverageRisk = itineraryRisk.TotalItineraryRisk;
            CardColour = riskGradientEnd[AverageRisk];
            Date = itineraryRisk.Created;
            Risk = riskDescription[AverageRisk];
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
