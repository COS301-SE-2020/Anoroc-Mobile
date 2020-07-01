using AnorocMobileApp.Services;
using AnorocMobileApp.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace AnorocMobileApp.Models
{
    /// <summary>
    /// View Model used ro the Login Feature
    /// </summary>
    public class LoginViewModel
    {
        public UserDetails userDetails = null;
        public readonly IFacebookLoginService facebookLoginService;

        public Command FacebookLogoutCmd { get; }
        public ICommand OnFacebookLoginSuccessCmd { get; }
        public ICommand OnFacebookLoginErrorCmd { get; }
        public ICommand OnFacebookLoginCancelCmd { get; }
        public static bool facebookLoginTest = true;
        /// <summary>
        /// A View Model for the Facebook Login that is used to handle successful logins, on login, and errors on login
        /// </summary>
        public LoginViewModel()
        {
            facebookLoginService = (Application.Current as App).FacebookLoginService;
            facebookLoginService.AccessTokenChanged = (string oldToken, string newToken) => FacebookLogoutCmd.ChangeCanExecute();

            //Test if the user has logged in already
          /*  if (facebookLoginTest)
            {
                if (facebookLoginService.isLoggedIn())
                {
                    Login.FacebookLoggedInAlready(facebookLoginService);
                }
                facebookLoginTest = false;
            }
*/

            FacebookLogoutCmd = new Command(() =>
                facebookLoginService.Logout(),
                () => !string.IsNullOrEmpty(facebookLoginService.AccessToken));

            OnFacebookLoginSuccessCmd = new Command<string>(
                (authToken) => Success("Success", authToken));

            OnFacebookLoginErrorCmd = new Command<string>(
                (err) => DisplayAlert("Error", $"Authentication failed: { err }"));

            OnFacebookLoginCancelCmd = new Command(
                () => DisplayAlert("Cancel", "Authentication cancelled by the user."));
        }
        /// <summary>
        /// A successful login from Facebook returns an Authorization token which is used
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="authToken">Authorization Token provided by Facebook</param>
        public void Success(string title, string authToken)
        {
            User.loggedInFacebook = true;
            _= LoginService.fillUserDetails(facebookLoginService);
            Login.FacebookSuccess(title, authToken, facebookLoginService);
        }
        /// <summary>
        /// A notification of the state of the Login through Facebook, if there is an error this function will notify the user
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="msg">Error message on unsuccessful login</param>
        public void DisplayAlert(string title, string msg)
        {
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "OK");
        }
    }
}
