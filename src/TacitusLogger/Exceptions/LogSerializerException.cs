using System;

namespace TacitusLogger.Exceptions
{
    public class LogSerializerException : Exception
    {
        public LogSerializerException(string message)
            : base(message)
        {

        }
        public LogSerializerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
