using System;

namespace TacitusLogger.Exceptions
{
    public class LogCreationException : Exception
    {
        public LogCreationException(string message)
            : base(message)
        {

        }
        public LogCreationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
