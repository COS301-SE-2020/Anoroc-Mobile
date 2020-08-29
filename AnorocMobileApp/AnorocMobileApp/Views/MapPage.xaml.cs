using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        MapViewModel viewModel;
        int CurrentRange;
        public Map()
        {
            InitializeComponent();

            //Task.Delay(2000);

            //UpdateMapAsync();
            CurrentRange = 0;
            MessagingCenter.Subscribe<UserLoggedIn>(this, "UserLoggedIn", message =>
            {
                DrawClusters();
            });
        }

        public async void DrawClusters()
        {
            Slider.IsEnabled = false;
            viewModel = new MapViewModel();
            List<Circle> circles = await viewModel.GetClustersForMap();
            if (circles != null)
            {
                foreach (Circle circle in circles)
                {
                    MyMap.MapElements.Add(circle);
                }
                addPins();
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-25.783290, 28.274518), Distance.FromKilometers(1)));
            }
            Slider.IsEnabled = true;
        }
        void addPins()
        {
            List<Pin> pins = viewModel.Pins;
            foreach (Pin pin in pins)
            {
                MyMap.Pins.Add(pin);
            }
        }

        /// <summary>
        /// Function to update the map based on the View Model's GetPinsForArea
        /// Move the map to the searched reagion:
        /// Mapspan.FromCenterAndRadius:
        /// Center = Postiion: the geocoordinates for the searched place
        /// Radius = Distance.FromKilomters: How "zoomed" the map view is over the position
        /// </summary>
        ///<param name="viewModel">An instance of the MapViewModel used to load a Map view along with Data Points on the map</param>
        async void UpdateMapAsync()
        {
            List<Pin> pins = await viewModel.GetPinsForAreaAsync();
           
            if (pins != null)
            {
                foreach (Pin pin in pins)
                {
                    MyMap.Pins.Add(pin);
                }
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-25.783290, 28.274518), Distance.FromKilometers(1)));
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var outputString = "Clusters from ";
            var theValue = (int)e.NewValue;

            if (theValue != CurrentRange)
            {
                switch (theValue)
                {
                    case (0):
                        DrawClusters();
                        outputString += " Today.";
                        break;
                    case (1):
                        outputString += 1 + " Day Ago";
                        break;
                    case (2):
                        outputString += 2 + " Days Ago";
                        break;
                    case (3):
                        outputString += 3 + " Days Ago";
                        break;
                    case (4):
                        outputString += 4 + " Days Ago";
                        break;
                    case (5):
                        outputString += 5 + " Days Ago";
                        break;
                    case (6):
                        outputString += 6 + " Days Ago";
                        break;
                    case (7):
                        outputString += 7 + " Days Ago";
                        break;
                    case (8):
                        outputString += 8 + " Days Ago";
                        break;
                    default:
                        DrawClusters();
                        outputString += " Today.";
                        break;
                }
                SliderLabel.Text = outputString;
            }
        }
    }
}