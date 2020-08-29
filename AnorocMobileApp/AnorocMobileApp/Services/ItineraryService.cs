using AnorocMobileApp.Exceptions;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Itinerary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Essentials;

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

        // TODO:
        // Checker if the itinerary is already in local var
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
                        UserItineraries = JsonConvert.DeserializeObject<List<ItineraryRisk>>(json);
                    }

                    if (UserItineraries == null)
                        return null;
                    else if (UserItineraries.Count > 0)
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
        private int retryCount = 0;
        public async Task<ItineraryRisk> ProcessItinerary(Itinerary userItinerary)
        {
            if (retryCount == 5)
            {
                return null;
            }
            else
            {
                bool retry = false;
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

                    token_object.Object_To_Server = JsonConvert.SerializeObject(new ItineraryWrap(userItinerary));

                    var data = JsonConvert.SerializeObject(token_object);

                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    HttpResponseMessage responseMessage;

                    try
                    {

                        responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, content);
                        ItineraryRiskWrapper itineraryRisk = null;
                        if (responseMessage != null)
                        {
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                var json = await responseMessage.Content.ReadAsStringAsync();
                                itineraryRisk = JsonConvert.DeserializeObject<ItineraryRiskWrapper>(json);
                                var convertedItinerary = itineraryRisk.toItineraryRisk();
                                saveItineraryRisk(convertedItinerary);
                                UserItineraries.Add(convertedItinerary);
                            }
                            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                retry = true;
                            }

                            if (!retry)
                                return itineraryRisk.toItineraryRisk();
                            else
                            {
                                retryCount++;
                                return await ProcessItinerary(userItinerary);
                            }
                        }
                        else
                            return null;
                    }
                    catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                    {
                        return null;
                    }
                }
            }
        }

        public List<ItineraryRisk> ItinerariesFromLocal()
        {
            var returnList = new List<ItineraryRisk>();
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.FilePath))
            {
                try
                {
                    var primitiveList = conn.Table<PrimitiveItineraryRisk>().ToList();
                    primitiveList.ForEach(primitive =>
                    {
                        returnList.Add(new ItineraryRisk(primitive));
                    });
                }
                catch (SQLiteException exception)
                {
                    Debug.WriteLine(exception.Message);
                }

            }
            return returnList;
        }

        private void saveItineraryRisk(ItineraryRisk risk)
        {
            var primitiveRisk = new PrimitiveItineraryRisk(risk);
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<PrimitiveItineraryRisk>();

                int rowsAdded = conn.Insert(primitiveRisk);
                if (rowsAdded > 0)
                {
                    Debug.WriteLine("Inserted Itinerary");
                }
                else
                {
                    Debug.WriteLine("Failed to Insert Itinerary");
                }
                conn.Close();
            }
            var myvar = ItinerariesFromLocal();
        }

        public async Task<List<ItineraryRisk>> LoadMore()
        {
            Pagination += 10;
            return await GetUserItineraries();
        }

        // TODO:
        // Put this logic in the Model for the itinerary
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

        public async Task<List<ItineraryRisk>> GetAllUserItineraries()
        {
            using (Anoroc_Client = new HttpClient(clientHandler))
            {
                if (UserItineraries == null)
                {
                    UserItineraries = new List<ItineraryRisk>();
                }

                Anoroc_Client.Timeout = TimeSpan.FromSeconds(30);

                Uri Anoroc_Uri = new Uri(Constants.AnorocURI + "Itinerary/GetUserItinerary");
                Token token_object = new Token();

                token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];

                token_object.Object_To_Server = Convert.ToString(-1);

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
                        UserItineraries = JsonConvert.DeserializeObject<List<ItineraryRisk>>(json);
                    }

                    if (UserItineraries == null)
                        return null;
                    else if (UserItineraries.Count > 0)
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
    }
}
