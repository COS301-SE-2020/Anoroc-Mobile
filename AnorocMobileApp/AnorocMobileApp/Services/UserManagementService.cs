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
            var url = Secrets.baseEndpoint + Secrets.carrierStatusEndpoint;

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

        public async Task<byte[]> GetUserProfileImage()
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

                    return profileImage;
                }
                else
                    return null;

            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {
                throw new CantConnecToClusterServiceException();
            }
        }

        public async void UploadUserProfileImage(Stream streamImage)
        {
            var image = ReadToEnd(streamImage);
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

        protected static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
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
