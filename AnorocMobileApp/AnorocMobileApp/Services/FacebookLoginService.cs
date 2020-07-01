using System;

namespace AnorocMobileApp.Services
{
    /// <summary>
    /// Facebook Login Service
    /// </summary>
    public interface IFacebookLoginService
    {

        string FirstName { get; }
        string UserID { get; }
        string LastName { get; }
        string AccessToken { get; }
        void setUserDetails();
        bool waitOnProfile();

        Action<string, string> AccessTokenChanged { get; set; }
        void Logout();
        bool isLoggedIn();
    }
}
