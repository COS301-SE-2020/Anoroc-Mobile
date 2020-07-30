using AnorocMobileApp.DataService;
using AnorocMobileApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Notification
{
    /// <summary>
    /// Page to show the health care details.
    /// </summary>
    [Preserve(AllMembers = true)]
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
    }
}
