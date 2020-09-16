using AnorocMobileApp.DataService;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.ViewModels.Notification;
using SQLite;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Notification
{
    /// <summary>
    /// Page to show the health care details.
    /// </summary>
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        public NotificationViewModel notificationViewModel;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationPage" /> class.
        /// </summary>
        public NotificationPage()
        {
            InitializeComponent();
            //this.BindingContext = NotificationDataService.Instance.NotificationViewModel;          
            var data = new EncounterDataService();
            this.BindingContext = data.NotificationViewModel;
            this.notificationViewModel = data.NotificationViewModel;
            MessagingCenter.Subscribe<object, string[]>(this, App.NotificationBodyReceivedKey, OnMessageReceived);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
                        
            MessagingCenter.Subscribe<object, string[]>(this, App.NotificationBodyReceivedKey, OnMessageReceived);

        }

        void OnMessageReceived(object sender, string[] msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                DependencyService.Get<NotificationServices>().CreateNotification(msg[0], msg[1]);
            });
        }

    }
}
