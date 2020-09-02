using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnorocMobileApp.Exceptions;
using AnorocMobileApp.Helpers;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AnorocMobileApp.Services
{
    public class UserManagementService : IUserManagementService
    {
        HttpClient Anoroc_Client;
        HttpClientHandler clientHandler = new HttpClientHandler();
        public static bool ServiceRunning { get; private set; }
        public UserManagementService()
        {
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            ServiceRunning = false;
        }

        public void StopUserManagementService()
        {
            ServiceRunning = false;
            var message = new StopBackgroundUserManagementService();
            MessagingCenter.Send(message, "StopBackgroundUserManagementService");
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


                Uri Anoroc_Uri = new Uri(Secrets.baseEndpoint + Secrets.sendFireBaseTokenEndpoint);
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
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(clientHandler);

            //HttpClientHandler clientHandler = new HttpClientHandler();
            var url = "https://10.0.2.2:5001/" + Secrets.carrierStatusEndpoint;

            var token_object = new Token();
            token_object.access_token = (string)Xamarin.Forms.Application.Current.Properties["TOKEN"];
            token_object.Object_To_Server = value;

            /*var status = value == "Positive";
            var carrierStatus = new CarrierStatus((string)Xamarin.Forms.Application.Current.Properties["TOKEN"], status);*/

            var data = JsonConvert.SerializeObject(token_object);

            var c = new StringContent(data, Encoding.UTF8, "application/json");
            c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            

            try
            {
                var response = await client.PostAsync(url, c);
                //string result = response.Content.ReadAsStringAsync().Result;
                //Debug.WriteLine(result);
            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {                
                throw new CantConnectToLocationServerException();
            }

        }

        public async void UserLoggedIn(string firstName, string surname, string userEmail)
        {
            using (Anoroc_Client = new HttpClient(clientHandler))
            {
                Token token_object = new Token();
                token_object.access_token = "expectingtoken";
                User.Email = userEmail;
                User.FirstName = firstName;
                User.UserSurname = surname;
                User.currentlyLoggedIn = true;
                token_object.Object_To_Server = User.toString(); ;

                var data = JsonConvert.SerializeObject(token_object);

                var stringcontent = new StringContent(data, Encoding.UTF8, "application/json");
                stringcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                Uri Anoroc_Uri = new Uri(Secrets.baseEndpoint + Secrets.UserLoggedInEndpoint);
                HttpResponseMessage responseMessage;

                try
                {
                    responseMessage = await Anoroc_Client.PostAsync(Anoroc_Uri, stringcontent);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        if (json != null)
                        {
                            Application.Current.Properties["TOKEN"] = json;
                            string firebaseToken = (string)Application.Current.Properties["FirebaseToken"];
                            IUserManagementService ims = App.IoCContainer.GetInstance<IUserManagementService>();
                            ims.SendFireBaseToken(firebaseToken);

                            //notify all listeners of successfull login
                            var message = new UserLoggedIn();
                            MessagingCenter.Send(message, "UserLoggedIn");
                        }
                    }
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnecToClusterServiceException();
                }
            }
        }

        public async Task<MemoryStream> GetUserProfileImage()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            var client = new HttpClient(clientHandler);
            Token token_object = new Token();
            token_object.access_token = (string)Application.Current.Properties["TOKEN"];
            token_object.Object_To_Server = "";

            var data = JsonConvert.SerializeObject(token_object);

            var stringcontent = new StringContent(data, Encoding.UTF8, "application/json");
            stringcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            Uri Anoroc_Uri = new Uri(Secrets.baseEndpoint + Secrets.GetUserProfileImageEndpoint);
            HttpResponseMessage responseMessage;

            try
            {
                responseMessage = await client.PostAsync(Anoroc_Uri, stringcontent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var profileImage = await responseMessage.Content.ReadAsByteArrayAsync();

                    var ms = new MemoryStream(profileImage);

                    //TODO: Save to SQLite database

                    return ms;
                }
                else
                    return null;

            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {
                throw new CantConnecToClusterServiceException();
            }
        }

        public async void UploadUserProfileImage(byte[] image)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            var client = new HttpClient(clientHandler);
            Token token_object = new Token();
            token_object.access_token = (string)Application.Current.Properties["TOKEN"];
            token_object.Object_To_Server = "";
            token_object.Profile_image = image;

            var data = JsonConvert.SerializeObject(token_object);

            var stringcontent = new StringContent(data, Encoding.UTF8, "application/json");
            stringcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            Uri Anoroc_Uri = new Uri(Secrets.baseEndpoint + Secrets.UploadUserProfileImage);
            HttpResponseMessage responseMessage;

            try
            {
                responseMessage = await client.PostAsync(Anoroc_Uri, stringcontent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    //TODO:
                    //Uploaded the profile image
                }

            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {
                throw new CantConnecToClusterServiceException();
            }
        }

        public async Task<int> UpdatedIncidents()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            var client = new HttpClient(clientHandler);
                Token token_object = new Token();
                token_object.access_token = (string)Application.Current.Properties["TOKEN"];
                token_object.Object_To_Server = "";

                var data = JsonConvert.SerializeObject(token_object);

                var stringcontent = new StringContent(data, Encoding.UTF8, "application/json");
                stringcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


                Uri Anoroc_Uri = new Uri(Secrets.baseEndpoint + Secrets.UserIncidentsEndpoint);
                HttpResponseMessage responseMessage;

                try
                {
                    responseMessage = await client.PostAsync(Anoroc_Uri, stringcontent);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        var incidents = Convert.ToInt32(json);
                        return incidents;
                    }
                    else
                        return 0;
                    
                }
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    throw new CantConnecToClusterServiceException();
                }
        }

        public async void CheckIncidents()
        {
            ServiceRunning = true;
            await Task.Run(async () =>
            {
                while (ServiceRunning)
                {
                    var message = new CheckUserIncidents();
                    MessagingCenter.Send(message, "CheckUserIncidents");

                    await Task.Delay(1000000);
                }
            }, CancellationToken.None);
        }
    }
}
