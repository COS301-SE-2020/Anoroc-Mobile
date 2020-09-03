using AnorocMobileApp.DataService;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
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
        

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationPage" /> class.
        /// </summary>
        public NotificationPage()
        {
            InitializeComponent();
            //this.BindingContext = NotificationDataService.Instance.NotificationViewModel;          
            this.BindingContext = EncounterDataService.Instance.NotificationViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            
            RefreshView rView = new RefreshView();
            ICommand refreshCommand = new Command(() =>
            {
                rView.IsRefreshing = false;
            });
            rView.Command = refreshCommand;
            MessagingCenter.Subscribe<object, string>(this, App.NotificationBodyReceivedKey, OnMessageReceived);
        }

        void OnMessageReceived(object sender, string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                DependencyService.Get<PushNotificationServices>().CreateNotification("Anoroc", msg);
            });
        }
    }
}
