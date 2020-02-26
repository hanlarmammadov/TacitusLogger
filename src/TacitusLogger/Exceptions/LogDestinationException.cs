using System;

namespace TacitusLogger.Exceptions
{
    public class LogDestinationException : Exception
    {
        public LogDestinationException(string message)
            : base(message)
        {

        }
        public LogDestinationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    } 
}
