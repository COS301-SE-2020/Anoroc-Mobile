using System;
using AnorocMobileApp.DataService;
using Syncfusion.XForms.Pickers;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
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
        private SfPopupLayout popupLayout;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddItineraryPage" /> class.
        /// </summary>
        public AddItineraryPage()
        {
            this.popupLayout = CreatePopupLayout();
            InitializeComponent();
            this.BindingContext = AddItineraryDataService.Instance.AddItineraryViewModel;
        }

        private void Clicked(object sender, EventArgs e)
        {
            this.popupLayout.Show();
        }

        private SfPopupLayout CreatePopupLayout()
        {
            var popup = new SfPopupLayout();
            
            var datePicker = new SfDatePicker()
            {
                ShowHeader = false
            };

            popup.PopupView.ContentTemplate = new DataTemplate(() => datePicker);

            return popup;
        }
    }
}