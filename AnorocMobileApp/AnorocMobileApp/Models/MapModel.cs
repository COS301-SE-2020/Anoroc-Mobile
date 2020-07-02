using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using AnorocMobileApp.Views;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using System.Diagnostics;
using AnorocMobileApp.Services;

namespace AnorocMobileApp.Models
{
    class MapModel
    {
        MapService Map_Service;
        public MapModel()
        {
            Map_Service = new MapService();
        }


        public async System.Threading.Tasks.Task<List<Clusters>> GetClustersAsync()
        {
          
            List<Clusters> clusters = await Map_Service.FetchClustersAsync();
            return clusters;
        }

        public Clusters loadJsonFileToList()
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

                var resultObject = JsonConvert.DeserializeObject<Clusters>(text);
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
