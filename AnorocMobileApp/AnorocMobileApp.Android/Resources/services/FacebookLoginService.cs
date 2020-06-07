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
    public class FacebookLoginService : Services.IFacebookLoginService
    {
        readonly MyAccessTokenTracker myAccessTokenTracker;
        public Action<string, string> AccessTokenChanged { get; set; }

        public FacebookLoginService()
        {
            myAccessTokenTracker = new MyAccessTokenTracker(this);
            // TODO: Stop tracking
            myAccessTokenTracker.StartTracking();
        }

        public string AccessToken => Xamarin.Facebook.AccessToken.CurrentAccessToken?.Token;

        public void Logout()
        {
            LoginManager.Instance.LogOut();
        }
    }

    class MyAccessTokenTracker : AccessTokenTracker
    {
        readonly Services.IFacebookLoginService facebookLoginService;

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