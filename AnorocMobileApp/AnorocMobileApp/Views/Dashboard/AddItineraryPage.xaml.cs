using System;
using AnorocMobileApp.DataService;
using AnorocMobileApp.ViewModels.Dashboard;
using Syncfusion.ListView.XForms;
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
        private SfPopupLayout searchPopupLayout;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddItineraryPage" /> class.
        /// </summary>
        public AddItineraryPage()
        {
            this.popupLayout = CreateDatePopoutLayout();
            this.searchPopupLayout = CreateSearchPopoutLayout();
            InitializeComponent();
            // this.BindingContext = AddItineraryDataService.Instance.AddItineraryViewModel;
            this.BindingContext = new AddItineraryViewModel();
        }

        private void ClickedDate(object sender, EventArgs e)
        {
            this.popupLayout.Show();
        }

        private void ClickedAdd(object sender, EventArgs e)
        {
            this.searchPopupLayout.Show();
        }

        private static SfPopupLayout CreateDatePopoutLayout()
        {
            var popup = new SfPopupLayout()
            {
                PopupView =
                {
                    HeaderTitle = "Itinerary date"
                },
            };
            
            var datePicker = new SfDatePicker()
            {
                ShowHeader = false
            };

            popup.PopupView.ContentTemplate = new DataTemplate(() => datePicker);

            return popup;
        }

        private static SfPopupLayout CreateSearchPopoutLayout()
        {
            var popup = new SfPopupLayout()
            {
                PopupView =
                {
                    ShowHeader = false,
                    ShowFooter = false
                }
            };

            var stack = new StackLayout();
            
            var searchBar = new SearchBar();
            var listView = new SfListView();
            stack.Children.Add(searchBar);
            stack.Children.Add(listView);
            
            popup.PopupView.ContentTemplate = new DataTemplate(() => stack);

            return popup;

        }
    }
}