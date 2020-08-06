using AnorocMobileApp.DataService;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Dashboard
{
    /// <summary>
    /// Page to add an itinerary.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItineraryPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddItineraryPage" /> class.
        /// </summary>
        public AddItineraryPage()
        {
            InitializeComponent();
            this.BindingContext = AddItineraryDataService.Instance.AddItineraryViewModel;
        }
    }
}