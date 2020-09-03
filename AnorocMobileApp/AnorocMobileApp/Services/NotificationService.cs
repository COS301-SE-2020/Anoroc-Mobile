using AnorocMobileApp.Models;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AnorocMobileApp.Services
{
    class NotificationService
    {

        private String AccessToken;
        HttpClient Anoroc_Client;
        HttpClientHandler clientHandler = new HttpClientHandler();
        public async Task test()
        {

            AccessToken = CrossSecureStorage.Current.GetValue("APIKEY");

            using (Anoroc_Client = new HttpClient(clientHandler))
            {
                var defaultRequestHeaders = Anoroc_Client.DefaultRequestHeaders;

                if (defaultRequestHeaders.Accept == null ||
                   !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    Anoroc_Client.DefaultRequestHeaders.Accept.Add(new
                      MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("bearer", AccessToken);

                HttpResponseMessage responseMessage;

                try
                {
                    responseMessage = await Anoroc_Client.GetAsync(B2CConstants.BaseAddress);

                    if (responseMessage.IsSuccessStatusCode)
                    {

                        Console.ForegroundColor = ConsoleColor.Green;
                        string json = await responseMessage.Content.ReadAsStringAsync();
                        Console.WriteLine(json);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Failed to call the Web Api: {responseMessage.StatusCode}");
                        string content = await responseMessage.Content.ReadAsStringAsync();
                        Console.WriteLine($"Content: {content}");
                    }
                    Console.ResetColor();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Failed", e);
                }
               
            }
        }
    }
}
