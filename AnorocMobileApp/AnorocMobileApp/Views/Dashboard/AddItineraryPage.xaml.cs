using System;
using System.Diagnostics;
using System.Linq;
using AnorocMobileApp.DataService;
using AnorocMobileApp.ViewModels.Dashboard;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Pickers;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Markup;
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
        private AddItineraryViewModel viewModel;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddItineraryPage" /> class.
        /// </summary>
        public AddItineraryPage()
        {
            InitializeComponent();
            // this.BindingContext = AddItineraryDataService.Instance.AddItineraryViewModel;
            viewModel = new AddItineraryViewModel();
            this.BindingContext = viewModel;
            this.popupLayout = CreateDatePopoutLayout();
            this.searchPopupLayout = CreateSearchPopoutLayout();
        }

        private void ClickedDate(object sender, EventArgs e)
        {
            this.popupLayout.Show();
        }

        private void ClickedAdd(object sender, EventArgs e)
        {
            this.searchPopupLayout.Show();
        }

        private SfPopupLayout CreateDatePopoutLayout()
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

        private SfPopupLayout CreateSearchPopoutLayout()
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
            searchBar.SetBinding(SearchBar.TextProperty, "AddressText");
            searchBar.TextChanged += OnSearchBarTextChanged;

            var sfListView = new SfListView
            {
                ItemsSource = viewModel.Addresses,
                Margin = new Thickness(8, 4)
            };
            // SfListView.SetBinding(SfListView.ItemsSourceProperty, "Addresses");


            var listViewDataTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                
                var addressForm = new Label
                {
                    FontAttributes = FontAttributes.None, 
                    BackgroundColor = Color.White, 
                    FontSize = 12
                };;
                addressForm.SetBinding(Label.TextProperty, new Binding("FreeformAddress"));
                grid.Children.Add(addressForm);
                
                return grid;
            });

            sfListView.ItemTemplate = listViewDataTemplate;
            
            stack.Children.Add(searchBar);
            stack.Children.Add(sfListView);
            
            popup.PopupView.ContentTemplate = new DataTemplate(() => stack);

            return popup;
        }


        private async void OnSearchBarTextChanged(object sender, EventArgs eventArgs)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.AddressText))
            {
                await viewModel.GetPlacesPredictonAsync();
            }
        }
    }
}