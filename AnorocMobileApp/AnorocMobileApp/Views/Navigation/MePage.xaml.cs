using System;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using Plugin.SecureStorage;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
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
        public MePage()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("CarrierStatus"))
            {
                var value = Application.Current.Properties["CarrierStatus"].ToString();
                if (value == "Positive")
                    picker.SelectedIndex = 0;
                else
                    picker.SelectedIndex = 1;
            }

            MessagingCenter.Subscribe<CheckUserIncidents>(this, "CheckUserIncidents", message =>
            {
                UpdatedIncidentNumner();
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var name = CrossSecureStorage.Current.GetValue("Name");
            var surname = CrossSecureStorage.Current.GetValue("Surname");

            var location = CrossSecureStorage.Current.GetValue("Location");
            if(location != null)
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
            Navigation.PushModalAsync(new Notification.NotificationPage());
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
    }
}