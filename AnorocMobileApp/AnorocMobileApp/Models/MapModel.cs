using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using AnorocMobileApp.Services;
using System.Threading.Tasks;
using AnorocMobileApp.Exceptions;

namespace AnorocMobileApp.Models
{
    class MapModel
    {
        MapService Map_Service;
        public MapModel()
        {
            Map_Service = new MapService();
        }

        public async Task<List<Cluster>> GetClustersWithRadius()
        {
            List<Cluster> cluster = null;
            try
            {
                cluster = await Map_Service.GetClustersForCirclesAsync();
            }
            catch(CantConnecToClusterServiceException)
            {
                //TODO:
                // Retry logic for not connecting to the cluster service
            }
            return cluster;
        }

        public async Task<List<ClusterAllPins>> GetClustersAsync()
        {
            List<ClusterAllPins> clusters = null;
            try
            {
                clusters = await Map_Service.FetchClustersAsync();
            }
            catch(CantConnecToClusterServiceException)
            {
                //TODO:
                // Retry logic for not connecting to the cluster service
            }
            return clusters;
        }



        /// <summary>
        /// function to fetch MOCK location data from JSON file
        /// </summary>
        /// <returns> ClusterAllPins object, which has the cluster attributes and all of the location points in the cluster </returns>
        public ClusterAllPins loadJsonFileToList()
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("AnorocMobileApp.Points.json");
                string text = string.Empty;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                var resultObject = JsonConvert.DeserializeObject<ClusterAllPins>(text);
                return resultObject;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

    }
}
