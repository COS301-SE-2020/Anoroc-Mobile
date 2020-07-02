using System.Collections.Generic;
using Xamarin.Forms.Maps;

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
        List<Clusters> Cluster_List;
        public MapViewModel()
        {
            Cluster_List = new List<Clusters>();
        }
        


        /// <summary>
        ///     Wrapper function that turns the returned clusters into pins used by the map
        /// </summary>
        /// <returns> The pins </returns>
        public async System.Threading.Tasks.Task<List<Pin>> GetPinsForAreaAsync()
        {
            List<Pin> map_pins = new List<Pin>();

            Cluster_List = await mapModel.GetClustersAsync();

            foreach (Clusters cluster in Cluster_List)
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

