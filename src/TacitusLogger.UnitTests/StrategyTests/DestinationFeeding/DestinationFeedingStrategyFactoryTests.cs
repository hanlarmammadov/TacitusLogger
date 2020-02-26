using NUnit.Framework; 
using TacitusLogger.Strategies.DestinationFeeding;

namespace TacitusLogger.UnitTests.StrategyTests.DestinationFeeding
{
    [TestFixture]
    public class DestinationFeedingStrategyFactoryTests
    {
        [Test]
        public void GetStrategy_When_Called_With_Greedy_Option_Returns_GreedyDestinationFeedingStrategy()
        {
            //Arrange
            DestinationFeedingStrategyFactory destinationFeedingStrategyFactory = new DestinationFeedingStrategyFactory();

            //Act
            DestinationFeedingStrategyBase strategyReturned = destinationFeedingStrategyFactory.GetStrategy(TacitusLogger.DestinationFeeding.Greedy);

            //Assert
            Assert.IsInstanceOf<GreedyDestinationFeedingStrategy>(strategyReturned);
        }
        [Test]
        public void GetStrategy_When_Called_With_FirstSuccess_Option_Returns_FirstSuccessDestinationFeedingStrategy()
        {
            //Arrange
            DestinationFeedingStrategyFactory destinationFeedingStrategyFactory = new DestinationFeedingStrategyFactory();

            //Act
            DestinationFeedingStrategyBase strategyReturned = destinationFeedingStrategyFactory.GetStrategy(TacitusLogger.DestinationFeeding.FirstSuccess);

            //Assert
            Assert.IsInstanceOf<FirstSuccessDestinationFeedingStrategy>(strategyReturned);
        }
    }
}
