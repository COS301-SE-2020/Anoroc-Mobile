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
using Plugin.Toast;
using Plugin.SecureStorage;
//using Container = AnorocMobileApp.Interfaces.Container;

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

            //if (Application.Current.Properties.ContainsKey("Tracking"))
            if (BackgroundLocationService.Tracking)
            {
                Locations_SfSwitch.IsOn = true;
                
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<object, string>(this, App.NotificationBodyReceivedKey, OnMessageReceived);

        }

        void OnMessageReceived(object sender, string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                DependencyService.Get<NotificationServices>().CreateNotification("Anoroc", msg);
            });
        }

        /// <summary>
        /// Enabels and disables location tracking
        /// </summary>
        async void SfSwitch_StateChanged(System.Object sender, Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs e)
        {
            IBackgroundLocationService back = App.IoCContainer.GetInstance<IBackgroundLocationService>();
            if (e.NewValue == true)
            {
                //BackgroundLocaitonService.Tracking = true;
                back.Start_Tracking();
                CrossSecureStorage.Current.SetValue("Location", "true");

            }
            else
            {
                //BackgroundLocaitonService.Tracking = false;                               
                back.Stop_Tracking();

                //await DisplayAlert("Attention", "Disabled", "OK");
                CrossToastPopUp.Current.ShowToastMessage("Lacation Tracking Disabled");
                CrossSecureStorage.Current.SetValue("Location", "false");
            }

        }

    }
}