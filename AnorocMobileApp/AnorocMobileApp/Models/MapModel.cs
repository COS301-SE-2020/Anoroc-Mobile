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

namespace AnorocMobileApp.Models
{
    public class MapModel
    {
        public MapModel()
        {
        }
        public Points loadJsonFileToList()
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

                var resultObject = JsonConvert.DeserializeObject<Points>(text);
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
