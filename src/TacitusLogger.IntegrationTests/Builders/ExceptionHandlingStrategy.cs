using Moq;
using NUnit.Framework;
using TacitusLogger.Builders;
using TacitusLogger.Strategies.ExceptionHandling;

namespace TacitusLogger.IntegrationTests.Builders
{
    [TestFixture]
    public class ExceptionHandlingStrategy
    {
        [Test]
        public void LoggerBuilder_When_Configuring_Exception_Handling_Strategy_When_Creates_Builder_With_This_Strategy()
        {
            // Act
            Logger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Rethrow)
                                                  .BuildLogger();

            // Assert
            Assert.IsInstanceOf<RethrowExceptionHandlingStrategy>(logger.ExceptionHandlingStrategy);
        }
        [Test]
        public void LoggerBuilder_When_Exception_Handling_Strategy_Not_Configured_Then_Sets_Default_Strategy()
        {
            // Act
            Logger logger = LoggerBuilder.Logger().BuildLogger();

            // Assert
            Assert.IsInstanceOf<SilentExceptionHandlingStrategy>(logger.ExceptionHandlingStrategy);
        }
        [Test]
        public void LoggerBuilder_When_Configuring_Custom_Exception_Handling_Strategy_Then_Creates_Builder_With_This_Strategy()
        {
            //Arrange
            var exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithExceptionHandling(exceptionHandlingStrategy)
                                                  .BuildLogger();

            // Assert
            Assert.AreEqual(exceptionHandlingStrategy, logger.ExceptionHandlingStrategy);
        }
    }
}
