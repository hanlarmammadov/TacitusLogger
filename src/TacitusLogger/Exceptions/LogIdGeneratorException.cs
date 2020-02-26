using System;

namespace TacitusLogger.Exceptions
{
    public class LogIdGeneratorException : Exception
    {
        public LogIdGeneratorException(string message)
            : base(message)
        {

        }
        public LogIdGeneratorException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
