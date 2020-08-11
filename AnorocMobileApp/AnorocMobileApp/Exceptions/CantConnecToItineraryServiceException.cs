using System;
using System.Runtime.Serialization;

namespace AnorocMobileApp.Services
{
    [Serializable]
    internal class CantConnecToItineraryServiceException : Exception
    {
        public CantConnecToItineraryServiceException()
        {
        }

        public CantConnecToItineraryServiceException(string message) : base(message)
        {
        }

        public CantConnecToItineraryServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantConnecToItineraryServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}