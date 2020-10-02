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

        void ToggleAnonymousUser();
        Task<bool> DownloadData();
    }
}
