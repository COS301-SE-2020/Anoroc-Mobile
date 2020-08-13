namespace AnorocMobileApp.Models
{
    /// <summary>
    /// Contains a carrierStatus field for the given token
    /// </summary>
    public class CarrierStatus
    {
        /// <summary>
        /// The user's token
        /// </summary>
        private string token;
        
        /// <summary>
        /// The user's carrier status
        /// </summary>
        private bool carrierStatus;

        public CarrierStatus(string token, bool carrierStatus)
        {
            this.token = token;
            this.carrierStatus = carrierStatus;
        }
    }
}