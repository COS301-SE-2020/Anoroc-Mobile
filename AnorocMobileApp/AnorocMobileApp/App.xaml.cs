using AnorocMobileApp.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Lowest);
           
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
