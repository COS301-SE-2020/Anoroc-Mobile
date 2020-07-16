using AnorocMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<Cluster>> GetClustersForCirclesAsync()
        {
            string json = "";
            HttpContent content = new StringContent("{\"Area\":{\"HandShake\":\"Hello\"}}", Encoding.UTF8, "application/json");
            Uri Anoroc_Uri = new Uri("https://10.0.2.2:5001/location/Clusters/Simplified");
            HttpResponseMessage responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, content);

            List<Cluster> clusters = new List<Cluster>();

            if (responseMessage.IsSuccessStatusCode)
            {
                json = await responseMessage.Content.ReadAsStringAsync();
                clusters = JsonConvert.DeserializeObject<List<Cluster>>(json);
            }

            return clusters;
        }

        public async Task<List<ClusterAllPins>> FetchClustersAsync()
        {
            string json = "";
   

            Uri Anoroc_Uri = new Uri("https://10.0.2.2:5001/location/Clusters/Pins");
            HttpResponseMessage responseMessage = await Anoroc_Client.GetAsync(Anoroc_Uri);
            List<ClusterAllPins> clusters = new List<ClusterAllPins>();
            if(responseMessage.IsSuccessStatusCode)
            {
                json = await responseMessage.Content.ReadAsStringAsync();
                clusters = JsonConvert.DeserializeObject<List<ClusterAllPins>>(json);
            }
            return clusters;
        }
    }
}
