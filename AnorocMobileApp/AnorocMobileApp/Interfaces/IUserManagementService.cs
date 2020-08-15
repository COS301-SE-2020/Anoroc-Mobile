namespace AnorocMobileApp.Interfaces
{
    public interface IUserManagementService
    {
        void RegisterUserAsync(string firstName, string surname, string userEmail);
        void SendFireBaseToken(string firebasetoken);
        void sendCarrierStatusAsync(string value);
    }
}
