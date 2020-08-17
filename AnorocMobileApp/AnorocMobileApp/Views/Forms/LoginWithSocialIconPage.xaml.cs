using System;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Navigation;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Forms
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    //[Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginWithSocialIconPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWithSocialIconPage" /> class.
        /// </summary>
        public LoginWithSocialIconPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function sets Main Page to Navigation Page
        /// </summary>
        private void Button_Clicked(object sender, EventArgs e)
        {

           Application.Current.MainPage = new NavigationPage(new BottomNavigationPage());

        }


        //MessagingCenter.Subscribe<object, string>(this, App.NotificationReceivedKey, OnMessageReceived);

        
        

        /*void OnMessageReceived(object sender, string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //Update Label
                DependencyService.Get<NotificationServices>().CreateNotification("Anoroc", msg);
            });
        }*/
    }
}