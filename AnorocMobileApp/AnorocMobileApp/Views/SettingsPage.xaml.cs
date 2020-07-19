using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Essentials;
using AnorocMobileApp.Services;
using AnorocMobileApp.Interfaces;
using System.Diagnostics;
using AnorocMobileApp.Models;
using AnorocMobileApp.ViewModels;

namespace AnorocMobileApp.Views
{

    /// <summary>
    /// Class to manage the Settings Paged
    /// </summary>
    public partial class SettingsPage : ContentPage
    {

        /// <summary>
        /// Initializes the settings Screen
        /// </summary>
        /// 
        
        public SettingsPage()
        {
            var status = new Label();
            status.SetBinding(Label.TextProperty, new Binding("SelectedItem", source: status));

            InitializeComponent();
            DisplayAlert("Carrier Status", User.carrierStatus, "Cancel");

            var request = new GeolocationRequest(GeolocationAccuracy.Lowest);
           
            if (Application.Current.Properties.ContainsKey("Tracking"))
            {
                var value = (bool)Application.Current.Properties["Tracking"];
                if (value)
                    Location_Tracking_Switch.IsToggled = true;
            }

        }       

        
        /// <summary>
        /// Function to toggle Asynchronous location, when off
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Toggled Event Arguments</param>
        /// 
        async void OnToggledAsync(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                
                BackgroundLocaitonService.Tracking = true;
                Container.BackgroundLocationService.Start_Tracking();               
            }
            else
            {
                BackgroundLocaitonService.Tracking = false;
                Container.BackgroundLocationService.Stop_Tracking();
                await DisplayAlert("Attention", "Disabled", "OK");
            }
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                string value = (string)picker.ItemsSource[selectedIndex];
                DisplayAlert("Carrier Status", value, "OK");
                Application.Current.Properties["CarrierStatus"] = value;
                Container.userManagementService.sendCarrierStatusAsync(value);

                //status.Text = (string)picker.ItemsSource[selectedIndex];
            }
        }
    }
}
