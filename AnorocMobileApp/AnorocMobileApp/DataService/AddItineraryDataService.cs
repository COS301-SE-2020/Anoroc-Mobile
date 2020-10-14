using System.Reflection;
using System.Runtime.Serialization.Json;
using AnorocMobileApp.ViewModels.Itinerary;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.DataService
{
    /// <summary>
    /// Data service to load the data from json file.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AddItineraryDataService
    {
        #region Fields

        private static AddItineraryDataService instance;
        private AddItineraryViewModel addItineraryViewModel;
        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance for the <see cref="AddItineraryDataService"/> class.
        /// </summary>
        public AddItineraryDataService()
        {
        }
        #endregion

        #region properties

        /// <summary>
        /// Gets an instance of the <see cref="AddItineraryDataService"/>.
        /// </summary>
        public static AddItineraryDataService Instance => instance ?? (instance = new AddItineraryDataService());

        /// <summary>
        /// Gets or sets the value of Add Itinerary page view model.
        /// </summary>
        public AddItineraryViewModel AddItineraryViewModel =>
            (this.addItineraryViewModel = PopulateData<AddItineraryViewModel>("timeline.json"));
        #endregion

        #region Methods

        /// <summary>
        /// Populates the data for view model from json file.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="fileName">Json file to fetch data.</param>
        /// <returns>Returns the view model object.</returns>
        private static T PopulateData<T>(string fileName)
        {
            var file = "AnorocMobileApp.Data." + fileName;

            var assembly = typeof(App).GetTypeInfo().Assembly;

            T obj;

            using (var stream = assembly.GetManifestResourceStream(file))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                obj = (T)serializer.ReadObject(stream);
            }

            return obj;
        }
        #endregion
    }
}
