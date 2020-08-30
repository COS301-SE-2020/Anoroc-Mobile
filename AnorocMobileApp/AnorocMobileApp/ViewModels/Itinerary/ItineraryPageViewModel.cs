using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AnorocMobileApp.Models.Dashboard;
using AnorocMobileApp.Models.Itinerary;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Dashboard;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

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

        private ObservableCollection<Models.Itinerary.Itinerary> itineraries;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ItineraryPageViewModel" /> class.
        /// </summary>
        public ItineraryPageViewModel(INavigation navigation)
        {
            ImagePath = "EmptyItinerary.svg";
            Header = "EMPTY ITINERARY";
            Content = "You currently have no itineraries";
            GoBackCommand = new Command(GoBack);
            Navigation = navigation;

            AddItineraryCommand = new Command(async () => await AddItinerary());
            DeleteCommand = new Command(DeleteButtonClicked);
            ItineraryCommand = new Command(async obj => await ItineraryClicked(obj));
            
            PopulateItineraries();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the Go back button is clicked.
        /// </summary>
        public ICommand GoBackCommand { get; set; }
        
        /// <summary>
        /// Gets or sets the command is executed when the delete button is clicked.
        /// </summary>
        public Command DeleteCommand { get; set; }
        
        public ICommand ItineraryCommand { get; set; }
        
        public ICommand AddItineraryCommand { get; set; }

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

        public ObservableCollection<Models.Itinerary.Itinerary> Itineraries
        {
            get => itineraries ?? (itineraries = new ObservableCollection<Models.Itinerary.Itinerary>());
            set
            {
                if (itineraries != value)
                {
                    itineraries = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public INavigation Navigation { get; set;}
        
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
        
        /// <summary>
        /// Invoked when the delete button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void DeleteButtonClicked(object obj)
        {
            Itineraries.Remove(obj as Models.Itinerary.Itinerary);
        }

        private async Task ItineraryClicked(object obj)
        {
            if (obj is ItemTappedEventArgs eventArgs)
            {
                if (eventArgs.ItemData is Models.Itinerary.Itinerary data) 
                    await Navigation.PushAsync(new ViewItineraryPage(data.ItineraryRisk));
            }
        }

        /// <summary>
        /// Invoked when the Add Itinerary button is clicked
        /// </summary>
        /// <returns></returns>
        private async Task AddItinerary()
        {
            await Navigation.PushAsync(new AddItineraryPage());
        }

        public void PopulateItineraries()
        {
            Itineraries.Clear();
            var itineraryService = new ItineraryService();
            var itineraryRisks = itineraryService.ItinerariesFromLocal();

            foreach (var itinerary in itineraryRisks.Select(itineraryRisk => new Models.Itinerary.Itinerary()
            {
                Date = itineraryRisk.Created,
                NumberOfLocations = itineraryRisk.LocationItineraryRisks.Count,
                RiskDescription = ItineraryRiskDetail.RiskDescription[itineraryRisk.TotalItineraryRisk],
                ItineraryRisk = itineraryRisk
            }))
            {
                Itineraries.Add(itinerary);
            }
        }

        #endregion      
    }
}
