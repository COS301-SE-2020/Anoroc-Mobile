using AnorocMobileApp.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AnorocMobileApp.Interfaces
{
    public interface IUserManagementService
    {
        void UserLoggedIn(string firstName, string surname, string userEmail);
        void SendFireBaseToken(string firebasetoken);
        void sendCarrierStatusAsync(string value);
        void CheckIncidents();
        Task<int> UpdatedIncidents();
        Task<string> GetUserProfileImage();
        void UploadUserProfileImage(string image);

        Task<string> ToggleAnonymousUser();
        Task<bool> DownloadData();
        Task<NotificationDB[]> GetNotifications();

        //void sendNotification(string notificationString);
        void DeleteTheUser();
    }
}
