using AnorocMobileApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Navigation
{
    /// <summary>
    /// Page to show the Me page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MePage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MePage" /> class.
        /// </summary>
        public MePage()
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

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("Alert", "Notifications", "OK");         
        }
    }
}