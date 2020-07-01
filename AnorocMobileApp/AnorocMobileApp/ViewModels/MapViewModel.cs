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
        /// <param name="placesList">A list of Place Onjects used to store all the data points</param>
        MapModel mapModel = new MapModel();
        List<Place> placesList;
        public MapViewModel()
        {
            placesList = new List<Place>();
        }
        /// <summary>
        /// Loads data points from JSON object and creates a list of Place objefcts based on the JSON data
        /// </summary>
        /// <param name="resultObject">The parsed JSON data that is used to read through the JSON</param>
        /// <returns>A list of new 'Place' Objects</returns>
        public List<Place> GetPinsForArea()
        {  
            var resultObject = mapModel.loadJsonFileToList();
            if (resultObject != null)
            {
                foreach (var place in resultObject.PointArray)
                {
                    placesList.Add(new Place
                    {
                        PlaceName = "Test Name",
                        Address = "Test Address",
                        Location = new Location(place.Latitude, place.Longitude),
                        Position = new Position(place.Latitude, place.Longitude)
                        //Icon = place.icon,
                        //Distance = $"{GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
                        //OpenNow = GetOpenHours(place?.opening_hours?.open_now)
                    });
                }
                return placesList;
            }
            else
                return null;
        }
    }
}
//PlacesListView.ItemsSource = placesList;
//var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
/* MyMap.Pins.Add(new Pin
                {
                    Label = place.name,
                    Address = place.vicinity,
                    Type = PinType.Place,
                    Position = new Position(place.geometry.location.lat, place.geometry.location.lng)
                });*/
