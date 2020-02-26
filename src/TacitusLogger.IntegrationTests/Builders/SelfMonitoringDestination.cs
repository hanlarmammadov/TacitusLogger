using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Destinations;

namespace TacitusLogger.IntegrationTests.Builders
{
    [TestFixture]
    public class SelfMonitoringDestination
    {
        [Test]
        public void LoggerBuilder_Configuring_Self_Monitoring_Destination()
        {
            // Arrange
            var selfMonitoringDestination = new Mock<ILogDestination>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithSelfMonitoring(selfMonitoringDestination)
                                                  .BuildLogger();

            // Assert
            Assert.AreEqual(selfMonitoringDestination, logger.SelfMonitoringDestination);
        }
        [Test]
        public void LoggerBuilder_Configuring_Self_Monitoring_Destination2()
        {
            // Arrange
            var selfMonitoringDestination = new Mock<ILogDestination>().Object;

            // Act
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger().WithSelfMonitoring(selfMonitoringDestination);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                loggerBuilder.WithSelfMonitoring(selfMonitoringDestination);
            });
        }
        [Test]
        public void LoggerBuilder_Self_Monitoring_Destination_Is_Not_Configured()
        {
            // Arrange
            var selfMonitoringDestination = new Mock<ILogDestination>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().BuildLogger();

            // Assert
            Assert.IsNull(logger.SelfMonitoringDestination);
        }
    }
}
