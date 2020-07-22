using AnorocMobileApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Home
{
    /// <summary>
    /// Home page for the app to display a summary of information
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage" /> class.
        /// </summary>
        public HomePage()
        {
            InitializeComponent();
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