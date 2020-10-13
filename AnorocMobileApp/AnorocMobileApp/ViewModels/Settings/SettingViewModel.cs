using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using AnorocMobileApp.Views.Forms;
using Plugin.SecureStorage;
using Plugin.Toast;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AnorocMobileApp.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public SettingViewModel()
        {
            BackButtonCommand = new Command(BackButtonClicked);
            EditProfileCommand = new Command(EditProfileClicked);
            ChangePasswordCommand = new Command(ChangePasswordClicked);
            RequestAllPersonalDataCommand = new Command(RequestAllPersonalDataClicked);
            HelpCommand = new Command(HelpClicked);
            TermsCommand = new Command(TermsServiceClicked);
            PolicyCommand = new Command(PrivacyPolicyClicked);
            FAQCommand = new Command(FAQClicked);
            LogOutCommand = new Command(LogOutClicked);
            DeleteAccount = new Command(DeleteUserAccount);
            DontTrackCommand = new Command(DontTrackLocation);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command is executed when the favourite button is clicked.
        /// </summary>
        public Command BackButtonCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the edit profile option is clicked.
        /// </summary>
        public Command EditProfileCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the change password option is clicked.
        /// </summary>
        public Command ChangePasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the account link option is clicked.
        /// </summary>
        public Command RequestAllPersonalDataCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the help option is clicked.
        /// </summary>
        public Command HelpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the terms of service option is clicked.
        /// </summary>
        public Command TermsCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the privacy policy option is clicked.
        /// </summary>
        public Command PolicyCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the FAQ option is clicked.
        /// </summary>
        public Command FAQCommand { get; set; }
        
        /// <summary>
        /// Gets or sets the command that is executed when the Log Out button is clicked
        /// </summary>
        public Command LogOutCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Delete account button is clicked
        /// </summary>
        public Command DeleteAccount { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Dont track button is clicked
        /// </summary>
        public Command DontTrackCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the back button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void BackButtonClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the edit profile option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void EditProfileClicked(object obj)
        {
            var userContext = await B2CAuthenticationService.Instance.EditProfileAsync(); 
            // Do something
        }

        /// <summary>
        /// Invoked when the change password clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void ChangePasswordClicked(object obj)
        {
            var userContext = await B2CAuthenticationService.Instance.ResetPasswordAsync();
            // Do something
        }

        /// <summary>
        /// Invoked when the account link clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void RequestAllPersonalDataClicked(object obj)
        {
            CrossToastPopUp.Current.ShowToastMessage("Requesting...");
            var user = App.IoCContainer.GetInstance<IUserManagementService>();
            var userdata = await user.DownloadData();
            if (userdata)
            {
                CrossToastPopUp.Current.ShowToastMessage("Email Sent.");
            }
            else
            {
                CrossToastPopUp.Current.ShowToastMessage("Error Generating the file.");
            }
        }

        /// <summary>
        /// Invoked when the terms of service clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void TermsServiceClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the privacy and policy clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void PrivacyPolicyClicked(object obj)
        {
            // Do something
            await Browser.OpenAsync(new Uri("https://anorocb2cloginwebsite.blob.core.windows.net/anorocb2cloginwebsite/privacyinfo.html"), new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.AliceBlue,
                PreferredControlColor = Color.Violet
            });
        }

        /// <summary>
        /// Invoked when the FAQ clicked
        /// </summary>
        /// <param name="obj">The object</param>
        /// 

        private async void FAQClicked(object obj)
        {
            // Do something
            await Browser.OpenAsync(new Uri("https://anorocb2cloginwebsite.blob.core.windows.net/anorocb2cloginwebsite/faqpage.component.html"), new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.AliceBlue,
                PreferredControlColor = Color.Violet
            });
        }

        /// <summary>
        /// Invoked when the help option is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void HelpClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the log out button is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void LogOutClicked(object obj)
        {
            var userContext = await B2CAuthenticationService.Instance.SignOutAsync();
            if (!userContext.IsLoggedOn)
            {
                // BUG why does UpdateSignInState uses APIKEY as thisisatoken
                UpdateSignInState(userContext);   
                Application.Current.MainPage = new LoginWithSocialIconPage();
            } 
        }
        
        /// <summary>
        /// Used to update the sign in state
        /// </summary>
        /// <param name="userContext"></param>
        void UpdateSignInState(UserContext userContext)
        {
            var isSignedIn = userContext.IsLoggedOn;

            CrossSecureStorage.Current.SetValue("SignedIn", isSignedIn.ToString());
            CrossSecureStorage.Current.SetValue("SignedInFirstTime", "false");
            CrossSecureStorage.Current.SetValue("APIKEY","thisisatoken");
            CrossSecureStorage.Current.SetValue("Name", "");
            CrossSecureStorage.Current.SetValue("Surname", "");
            CrossSecureStorage.Current.SetValue("Email", "");

            //btnSignInSignOut.Text = isSignedIn ? "Sign out" : "Sign in";

        }

        /// <summary>
        /// Invoked when the Delete User account is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void DeleteUserAccount(object obj)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete Account", "Are you sure you wish to delete your account?", "Yes", "No");
            if(answer)
            {
                bool answer2 = await Application.Current.MainPage.DisplayAlert("Delete Account", "This process may not be undone, are you sure you want to continue?", "Yes", "No");
                if(answer2)
                {
                    bool answer3 = await Application.Current.MainPage.DisplayAlert("Delete Account", "Are you VERY sure?", "No", "Yes");
                    if(!answer3)
                    {
                        //delete account
                        var user = App.IoCContainer.GetInstance<IUserManagementService>();
                        user.DeleteTheUser();
                        var userContext = await B2CAuthenticationService.Instance.SignOutAsync();
                        if (!userContext.IsLoggedOn)
                        {
                            // BUG why does UpdateSignInState uses APIKEY as thisisatoken
                            UpdateSignInState(userContext);
                        }
                        Application.Current.MainPage = new LoginWithSocialIconPage();
                    }
                }
            }
        }

        private void DontTrackLocation(object obj)
        {
            var locationSerivce = App.IoCContainer.GetInstance<ILocationService>();
            locationSerivce.DontSendCurrentLocationAnymoreAsync();
            CrossToastPopUp.Current.ShowToastMessage("This location won't be tracked.");
        }
        #endregion
    }
}
