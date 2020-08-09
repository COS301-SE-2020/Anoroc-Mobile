using System.Collections.ObjectModel;
using System.Net.Http;
using AnorocMobileApp.Models;

namespace AnorocMobileApp.Services
{
    public class PlacesService
    {
        private static HttpClient _httpClientInstance;
        public static HttpClient HttpClientInstance => _httpClientInstance ?? (_httpClientInstance = new HttpClient());
        
        
    }
}