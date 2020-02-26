using System;

namespace TacitusLogger.Strategies.DestinationFeeding
{
    public class DestinationFeedingStrategyFactory
    {
        public DestinationFeedingStrategyBase GetStrategy(TacitusLogger.DestinationFeeding  destinationFeeding)
        {
            switch (destinationFeeding)
            {
                case TacitusLogger.DestinationFeeding.Greedy:
                    return new GreedyDestinationFeedingStrategy();
                case TacitusLogger.DestinationFeeding.FirstSuccess:
                    return new FirstSuccessDestinationFeedingStrategy();
                default:
                    throw new NotImplementedException("This feeding strategy has not been implemented yet");
            }
        }
    }
}
