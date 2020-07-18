using System;
using System.Runtime.Serialization;

namespace AnorocMobileApp.Services
{
    [Serializable]
    internal class CantConnectToLocationServerException : Exception
    {
        public CantConnectToLocationServerException()
        {
        }

        public CantConnectToLocationServerException(string message) : base(message)
        {
        }

        public CantConnectToLocationServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantConnectToLocationServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}