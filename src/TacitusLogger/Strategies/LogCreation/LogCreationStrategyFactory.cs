using System;

namespace TacitusLogger.Strategies.LogCreation
{
    public class LogCreationStrategyFactory
    {
        public LogCreationStrategyBase GetStrategy(TacitusLogger.LogCreation logCreation, bool useUtcTime = false)
        {
            switch (logCreation)
            {
                case TacitusLogger.LogCreation.Standard:
                    return new StandardLogCreationStrategy(useUtcTime);
                default:
                    throw new NotImplementedException("This log creation strategy has not been implemented yet");
            }
        }
    }
}
