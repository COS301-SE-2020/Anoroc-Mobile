using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AnorocMobileApp.Views
{
    public partial class NotificationPage : ContentPage
    {
        public NotificationPage()
        {
            InitializeComponent();
        }

        private void SendNotification(object sender, EventArgs e)
        {
            
            getRequestAsync();
        }

        public async void getRequestAsync()
        {
     
            var url = "https://10.0.2.2:5001/notification/all";
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);

            var response = await client.GetAsync(url);


            string result = response.Content.ReadAsStringAsync().Result;

        
           DependencyService.Get<INotification>().CreateNotification("Anoroc", result);
            

          

        }

    }
}