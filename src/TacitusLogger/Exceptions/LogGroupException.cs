using System;

namespace TacitusLogger.Exceptions
{
    public class LogGroupException : Exception
    {
        public LogGroupException(string message)
            : base(message)
        {

        }
        public LogGroupException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
