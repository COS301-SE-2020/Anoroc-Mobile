using System;
using AnorocMobileApp.Controls;
using AnorocMobileApp.ViewModels.Itinerary;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Border;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Itinerary
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
            // viewModel = new AddItineraryViewModel(Navigation, this);
            ((AddItineraryViewModel) BindingContext).View = this;
            ((AddItineraryViewModel) BindingContext).Navigation = Navigation;
            viewModel = (AddItineraryViewModel) BindingContext;
            popupLayout = CreateDatePopoutLayout();
            searchPopupLayout = CreateSearchPopoutLayout();
        }

        private void ClickedDate(object sender, EventArgs e)
        {
            popupLayout.Show();
        }

        private void ClickedAdd(object sender, EventArgs e)
        {
            searchPopupLayout.Show();
        }

        private SfPopupLayout CreateDatePopoutLayout()
        {
            var popup = new SfPopupLayout
            {
                PopupView =
                {
                    HeaderTitle = "Itinerary date"
                },
            };
            
            var nativeDatePicker = new DatePicker
            {
              HorizontalOptions  = LayoutOptions.Center,
              
                  
            };

            popup.PopupView.ContentTemplate = new DataTemplate(() => nativeDatePicker);

            return popup;
        }

        private SfPopupLayout CreateSearchPopoutLayout()
        {
            var popup = new SfPopupLayout
            {
                PopupView =
                {
                    ShowHeader = false,
                    ShowFooter = false,
                    AnimationMode = AnimationMode.Zoom
                }
            };

            var stack = new StackLayout();
            
            var searchBar = new SearchBar();
            searchBar.SetBinding(SearchBar.TextProperty, "AddressText");
            searchBar.TextChanged += OnSearchBarTextChanged;

            var topStack = new StackLayout
            {
                IsVisible = false,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0.0,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            var backButton = new SfButton();
            backButton.Clicked += (sender, args) =>
            {
                popup.IsOpen = false;
                popup.IsVisible = false;
            };
            Application.Current.Resources.TryGetValue("Back", out var resource);
            // backButton.Text = (string) resource;
            // backButton.Style = (Style) Resources["NavigationBarButtonStyle"];
            
            topStack.Children.Add(backButton);

            var searchBorder = new SfBorder();
            
            var borderlessEntry = new BorderlessEntry();
            borderlessEntry.SetBinding(Entry.TextProperty, "AddressText");
            borderlessEntry.Placeholder = "Search here";
            borderlessEntry.HorizontalOptions = LayoutOptions.FillAndExpand;
           // borderlessEntry.Style = (Style) Resources["SearchEntryStyle"];

          //  searchBorder.Children.Append(borderlessEntry);

            topStack.Children.Add(borderlessEntry);
            
            var sfListView = new SfListView
            {
                ItemsSource = viewModel.Addresses,
                Margin = new Thickness(8, 4),
                TapCommand = viewModel.SearchLocationTapped
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