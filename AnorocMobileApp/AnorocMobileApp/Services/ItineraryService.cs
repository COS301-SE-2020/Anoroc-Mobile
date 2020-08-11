using AnorocMobileApp.Exceptions;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Itinerary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnorocMobileApp.Services
{
    class ItineraryService : IItineraryService
    {
        HttpClient Anoroc_Client;
        HttpClientHandler clientHandler = new HttpClientHandler();

        public ItineraryService()
        {
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Anoroc_Client = new HttpClient(clientHandler);
        }
        public static int Pagination { get; private set; }
        public List<ItineraryRisk> UserItineraries { get; private set; }
        public void Clear()
        {
            Pagination = 10;
            if (UserItineraries != null)
            {
                UserItineraries.ForEach(_userItineraries =>
                {
                    _userItineraries.LocationItineraryRisks.Clear();
                });
                UserItineraries.Clear();
            }
        }

        public async Task<List<ItineraryRisk>> GetUserItineraries()
        {
            using (Anoroc_Client = new HttpClient(clientHandler))
            {
                if(UserItineraries == null)
                {
                    UserItineraries = new List<ItineraryRisk>();
                }

                Anoroc_Client.Timeout = TimeSpan.FromSeconds(30);

                Uri Anoroc_Uri = new Uri(Constants.AnorocURI + "Itinerary/GetUserItinerary");
                Token token_object = new Token();

                token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];

                token_object.Object_To_Server = Pagination.ToString();

                var data = JsonConvert.SerializeObject(token_object);

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage responseMessage;

                try
                {
                    responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, content);
                  
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        UserItineraries.Add(JsonConvert.DeserializeObject<ItineraryRisk>(json));
                    }
                    return UserItineraries;
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnecToItineraryServiceException();
                }
            }
        }

        public void ProcessItinerary(Itinerary userItinerary)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItineraryRisk>> LoadMore()
        {
            Pagination += 10;
            return await GetUserItineraries();
        }

        public void Reset()
        {
            Pagination = 10;
            List<ItineraryRisk> itineraryRisks = new List<ItineraryRisk>();
            
            for(int i = 0; i < Pagination; i++)
            {
                itineraryRisks.Add(UserItineraries.ElementAt(i));
            }

            UserItineraries.Clear();
            UserItineraries = itineraryRisks;
        }
    }
}
