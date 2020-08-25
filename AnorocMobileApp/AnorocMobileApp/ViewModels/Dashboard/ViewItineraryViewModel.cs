using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Dashboard;
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
        private ObservableCollection<HealthCare> cardItems;

        /// <summary>
        /// To store the health care data collection.
        /// </summary>
        private ObservableCollection<HealthCare> listItems;

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

        private string averageRisk;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ViewItineraryViewModel" /> class.
        /// </summary>
        public ViewItineraryViewModel(INavigation navigation, Dictionary<Location, int> locations)
        {
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
            cardItems = new ObservableCollection<HealthCare>()
            {
                new HealthCare()
                {
                    Category = "University of Pretoria (Test)",
                    CategoryValue = riskDescription[0],
                    BackgroundGradientStart = riskGradientStart[0],
                    BackgroundGradientEnd = riskGradientEnd[0]
                },
                new HealthCare()
                {
                    Category = "McDonald's Hatfield (Test)",
                    CategoryValue = riskDescription[1],
                    BackgroundGradientStart = riskGradientStart[1],
                    BackgroundGradientEnd = riskGradientEnd[1]
                },
                new HealthCare()
                {
                    Category = "Centurion Gautrain (Test)",
                    CategoryValue = riskDescription[2],
                    BackgroundGradientStart = riskGradientStart[2],
                    BackgroundGradientEnd = riskGradientEnd[2]
                },
                new HealthCare()
                {
                    Category = "Loftus Versfeld Stadium (Test)",
                    CategoryValue = riskDescription[3],
                    BackgroundGradientStart = riskGradientStart[3],
                    BackgroundGradientEnd = riskGradientEnd[3]
                },
                new HealthCare()
                {
                    Category = "Menlyn Shopping Mall (Test)",
                    CategoryValue = riskDescription[4],
                    BackgroundGradientStart = riskGradientStart[4],
                    BackgroundGradientEnd = riskGradientEnd[4]
                }
            };
            var sortedLocations = from entry in locations orderby entry.Value descending select entry;
            foreach (var risk in sortedLocations)
            {
                cardItems.Add(new HealthCare()
                {
                    Category = risk.Key.Region.Suburb,
                    CategoryValue = riskDescription[risk.Value],
                    BackgroundGradientStart = riskGradientStart[risk.Value],
                    BackgroundGradientEnd = riskGradientEnd[risk.Value]
                    
                });
            }
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
        public ObservableCollection<HealthCare> CardItems
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
        public ObservableCollection<HealthCare> ListItems
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

        #endregion

        #region Comments

        /// <summary>
        /// Gets or sets the command that will be executed when the menu button is clicked.
        /// </summary>
        public Command MenuCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Chart Data Collection
        /// </summary>
        private void GetChartData()
        {
            DateTime dateTime = new DateTime(2019, 5, 1);

            heartRateData = new ObservableCollection<ChartDataPoint>()
            {
                new ChartDataPoint(dateTime, 15),
                new ChartDataPoint(dateTime.AddMonths(1), 20),
                new ChartDataPoint(dateTime.AddMonths(2), 17),
                new ChartDataPoint(dateTime.AddMonths(3), 23),
                new ChartDataPoint(dateTime.AddMonths(4), 18),
                new ChartDataPoint(dateTime.AddMonths(5), 25),
                new ChartDataPoint(dateTime.AddMonths(6), 19),
                new ChartDataPoint(dateTime.AddMonths(7), 21),
            };

            caloriesBurnedData = new ObservableCollection<ChartDataPoint>()
            {
                new ChartDataPoint(dateTime, 940),
                new ChartDataPoint(dateTime.AddMonths(1), 960),
                new ChartDataPoint(dateTime.AddMonths(2), 942),
                new ChartDataPoint(dateTime.AddMonths(3), 957),
                new ChartDataPoint(dateTime.AddMonths(4), 940),
                new ChartDataPoint(dateTime.AddMonths(5), 942),
            };

            sleepTimeData = new ObservableCollection<ChartDataPoint>()
            {
                new ChartDataPoint(dateTime, 7.8),
                new ChartDataPoint(dateTime.AddMonths(1), 7.2),
                new ChartDataPoint(dateTime.AddMonths(2), 8.0),
                new ChartDataPoint(dateTime.AddMonths(3), 6.8),
                new ChartDataPoint(dateTime.AddMonths(4), 7.6),
                new ChartDataPoint(dateTime.AddMonths(5), 7.0),
                new ChartDataPoint(dateTime.AddMonths(6), 7.5),
            };

            waterConsumedData = new ObservableCollection<ChartDataPoint>()
            {
                new ChartDataPoint(dateTime, 36),
                new ChartDataPoint(dateTime.AddMonths(1), 41),
                new ChartDataPoint(dateTime.AddMonths(2), 38),
                new ChartDataPoint(dateTime.AddMonths(3), 41),
                new ChartDataPoint(dateTime.AddMonths(4), 35),
                new ChartDataPoint(dateTime.AddMonths(5), 37),
                new ChartDataPoint(dateTime.AddMonths(6), 38),
                new ChartDataPoint(dateTime.AddMonths(7), 36),
            };
        }

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
