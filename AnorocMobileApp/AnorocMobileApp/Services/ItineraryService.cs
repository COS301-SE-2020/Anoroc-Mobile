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

        // TODO: 
        // Make a max for the number of itineraries that we have in memory
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
                        var itineraruRisk = JsonConvert.DeserializeObject<ItineraryRisk>(json);

                        if(itineraruRisk != null)
                            UserItineraries.Add(itineraruRisk);
                    }

                    if (UserItineraries.Count > 0)
                        return UserItineraries;
                    else
                        return null;
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnecToItineraryServiceException();
                }
            }
        }

        public async Task<ItineraryRisk> ProcessItinerary(Itinerary userItinerary)
        {
            
            using (Anoroc_Client = new HttpClient(clientHandler))
            {
                if (UserItineraries == null)
                {
                    UserItineraries = new List<ItineraryRisk>();
                }

                Anoroc_Client.Timeout = TimeSpan.FromSeconds(30);

                Uri Anoroc_Uri = new Uri(Constants.AnorocURI + "Itinerary/ProcessItinerary");
                Token token_object = new Token();

                token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];

                token_object.Object_To_Server = JsonConvert.SerializeObject(userItinerary);

                var data = JsonConvert.SerializeObject(token_object);

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage responseMessage;

                try
                {
                    responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, content);
                    ItineraryRisk itineraryRisk = null;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        itineraryRisk = JsonConvert.DeserializeObject<ItineraryRisk>(json);
                        UserItineraries.Add(itineraryRisk);
                    }
                    return itineraryRisk;
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    return null;
                }
            }
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

            int size;
            if (UserItineraries.Count < Pagination)
                size = UserItineraries.Count;
            else
                size = Pagination;

            for (int i = 0; i < size; i++)
            {
                itineraryRisks.Add(UserItineraries.ElementAt(i));
            }

            UserItineraries.Clear();
            UserItineraries = itineraryRisks;
        }
    }
}
