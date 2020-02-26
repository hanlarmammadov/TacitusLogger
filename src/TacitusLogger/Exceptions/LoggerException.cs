using System;

namespace TacitusLogger.Exceptions
{
    public class LoggerException : Exception
    {
        public LoggerException(string message)
            : base(message)
        {

        }
        public LoggerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
