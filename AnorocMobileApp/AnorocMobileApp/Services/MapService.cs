using AnorocMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AnorocMobileApp.Services
{
    class MapService
    {
        HttpClient Anoroc_Client;
        HttpClientHandler clientHandler = new HttpClientHandler();
        
        public MapService()
        {
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Anoroc_Client = new HttpClient(clientHandler);
        }

        public async System.Threading.Tasks.Task<List<Clusters>> FetchClustersAsync()
        {
            string json = "";
   

            Uri Anoroc_Uri = new Uri("https://10.0.2.2:5001/location/Clusters");
            HttpResponseMessage responseMessage = await Anoroc_Client.GetAsync(Anoroc_Uri);
            List<Clusters> clusters = new List<Clusters>();
            if(responseMessage.IsSuccessStatusCode)
            {
                json = await responseMessage.Content.ReadAsStringAsync();
                clusters = JsonConvert.DeserializeObject<List<Clusters>>(json);
            }
            return clusters;
        }
    }
}
