using System;

namespace TacitusLogger.Exceptions
{
    public class DestinationFeedingException : Exception
    {
        public DestinationFeedingException(string message)
            : base(message)
        {

        }
        public DestinationFeedingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
