using System;

namespace TacitusLogger.Strategies.ExceptionHandling
{
    public class ExceptionHandlingStrategyFactory
    {
        public ExceptionHandlingStrategyBase GetStrategy(TacitusLogger.ExceptionHandling exceptionHandling)
        {
            switch (exceptionHandling)
            {
                case TacitusLogger.ExceptionHandling.Silent:
                    return new SilentExceptionHandlingStrategy();
                case TacitusLogger.ExceptionHandling.Log:
                    return new LogExceptionHandlingStrategy();
                case TacitusLogger.ExceptionHandling.Rethrow:
                    return new RethrowExceptionHandlingStrategy();
                default:
                    throw new NotImplementedException("This exception handling strategy has not been implemented yet");
            }
        }
    }
}
