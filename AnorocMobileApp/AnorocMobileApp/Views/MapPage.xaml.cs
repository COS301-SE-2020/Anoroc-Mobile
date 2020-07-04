using AnorocMobileApp.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        MapViewModel viewModel = new MapViewModel();
        public Map()
        {
            InitializeComponent();

            //Task.Delay(2000);

            //UpdateMapAsync();
            DrawClusters();
        }


        async void DrawClusters()
        {
            List<Circle> circles = await viewModel.GetClustersForMap();
            foreach(Circle circle in circles)
            {
                MyMap.MapElements.Add(circle);
            }
            addPins();
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-25.783290, 28.274518), Distance.FromKilometers(1)));
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
            foreach(Pin pin in pins)
            {
                MyMap.Pins.Add(pin);
            }
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-25.783290, 28.274518), Distance.FromKilometers(1)));
        }
    }
}