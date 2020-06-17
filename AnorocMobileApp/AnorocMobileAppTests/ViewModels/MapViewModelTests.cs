using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnorocMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace AnorocMobileApp.Models.Tests
{
    [TestClass()]
    public class MapViewModelTests
    {
        [TestMethod()]
        public void GetPinsForAreaTest()
        {
            try
            {
                MapModel mapModel = new MapModel();
                var resultObject = mapModel.loadJsonFileToList();
                List<Place> placesList = new List<Place>();

                foreach (var place in resultObject.PointArray)
                {
                    placesList.Add(new Place
                    {
                        PlaceName = "Test Name",
                        Address = "Test Address",
                        Location = new Location(place.Latitude, place.Longitude),
                        Position = new Position(place.Latitude, place.Longitude),
                        //Icon = place.icon,
                        //Distance = $"{GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
                        //OpenNow = GetOpenHours(place?.opening_hours?.open_now)
                    });
                }
                Assert.IsNotNull(placesList[0].PlaceName);
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}