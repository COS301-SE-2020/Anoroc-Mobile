using System;
using System.ComponentModel;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.ViewModels.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using AnorocMobileApp.Interfaces;


namespace AnorocMobileApp.Views.Navigation
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel viewModel = new SettingsViewModel();
        public SettingsPage()
        {
            var status = new Label();
            status.SetBinding(Label.TextProperty, new Binding("SelectedItem", source: status));

            InitializeComponent();

            //if (Application.Current.Properties.ContainsKey("CarrierStatus"))
            //    DisplayAlert("Carrier Status", (string)Application.Current.Properties["CarrierStatus"], "Cancel");

            var request = new GeolocationRequest(GeolocationAccuracy.Lowest);

            if (Application.Current.Properties.ContainsKey("Tracking"))
            {
                var value = (bool)Application.Current.Properties["Tracking"];
                if (value)
                    LocationSwitch.IsEnabled = true;
            }

            if (Application.Current.Properties.ContainsKey("CarrierStatus"))
            {
                var value = Application.Current.Properties["CarrierStatus"].ToString();
                if (value == "Positive")
                    picker.SelectedIndex = 0;
                else
                    picker.SelectedIndex = 1;
            }
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
                DependencyService.Get<NotificationServices>().CreateNotification("Anoroc", msg);
            });
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs args)
        {

            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                string value = (string)picker.ItemsSource[selectedIndex];
                //DisplayAlert("Carrier Status", value, "OK");
                Application.Current.Properties["CarrierStatus"] = value;

                if (value == "Positive")
                    User.carrierStatus = true;
                else
                    User.carrierStatus = false;

                Container.userManagementService.sendCarrierStatusAsync(value);
            }
        }


        async void SfSwitch_StateChanged(System.Object sender, Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs e)
        {
            if (e.NewValue == true)
            {
                BackgroundLocaitonService.Tracking = true;
                //Container.BackgroundLocationService.Start_Tracking();
            }
            else
            {
                BackgroundLocaitonService.Tracking = false;
                Container.BackgroundLocationService.Stop_Tracking();
                await DisplayAlert("Attention", "Disabled", "OK");
            }

        }

    }
}