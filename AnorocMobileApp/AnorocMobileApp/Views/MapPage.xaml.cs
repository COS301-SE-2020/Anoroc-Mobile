using AnorocMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AnorocMobileApp.ViewModels;
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

            UpdateMap();
        }

        

        private void UpdateMap()
        {
            // Simulate search for Menlyn Mall: new Position(-25.783290, 28.274518), Distance.FromKilometers(1)

            MyMap.ItemsSource = viewModel.GetPinsForArea();

            // Move the map to the searched reagion:
            //  Mapspan.FromCenterAndRadius:
            //      Center = Postiion: the geocoordinates for the searched place
            //      Radius = Distance.FromKilomters: How "zoomed" the map view is over the position
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-25.783290, 28.274518), Distance.FromKilometers(1)));
        }
    }
}