using AnorocMobileApp.Exceptions;
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
            using (Anoroc_Client = new HttpClient(clientHandler))
            {
                Anoroc_Client.Timeout = TimeSpan.FromSeconds(30);

                Token token_object = new Token();
                token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];

                //MOCK AREA OBJECT
                token_object.Object_To_Server = "{\"Area\":{\"HandShake\":\"Hello\"}}";

                var data = JsonConvert.SerializeObject(token_object);

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


                Uri Anoroc_Uri = new Uri("https://10.0.2.2:5001/location/Clusters/Simplified");
                HttpResponseMessage responseMessage;

                try
                {
                    responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, content);
                    List<Cluster> clusters = new List<Cluster>();

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        json = await responseMessage.Content.ReadAsStringAsync();
                        clusters = JsonConvert.DeserializeObject<List<Cluster>>(json);
                    }

                    return clusters;
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnecToClusterServiceException();
                }
            }
        }

        public async Task<List<ClusterAllPins>> FetchClustersAsync()
        {
            string json = "";

            using (Anoroc_Client = new HttpClient(clientHandler))
            {

                Anoroc_Client.Timeout = TimeSpan.FromSeconds(30);

                Uri Anoroc_Uri = new Uri("https://10.0.2.2:5001/location/Clusters/Pins");
                Token token_object = new Token();
                token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];

                //MOCK AREA OBJECT
                token_object.Object_To_Server = "{\"Area\":{\"HandShake\":\"Hello\"}}";
                var data = JsonConvert.SerializeObject(token_object);

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage responseMessage;
                try
                {
                    responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, content);
                    List<ClusterAllPins> clusters = new List<ClusterAllPins>();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        json = await responseMessage.Content.ReadAsStringAsync();
                        clusters = JsonConvert.DeserializeObject<List<ClusterAllPins>>(json);
                    }
                    return clusters;
                }
                catch(Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnecToClusterServiceException();
                }
            }
        }
    }
}
