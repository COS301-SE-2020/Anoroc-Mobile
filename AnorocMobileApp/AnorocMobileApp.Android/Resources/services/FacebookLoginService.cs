using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace AnorocMobileApp.Droid.Resources.services
{
    public class FacebookLoginService : IFacebookLoginService
    {
        readonly MyAccessTokenTracker myAccessTokenTracker;
        public Action<string, string> AccessTokenChanged { get; set; }
        public string name;
        public Profile profile = Profile.CurrentProfile;
        public ProfileTracker MyProfileTracker;
        public FacebookLoginService()
        {
            

            //this.MyProfileTracker.StartTracking();
            myAccessTokenTracker = new MyAccessTokenTracker(this);
            // TODO: Stop tracking
            myAccessTokenTracker.StartTracking();
           
            //User.loggedInFacebook = true;
        }

        public bool waitOnProfile()
        {
            if (Profile.CurrentProfile == null)
                return true;
            else
                return false;
        }

        public void setUserDetails()
        {
            User.UserID = Profile.CurrentProfile?.Id;
            User.FirstName = Profile.CurrentProfile?.Name;
            User.UserSurname = Profile.CurrentProfile?.LastName;
        }

        /*public AccessToken getFacebookAccessToken()
        {
            return Xamarin.Facebook.AccessToken.CurrentAccessToken;
        }*/

        public bool isLoggedIn()
        {  
            Xamarin.Facebook.AccessToken accessToken =  Xamarin.Facebook.AccessToken.CurrentAccessToken;
            return accessToken != null;
        }

        

        public string AccessToken => Xamarin.Facebook.AccessToken.CurrentAccessToken?.Token;
        
        public string FirstName => Profile.CurrentProfile?.FirstName;
        public string UserID => Profile.CurrentProfile?.Id;
        public string LastName => Profile.CurrentProfile?.LastName;

        public void Logout()
        {
            myAccessTokenTracker.StopTracking();
            LoginManager.Instance.LogOut();
        }
    }

    class MyAccessTokenTracker : AccessTokenTracker
    {
        readonly IFacebookLoginService facebookLoginService;

        public MyAccessTokenTracker(FacebookLoginService facebookLoginService)
        {
            this.facebookLoginService = facebookLoginService;
        }

        protected override void OnCurrentAccessTokenChanged(AccessToken oldAccessToken, AccessToken currentAccessToken)
        {
            facebookLoginService.AccessTokenChanged?.Invoke(oldAccessToken?.Token, currentAccessToken?.Token);
        }
    }
}