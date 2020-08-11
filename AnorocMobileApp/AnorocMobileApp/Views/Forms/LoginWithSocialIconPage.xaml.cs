using System;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Navigation;
using Microsoft.Identity.Client;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Forms
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
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
             OnSignInSignOut(sender,e);
        }

        async void OnSignInSignOut(object sender, EventArgs e)
        {

            var userContext = await B2CAuthenticationService.Instance.SignInAsync();
            Console.WriteLine("Access Token: " + userContext.AccessToken);
            Application.Current.Properties["accessToken"] = userContext.AccessToken;

        }


    }
}