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
        public FacebookLoginService()
        {
          
            myAccessTokenTracker = new MyAccessTokenTracker(this);
            // TODO: Stop tracking
            myAccessTokenTracker.StartTracking();
            Profile profile = Profile.CurrentProfile;
        }

        public string AccessToken => Xamarin.Facebook.AccessToken.CurrentAccessToken?.Token;
       
        public string FirstName => Profile.CurrentProfile.FirstName;
        public string UserID => Profile.CurrentProfile.Id;
        public string LastName => Profile.CurrentProfile.LastName;
      
        public void Logout()
        {
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