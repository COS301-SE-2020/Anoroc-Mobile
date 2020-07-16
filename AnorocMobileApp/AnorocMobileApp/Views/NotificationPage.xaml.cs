using AnorocMobileApp.Services;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace AnorocMobileApp.Views
{
    /// <summary>
    /// Class used to manage Notifications page
    /// </summary>
    public partial class NotificationPage : ContentPage
    {
        public NotificationPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
            MessagingCenter.Subscribe<object, string>(this, App.NotificationReceivedKey, OnMessageReceived);

        }

        void OnMessageReceived(object sender, string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                lblMsg.Text = msg;
            });
        }

        /// <summary>
        /// Function to Send Notofication to user
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Event Arguments</param>
        private void SendNotification(object sender, EventArgs e)
        {
            getRequestAsync();
        }
        /// <summary>
        /// Asynchronous function that uses an HTTP Client Handler to retrieve notifications
        /// </summary>
        public async void getRequestAsync()
        {
            var url = "https://10.0.2.2:5001/notification/all";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);
            var response = await client.GetAsync(url);

            string result = response.Content.ReadAsStringAsync().Result;
            DependencyService.Get<NotificationServices>().CreateNotification("Anoroc", result);
        }

    }
}