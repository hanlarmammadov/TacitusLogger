using NUnit.Framework;
using TacitusLogger.Strategies.LogCreation;

namespace TacitusLogger.UnitTests.StrategyTests.LogCreation
{
    [TestFixture]
    public class LogCreationStrategyFactoryTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void GetStrategy_WhenCalledWithEnumDefaultValue_Returns_Right_Strategy(bool useUtcTime)
        {
            // Arrange
            LogCreationStrategyFactory logCreationStrategyFactory = new LogCreationStrategyFactory();

            // Act
            var strategy = logCreationStrategyFactory.GetStrategy(TacitusLogger.LogCreation.Standard, useUtcTime);

            // Assert
            Assert.IsInstanceOf<StandardLogCreationStrategy>(strategy);
            Assert.AreEqual(useUtcTime, (strategy as StandardLogCreationStrategy).UseUtcTime);
        }
    }
}
