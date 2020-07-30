using AnorocMobileApp.Models;

namespace AnorocMobileApp.Interfaces
{
    public interface ILocationService
    {
        void Send_Locaiton_ServerAsync(Location location);
    }
}
