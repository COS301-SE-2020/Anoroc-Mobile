using AnorocMobileApp.Models;

namespace AnorocMobileApp.Interfaces
{
    public interface ILocationService
    {
        /// <summary>
        /// Send the location to the server.
        /// </summary>
        /// <param name="location">The locaiton to send.</param>
        void Send_Locaiton_ServerAsync(Location location);
        /// <summary>
        /// User can choose that their current location will no longer be sent to the server.
        /// </summary>
        void DontSentCurrentLocationAnymoreAsync();
    }
}
