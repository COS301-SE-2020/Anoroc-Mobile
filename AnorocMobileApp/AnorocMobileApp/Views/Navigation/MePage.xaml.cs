﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Models.Notification;
using AnorocMobileApp.Services;
using AnorocMobileApp.ViewModels.Navigation;
using AnorocMobileApp.Views.Notification;
using Plugin.SecureStorage;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Navigation
{
    /// <summary>
    /// Page to show the Me page.
    /// </summary>
    [SQLite.Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MePage : ContentPage
    {



        /// <summary>
        /// Initializes a new instance of the <see cref="MePage" /> class.
        /// </summary>
        /// 
        private string title = "";
        private string body = "";
        NotificationDB notificationDB = new NotificationDB();
        MeViewModel me = new MeViewModel();

        public MePage()
        {

            InitializeComponent();
            MessagingCenter.Subscribe<object, string>(this, App.NotificationOnMePageKey, OnBodyMessageReceived);
            //_ProfileImage.Source = ImageSource.FromUri(new Uri(Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "profilepicture.jpg")));
            if (Application.Current.Properties.ContainsKey("CarrierStatus"))
            {
                var value = Application.Current.Properties["CarrierStatus"].ToString();
                if (value == "Positive")
                    picker.SelectedIndex = 0;
                else
                    picker.SelectedIndex = 1;
            }
            else
                picker.SelectedIndex = 1;

            MessagingCenter.Subscribe<UserLoggedIn>(this, "UserLoggedIn", async message =>
             {
                 var ims = App.IoCContainer.GetInstance<IUserManagementService>();
                 var base64 = await ims.GetUserProfileImage();
                 if (base64 != "")
                 {
                     if (base64 != null)
                     {
                         var bytes = Convert.FromBase64String(base64);
                         MemoryStream ms = new MemoryStream(bytes);
                         ms.Position = 0;
                         MemoryStream otherstream = new MemoryStream();
                         ms.CopyTo(otherstream);
                         otherstream.Position = 0;

                         _ProfileImage.Source = ImageSource.FromStream(() => otherstream);
                     }
                 }
             });           
        }

        

        protected override void OnAppearing()
        {           
            base.OnAppearing();
            if (Application.Current.Properties.ContainsKey("TOKEN"))
            {
                UpdatedIncidentNumner();
            }
            var name = CrossSecureStorage.Current.GetValue("Name");
            var surname = CrossSecureStorage.Current.GetValue("Surname");

            try
            {
                var location = CrossSecureStorage.Current.GetValue("Location");
                if (location != null)
                {
                    if (location.Equals("true"))
                    {
                        locationStatus.Text = "Enabled";
                    }
                    else
                    {
                        locationStatus.Text = "Disabled";
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                locationStatus.Text = "Disabled";
            }
            
            profileName.Text = name.ToString() + " " + surname.ToString();

            var status_info = CrossSecureStorage.Current.GetValue("Carrier_status");
            if (status_info != null)
            {
                if (status_info.Equals("true"))
                {
                    carrier_status_info.Text = "Preventions";
                }
                else
                {
                    carrier_status_info.Text = "Incidents";
                }
            }            
        }


        
        /*
        public Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }
        */
        /// <summary>
        /// Goes to notifications view.
        /// TODO: Show notifications in an improved way
        /// </summary>
        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            //DisplayAlert("Alert", "Notifications", "OK");
            Navigation.PushAsync(new Notification.NotificationPage());
        }

        async void UpdatedIncidentNumner()
        {
            var ims = App.IoCContainer.GetInstance<IUserManagementService>();
            var theNumber = await ims.UpdatedIncidents();
            carrier_status_num.Text = theNumber.ToString();
        }

        /// <summary>
        /// When Carrier status is changed, Calls funtion to send status to server
        /// </summary>
        private void OnPickerSelectedIndexChanged(object sender, EventArgs args)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                string value = (string)picker.ItemsSource[selectedIndex];
                Application.Current.Properties["CarrierStatus"] = value;

                if (value == "Positive")
                {
                    User.carrierStatus = true;
                    CrossSecureStorage.Current.SetValue("Carrier_status", "true");
                }
                else
                {
                    User.carrierStatus = false;
                    CrossSecureStorage.Current.SetValue("Carrier_status", "false");
                }


                IUserManagementService user = App.IoCContainer.GetInstance<IUserManagementService>();
                user.sendCarrierStatusAsync(value);
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            MemoryStream copyStream = new MemoryStream();
            
            if (stream != null)
            {
                await stream.CopyToAsync(copyStream);
                copyStream.Position = 0;
                MemoryStream ms = new MemoryStream();
                await copyStream.CopyToAsync(ms);
                copyStream.Position = 0;

                _ProfileImage.Source = ImageSource.FromStream(()=> copyStream);

                var theString = _ProfileImage.Source.ToString();

                ms.Position = 0;
                var bytes = ms.ToArray();
                string base64 = System.Convert.ToBase64String(bytes);
                var ims = App.IoCContainer.GetInstance<IUserManagementService>();
                ims.UploadUserProfileImage(base64);
            }
        }

        void OnBodyMessageReceived(object sender, string msg)
        {
            Console.WriteLine(msg);
            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                me.loadNotifications();
            });
            
        }
    }
}