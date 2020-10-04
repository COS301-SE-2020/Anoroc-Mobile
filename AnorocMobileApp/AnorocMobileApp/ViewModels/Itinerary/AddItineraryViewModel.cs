using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AnorocMobileApp.Helpers;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Dashboard;
using AnorocMobileApp.Models.Itinerary;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Dashboard;
using Newtonsoft.Json;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
using Xamarin.Forms.Internals; //using AnorocMobileApp.Models;
//using Itinerary = AnorocMobileApp.Models.Itinerary;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace AnorocMobileApp.ViewModels.Itinerary
{
    /// <summary>
    /// ViewModel for Add Itinerary page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class AddItineraryViewModel : BaseViewModel
    {
        #region Constructor
        
        public AddItineraryViewModel()
        {
            SearchLocationTapped = new Command<object>(SearchLocationTappedMethod);
            DoneButtonTapped = new Command(DoneTappedMethod);
            OpenSearchDialog = new Command<SfPopupLayout>(DisplaySearchDialog);
            Date = DateTime.Today;
        }

        /// <summary>
        /// Initializes a new instance for the <see cref="AddItineraryViewModel"/> class.
        /// </summary>
        public AddItineraryViewModel(INavigation navigation, Page view)
        {
            SearchLocationTapped = new Command<object>(SearchLocationTappedMethod);
            DoneButtonTapped = new Command(DoneTappedMethod);
            OpenSearchDialog = new Command<SfPopupLayout>(DisplaySearchDialog);
            Navigation = navigation;
            this.View = view;
            this.Date = DateTime.Today;
        }

        #endregion
        
        #region HttpRequest
        
        private HttpClient _httpClientInstance;

        private HttpClient HttpClientInstance
        {
            get
            {
                if (_httpClientInstance == null)
                {
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    };
                    _httpClientInstance = _httpClientInstance ?? (_httpClientInstance = new HttpClient());
                }
                return _httpClientInstance;
            }
        }

        
        #endregion
        
        #region Fields

        private ObservableCollection<Result> results;
        private ObservableCollection<Address> addresses;
        private List<Location> locations;
        private ObservableCollection<Address> addressTimeline;
        private string addressText;
        private DateTime date;

        public INavigation Navigation { get; set;}
        public Page View { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a collection of value to be displayed in add itinerary page.
        /// </summary>
        [DataMember(Name = "dailyTimeline")]
        public ObservableCollection<Event> DailyTimeline { get; set; }

        public ObservableCollection<Result> Results
        {
            get => results ?? (results = new ObservableCollection<Result>());
            set
            {
                if (results != value)
                {
                    results = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public ObservableCollection<Address> Addresses
        {
            get => addresses ?? (addresses = new ObservableCollection<Address>());
            set
            {
                if (addresses != value)
                {
                    addresses = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private List<Location> Locations
        {
            get => locations ?? (locations = new List<Location>());
            set
            {
                if (locations != value)
                {
                    locations = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<Address> AddressTimeline
        {
            get => addressTimeline ?? (addressTimeline = new ObservableCollection<Address>());
            set
            {
                if (addressTimeline != value)
                {
                    addressTimeline = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string AddressText 
        {
            get => addressText;
            set 
            {
                if (addressText != value) {
                    addressText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion
        
        #region Commands

        public Command<object> SearchLocationTapped { get; set; }
        
        public Command DoneButtonTapped { get; set; }
        
        public Command<SfPopupLayout> OpenSearchDialog { get; set; }

        #endregion
        
        #region Methods

        public async Task GetPlacesPredictonAsync()
        {
            Debug.Print("In GetPlaces function");
            // TODO: Add some logic to slow down requests
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token;

            // TODO: Pass current lon and lat to show areas closer to the user
            var url = string.Format(Constants.AzureFuzzySearchUrl,
                WebUtility.UrlEncode(addressText),
                Secrets.AzureMapsSubscriptionKey);
            using (var message = await HttpClientInstance.GetAsync(url, HttpCompletionOption.ResponseContentRead, cancellationToken))
            {
                if (message.IsSuccessStatusCode)
                {
                    var json = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var wholeResponse = await Task.Run(() => JsonConvert.DeserializeObject<Welcome>(json)).ConfigureAwait(false);
                    
                    // TODO: Maybe check if it converted successfully
                    
                    Addresses.Clear();
                    Results.Clear();

                    if (wholeResponse.Summary.TotalResults > 0)
                    {
                        foreach (var result in wholeResponse.Results)
                        {
                            Addresses.Add(result.Address);
                            Results.Add(result);
                        }
                    }
                }
            }
        }

        private async void SearchLocationTappedMethod(object obj)
        {
            if (obj is ItemTappedEventArgs item)
            {
                var address = item.ItemData as Address;
                foreach (var result in Results)
                {
                    if (result.Address == address)
                    {
                        // TODO perhaps just store Position object instead of Location
                        var location = new Location(result.Position);
                        await location.GetRegion();
                        Locations.Add(location);
                        AddressTimeline.Add(result.Address);
                    }
                }
            };
        }

        private async void DoneTappedMethod()
        {
            var itinerary = new Models.Itinerary.Itinerary {Locations = Locations, Date = Date};
            var service = new ItineraryService();
            var risk = await service.ProcessItinerary(itinerary);
            Navigation.InsertPageBefore(new ViewItineraryPage(risk), View);
            await Navigation.PopAsync();
        }

        private void DisplaySearchDialog(SfPopupLayout popupLayout)
        {
            popupLayout.Show();
        }
        
        #endregion

    }
}
