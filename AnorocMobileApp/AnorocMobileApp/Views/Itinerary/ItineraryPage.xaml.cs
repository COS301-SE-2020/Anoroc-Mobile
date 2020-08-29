using AnorocMobileApp.ViewModels.Itinerary;
using AnorocMobileApp.Views.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Itinerary
{
    /// <summary>
    /// Page to show the location denied
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItineraryPage
    {
        private ItineraryPageViewModel ViewModel; 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryPage" /> class.
        /// </summary>
        public ItineraryPage()
        {
            InitializeComponent();
            ViewModel = new ItineraryPageViewModel(Navigation);
            BindingContext = ViewModel;
            //Content = new EmptyPage();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.PopulateItineraries();
        }

        private void EmptyPage()
        {
            var grid = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };
            
        }
    }
}