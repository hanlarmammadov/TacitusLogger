using System;

namespace TacitusLogger.Exceptions
{
    public class LogCacheException : Exception
    {
        public LogCacheException(string message)
            : base(message)
        {

        }
        public LogCacheException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
