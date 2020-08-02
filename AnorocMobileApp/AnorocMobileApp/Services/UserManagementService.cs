using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AnorocMobileApp.Exceptions;
using AnorocMobileApp.Helpers;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AnorocMobileApp.Services
{
    public class UserManagementService : IUserManagementService
    {
        HttpClient Anoroc_Client;
        HttpClientHandler clientHandler = new HttpClientHandler();
        public UserManagementService()
        {
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        /// <summary>
        /// Function to send Fire base token
        /// </summary>
        /// <param name="firebasetoken">Fire base token</param>

        public async void SendFireBaseToken(string firebasetoken)
        {
            using (Anoroc_Client = new HttpClient(clientHandler))
            {
                Token token_object = new Token();
                token_object.access_token = (string)Application.Current.Properties["TOKEN"];
                token_object.Object_To_Server = firebasetoken;

                var data = JsonConvert.SerializeObject(token_object);

                var stringcontent = new StringContent(data, Encoding.UTF8, "application/json");
                stringcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


                Uri Anoroc_Uri = new Uri("https://10.0.2.2:5001/UserManagement/FirebaseToken");
                HttpResponseMessage responseMessage;

                try
                {
                    responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, stringcontent);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var json = await responseMessage.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnecToClusterServiceException();
                }
            }
        }


        /// <summary>
        /// Function to send Carrier status to server
        /// </summary>
        /// <param name="value">Carrier status</param>
        /// 
        public async void sendCarrierStatusAsync(string value)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient(clientHandler);

            //HttpClientHandler clientHandler = new HttpClientHandler();
            var url = Secrets.baseEndpoint + Secrets.carrierStatusEndpoint;

            var token_object = new Token();
            token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];
            token_object.Object_To_Server = value;

            var data = JsonConvert.SerializeObject(token_object);

            var c = new StringContent(data, Encoding.UTF8, "application/json");
            c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync(url, c);
            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {                
                throw new CantConnectToLocationServerException();
            }

        }
    }
}
