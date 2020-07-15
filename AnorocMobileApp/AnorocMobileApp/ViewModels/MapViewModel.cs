using System.Collections.Generic;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AnorocMobileApp.Models
{
    class MapViewModel
    {
        /// <summary>
        /// Public instances declared in MapViewModel Class along with constructor
        /// </summary>
        /// <param name="mapModel">A new instancce of the MapModel Object used to show the map</param>
        /// <param name="Cluster_List">A list of Place Objects used to store all the data points</param>
        MapModel mapModel = new MapModel();
        List<ClusterAllPins> Cluster_List;

        public List<Pin> Pins = new List<Pin>();
        public MapViewModel()
        {
            Cluster_List = new List<ClusterAllPins>();
        }
        



        public async Task<List<Circle>> GetClustersForMap()
        {
            List<Cluster> clusters = await mapModel.GetClustersWithRadius();
            List<Circle> circles = new List<Circle>();
            
            foreach (Cluster cluster in clusters)
            {
                Circle circle = new Circle
                {
                    Center = new Position(cluster.Center_Pin.Coordinate.Latitude, cluster.Center_Pin.Coordinate.Longitude),
                    Radius = new Distance(cluster.Cluster_Radius),
                    StrokeColor = Color.FromHex("#88FF0000"),
                    StrokeWidth = 8,
                    FillColor = Color.FromHex("#88FFC0CB")
                };
                // clickable center pin
                Pins.Add(new Pin
                {
                    Label = "Cluster Count: " + cluster.Pin_Count,
                    Address = "Percentage carrier: " + (int)((cluster.Carrier_Pin_Count / cluster.Pin_Count) * 100),
                    Type = PinType.SearchResult,
                    Position = new Position(cluster.Center_Pin.Coordinate.Latitude, cluster.Center_Pin.Coordinate.Longitude)
                });
                circles.Add(circle);
            }
            return circles;
        }


        /// <summary>
        ///     Wrapper function that turns the returned clusters into pins used by the map
        /// </summary>
        /// <returns> The pins </returns>
        public async Task<List<Pin>> GetPinsForAreaAsync()
        {
            List<Pin> map_pins = new List<Pin>();

            Cluster_List = await mapModel.GetClustersAsync();

            foreach (ClusterAllPins cluster in Cluster_List)
            {
                foreach (Location location in cluster.Coordinates)
                {
                    map_pins.Add(new Pin
                    {
                        Label = "Test name",
                        Address = "Test Address",
                        Type = PinType.Place,
                        Position = new Position(location.Coordinate.Latitude, location.Coordinate.Longitude)
                    });
                }
            }
            return map_pins;
        }
    }
}

