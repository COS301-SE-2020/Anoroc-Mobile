using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnorocMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using AnorocMobileApp.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace AnorocMobileApp.Services.Tests
{
    [TestClass()]
    public class Intergrationtestapptoapi
    {
        [TestMethod()]
        public void Send_Locaiton_ServerAsyncTestAsync()
        {
            Task.Run(async () =>
            {
                Token test_object = new Token();
                test_object.access_token = "thisisatoken";
                test_object.Object_To_Server = JsonConvert.SerializeObject(new Location(new GEOCoordinate(27.9393, -25.9923), DateTime.Now, false));
                test_object.error_descriptions = "Integration Testing";

                string url = "https://localhost:5001/location/GEOLocation";

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                try
                {
                    using (HttpClient client = new HttpClient(clientHandler))
                    {

                        client.Timeout = TimeSpan.FromSeconds(30);


                        var data = JsonConvert.SerializeObject(test_object);

                        Debug.WriteLine(data);

                        var StringConent = new StringContent(data, Encoding.UTF8, "application/json");
                        StringConent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = new HttpResponseMessage();

                        try
                        {
                            response = await client.PostAsync(url, StringConent);
                        }
                        catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                        {
                            Assert.AreEqual(e.Message, "No connection could be made because the target machine actively refused it.");
                        }


                        string result = response.Content.ReadAsStringAsync().Result;

                        //await DisplayAlert("Attention", "Enabled: " + result, "OK");
                        HttpResponseMessage responseMessage = JsonConvert.DeserializeObject<HttpResponseMessage>(result);

                        Assert.AreEqual(responseMessage.StatusCode, System.Net.HttpStatusCode.OK);
                    }

                }
                catch (Exception e)
                {
                    Assert.Fail();
                }
            }).GetAwaiter().GetResult();
        }

        [TestMethod()]
        public void Send_LocationFailTest()
        {
            Assert.ThrowsException<AssertFailedException>(() => Task.Run(async () =>
            {
                Token test_object = new Token();
                test_object.access_token = "thisisatoken";
                test_object.Object_To_Server = JsonConvert.SerializeObject(new Location(new GEOCoordinate(27.9393, -25.9923), DateTime.Now, false));
                test_object.error_descriptions = "Integration Testing";

                string url = "https://location/GEOLocation";

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                try
                {
                    using (HttpClient client = new HttpClient(clientHandler))
                    {

                        client.Timeout = TimeSpan.FromSeconds(30);


                        var data = JsonConvert.SerializeObject(test_object);

                        Debug.WriteLine(data);

                        var StringConent = new StringContent(data, Encoding.UTF8, "application/json");
                        StringConent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = new HttpResponseMessage();

                        try
                        {
                            response = await client.PostAsync(url, StringConent);
                        }
                        catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                        {
                            Assert.AreEqual(e.Message, "No connection could be made because the target machine actively refused it.");
                        }


                        string result = response.Content.ReadAsStringAsync().Result;

                        //await DisplayAlert("Attention", "Enabled: " + result, "OK");
                        HttpResponseMessage responseMessage = JsonConvert.DeserializeObject<HttpResponseMessage>(result);
                    }

                }
                catch (Exception e)
                {
                    Assert.Fail();
                }
            }).GetAwaiter().GetResult());
        }
    }
}