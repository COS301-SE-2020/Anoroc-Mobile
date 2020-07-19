using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AnorocMobileApp.Exceptions
{
    class CantConnecToClusterServiceException : Exception
    {
        public CantConnecToClusterServiceException()
        {
        }

        public CantConnecToClusterServiceException(string message) : base(message)
        {
        }

        public CantConnecToClusterServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantConnecToClusterServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
