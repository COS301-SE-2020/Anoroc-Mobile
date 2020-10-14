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
using Microsoft.Identity.Client;
using AnorocMobileApp.Views.Forms;
using AnorocMobileApp.Helpers;
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
            if(Application.Current.Properties.ContainsKey("isAnonymous"))
            {
                var anon = Application.Current.Properties["isAnonymous"];
                if (anon.Equals("True"))
                {
                    Anonymity_SfSwitch.IsOn = true;
                }
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            var signedIn = CrossSecureStorage.Current.GetValue("SignedIn");

            if(signedIn != null && signedIn.ToString().Equals("True"))
            {
                btnSignOut.IsVisible = true;
            }
            else
            {
                btnSignOut.IsVisible = false;
            }


        }

        private void SignOutButton_Clicked(object sender, EventArgs e)
        {
            OnSignOut(sender, e);
        }
        async void OnSignOut(object sender, EventArgs e)
        {


            try
            {



                var userContext = await B2CAuthenticationService.Instance.SignOutAsync();
           

                if (!userContext.IsLoggedOn)
                {
                    UpdateSignInState(userContext);
                    Application.Current.MainPage = new LoginWithSocialIconPage();

                }
                else
                {
                    CrossToastPopUp.Current.ShowToastMessage("Sign out failed");

                }

            }
            catch (Exception ex)
            {
                // Checking the exception message 
                // should ONLY be done for B2C
                // reset and not any other error.
                if (ex.Message.Contains("AADB2C90118"))
                    OnPasswordReset();
                // Alert if any exception excluding user canceling sign-in dialog
                else if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }
        async void OnPasswordReset()
        {
            try
            {
                var userContext = await B2CAuthenticationService.Instance.ResetPasswordAsync();
                UpdateSignInState(userContext);
            }
            catch (Exception ex)
            {
                // Alert if any exception excluding user canceling sign-in dialog
                if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }
        void UpdateSignInState(UserContext userContext)
        {
            var isSignedIn = userContext.IsLoggedOn;

            CrossSecureStorage.Current.SetValue("SignedIn", isSignedIn.ToString());
            CrossSecureStorage.Current.SetValue("SignedInFirstTime", "false");
            CrossSecureStorage.Current.SetValue("APIKEY","thisisatoken");
            CrossSecureStorage.Current.SetValue("Name", "");
            CrossSecureStorage.Current.SetValue("Surname", "");
            CrossSecureStorage.Current.SetValue("Email", "");

            //btnSignInSignOut.Text = isSignedIn ? "Sign out" : "Sign in";

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
                CrossToastPopUp.Current.ShowToastMessage("Location Tracking Enabled");

            }
            else
            {
                //BackgroundLocaitonService.Tracking = false;                               
                back.Stop_Tracking();

                //await DisplayAlert("Attention", "Disabled", "OK");
                CrossToastPopUp.Current.ShowToastMessage("Location Tracking Disabled");
                CrossSecureStorage.Current.SetValue("Location", "false");
            }

        }
        async void SfSwitch_Anonymity_StateChanged(System.Object ssender, Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs e)
        {
            var user = App.IoCContainer.GetInstance<IUserManagementService>();
            var response = await user.ToggleAnonymousUser((bool)e.NewValue);
            CrossToastPopUp.Current.ShowToastMessage("Anonomity set to: " + response);
            App.Current.Properties["isAnonymous"] = response;
        }

        private async void btnDownloadUserData_Clicked(object sender, EventArgs e)
        {
            var user = App.IoCContainer.GetInstance<IUserManagementService>();
            var userdata = await user.DownloadData();
            if (userdata)
            {
                CrossToastPopUp.Current.ShowToastMessage("Email Sent.");
            }
            else
            {
                CrossToastPopUp.Current.ShowToastMessage("Error Generating the file.");
            }
        }

        private void btnDontSendLocation_Clicked(object sender, EventArgs e)
        {
            var user = App.IoCContainer.GetInstance<ILocationService>();
            user.DontSendCurrentLocationAnymoreAsync();
            CrossToastPopUp.Current.ShowToastMessage("Location not longer being tracked.");
        }
    }
}