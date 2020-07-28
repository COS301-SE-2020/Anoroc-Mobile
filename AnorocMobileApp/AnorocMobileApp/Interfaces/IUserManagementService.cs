namespace AnorocMobileApp.Interfaces
{
    public interface IUserManagementService
    {
        void SendFireBaseToken(string firebasetoken);
        void sendCarrierStatusAsync(string value);
    }
}
