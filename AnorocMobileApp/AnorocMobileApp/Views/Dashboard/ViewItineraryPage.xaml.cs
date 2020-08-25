using System.Collections.Generic;
using AnorocMobileApp.Models;
using AnorocMobileApp.ViewModels.Dashboard;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Dashboard
{
    /// <summary>
    /// Page to show the health care details.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewItineraryPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewItineraryPage" /> class.
        /// </summary>
        public ViewItineraryPage(List<Location> locations)
        {
            InitializeComponent();
            BindingContext = new ViewItineraryViewModel(Navigation, locations);
        }
    }
}
