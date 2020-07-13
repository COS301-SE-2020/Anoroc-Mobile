using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage_old : TabbedPage
    {
        public string Email { get; set; }
        public IFacebookLoginService FacebookLoginService { get; set; }
        public HomePage_old()
        {
            InitializeComponent();
            
        }
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
        private void checkEmail(object sender, EventArgs e)
        {
            DisplayAlert("Testing", User.FirstName + "," + User.Email, "OK");
        }
        private void logout(object sender, EventArgs e)
        {
            if(User.loggedInFacebook)
            {
                FacebookLoginService.Logout();
                Application.Current.MainPage = new NavigationPage(new LoginWithSocialIconPage());
            }
            else if(User.loggedInAnoroc)
            {
                Application.Current.MainPage = new NavigationPage(new LoginWithSocialIconPage());
            }
        }
    }
}