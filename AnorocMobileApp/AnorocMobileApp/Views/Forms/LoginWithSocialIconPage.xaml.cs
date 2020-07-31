using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnorocMobileApp.Models;
using Microsoft.Identity.Client;
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
        
        protected override async void OnAppearing()
        {
            /*try
            {
                // Look for existing account
                var accounts = await App.AuthenticationClient.GetAccountsAsync();

                var result = await App.AuthenticationClient
                    .AcquireTokenSilent(Constants.Adb2C.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                await Navigation.PushAsync(new LogoutPage(result));
            }
            catch
            {
                // Do nothing - the user isn't logged in
            }
            base.OnAppearing();*/
        }
        
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            AuthenticationResult result;
            try
            {
                result = await App.AuthenticationClient
                    .AcquireTokenInteractive(Constants.Adb2C.Scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .WithParentActivityOrWindow(App.UIParent)
                    .ExecuteAsync();
                
                Console.WriteLine(result);
                await Navigation.PushAsync(new LoginWithSocialIconPage());
            }
            catch (MsalException ex)
            {
                if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                {
                    result = await OnForgotPassword();
                    Console.WriteLine(result);
                    await Navigation.PushAsync(new LoginWithSocialIconPage());
                }
                else if (ex.ErrorCode != "authentication_canceled")
                {
                    await DisplayAlert("An error has occurred", "Exception message: " + ex.Message, "Dismiss");
                }
            }
        }
        
        async Task<AuthenticationResult> OnForgotPassword()
        {
            try
            {
                return await App.AuthenticationClient
                    .AcquireTokenInteractive(Constants.Adb2C.Scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .WithParentActivityOrWindow(App.UIParent)
                    .WithB2CAuthority(Constants.Adb2C.AuthorityPasswordReset)
                    .ExecuteAsync();
            }
            catch (MsalException)
            {
                // Do nothing - ErrorCode will be displayed in OnLoginButtonClicked
                return null;
            }
        }
    }
}