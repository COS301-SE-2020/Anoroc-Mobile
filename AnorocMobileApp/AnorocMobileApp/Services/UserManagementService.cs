using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using Newtonsoft.Json;

namespace AnorocMobileApp.Services
{
    public class UserManagementService : IUserManagementService
    {
        public UserManagementService()
        {
        }

        public async void sendCarrierStatusAsync(string value)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);

            //HttpClientHandler clientHandler = new HttpClientHandler();
            string url = "https://10.0.2.2:5001/UserManagement/CarrierStatus";

            Token token_object = new Token();
            token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];
            token_object.Object_To_Server = value;

            var data = JsonConvert.SerializeObject(token_object);

            var c = new StringContent(data, Encoding.UTF8, "application/json");
            c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response;

            //using (HttpClient client = new HttpClient(clientHandler))
            //{
                try
                {
                    response = await client.PostAsync(url, c);
                    string result = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(result);
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    //throw new Exception();
                    throw new CantConnectToLocationServerException();

                }


            //}
        }
    }
}
