using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    /// <summary>
    /// Class to manage the Home Page Screen
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage_old : TabbedPage
    {
        /// <summary>
        /// Getter's and Setter's for Email and Facebook Login Service
        /// </summary>
        public string Email { get; set; }
        public IFacebookLoginService FacebookLoginService { get; set; }
        
        /// <summary>
        /// Constructor to load homepage
        /// </summary>
        public HomePage_old()
        {
            InitializeComponent();
            
        }
        
        /// <summary>
        /// Tests whether the user is logged in with Facebook
        /// </summary>
        /// <param name="facebookLoginService">Login With Facebook Service</param>
        public HomePage_old(IFacebookLoginService facebookLoginService)
        {
            FacebookLoginService = facebookLoginService;
            
            InitializeComponent();
            lblAnorocHeading.Text = User.FirstName + " " + User.UserSurname;
            if (User.loggedInFacebook)
            {
                lblTitle.Text = "Anoroc Logged in with Facebook";
            }
        }
        /// <summary>
        /// Test function to retrive the User's data
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Event Arguments</param>
        private void checkEmail(object sender, EventArgs e)
        {
            DisplayAlert("Testing", User.FirstName + "," + User.Email, "OK");
        }
        /// <summary>
        /// Log out function to log user out from Facebok or Google
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Arguments</param>
        private void logout(object sender, EventArgs e)
        {
            if(User.loggedInFacebook)
            {
                FacebookLoginService.Logout();
                Application.Current.MainPage = new NavigationPage(new Login());
            }
            else if(User.loggedInAnoroc)
            {
                Application.Current.MainPage = new NavigationPage(new Login());
            }
        }
    }
}