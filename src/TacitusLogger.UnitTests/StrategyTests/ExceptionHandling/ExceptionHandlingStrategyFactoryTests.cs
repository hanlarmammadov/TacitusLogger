using NUnit.Framework;
using TacitusLogger.Strategies.ExceptionHandling;

namespace TacitusLogger.UnitTests.StrategyTests.ExceptionHandling
{
    [TestFixture]
    public class ExceptionHandlingStrategyFactoryTests
    {
        [Test]
        public void GetStrategy_When_Called_With_Silent_Enum_Value_Returns_Correct_Strategy()
        {
            // Arrange
            ExceptionHandlingStrategyFactory exceptionHandlingStrategyFactory = new ExceptionHandlingStrategyFactory();

            // Act
            var strategy = exceptionHandlingStrategyFactory.GetStrategy(TacitusLogger.ExceptionHandling.Silent);

            // Assert
            Assert.IsInstanceOf<SilentExceptionHandlingStrategy>(strategy);
        }
        [Test]
        public void GetStrategy_When_Called_With_Rethrow_Enum_Value_Returns_Correct_Strategy()
        {
            // Arrange
            ExceptionHandlingStrategyFactory exceptionHandlingStrategyFactory = new ExceptionHandlingStrategyFactory();

            // Act
            var strategy = exceptionHandlingStrategyFactory.GetStrategy(TacitusLogger.ExceptionHandling.Rethrow);

            // Assert
            Assert.IsInstanceOf<RethrowExceptionHandlingStrategy>(strategy);
        }
        public void GetStrategy_When_Called_With_Log_Enum_Value_Returns_Correct_Strategy()
        {
            // Arrange
            ExceptionHandlingStrategyFactory exceptionHandlingStrategyFactory = new ExceptionHandlingStrategyFactory();

            // Act
            var strategy = exceptionHandlingStrategyFactory.GetStrategy(TacitusLogger.ExceptionHandling.Log);

            // Assert
            Assert.IsInstanceOf<LogExceptionHandlingStrategy>(strategy);
        }

    }
}
