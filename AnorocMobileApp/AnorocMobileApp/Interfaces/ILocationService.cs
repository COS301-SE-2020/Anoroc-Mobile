using AnorocMobileApp.Models;
using System.Collections.Generic;

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
        void DontSendCurrentLocationAnymoreAsync();

        /// <summary>
        /// Get a list of locations to compare to to see if the current location is found
        /// </summary>
        /// <returns></returns>
        bool LocationSavedToNotSend(Location location);
    }
}
