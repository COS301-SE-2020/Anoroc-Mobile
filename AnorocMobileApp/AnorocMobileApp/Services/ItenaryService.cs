using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AnorocMobileApp.Helpers;
using Newtonsoft.Json;

namespace AnorocMobileApp.Services
{
    public class ItenaryService
    {
        public ItenaryService()
        {
        }

        /// <summary>
        /// Function to send Itenary object to server
        /// </summary>
        /// <param name="itenary">Itenary object</param>
        /// 
        public async void sendItenaryObjecct(string itenary)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(clientHandler);

            //HttpClientHandler clientHandler = new HttpClientHandler();
            var url = Secrets.baseEndpoint + Secrets.carrierStatusEndpoint;

            /*var token_object = new Token();
            token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];
            token_object.Object_To_Server = value;*/

           
            var data = JsonConvert.SerializeObject(itenary);

            var c = new StringContent(data, Encoding.UTF8, "application/json");
            c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            try
            {
                //var response = await client.PostAsync(url, c);
                //string result = response.Content.ReadAsStringAsync().Result;
                //Debug.WriteLine(result);
            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {
                throw new CantConnectToLocationServerException();
            }

        }
    }
}


 