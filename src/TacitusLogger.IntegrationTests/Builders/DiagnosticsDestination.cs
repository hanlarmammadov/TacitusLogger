using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Destinations;

namespace TacitusLogger.IntegrationTests.Builders
{
    [TestFixture]
    public class DiagnosticsDestination
    {
        [Test]
        public void LoggerBuilder_Configuring_Diagnostics_Destination()
        {
            // Arrange
            var diagnosticsDestination = new Mock<ILogDestination>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithDiagnostics(diagnosticsDestination)
                                                  .BuildLogger();

            // Assert
            Assert.AreEqual(diagnosticsDestination, logger.DiagnosticsDestination);
        }
        [Test]
        public void LoggerBuilder_Configuring_Diagnostics_Destination2()
        {
            // Arrange
            var diagnosticsDestination = new Mock<ILogDestination>().Object;

            // Act
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger().WithDiagnostics(diagnosticsDestination);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                loggerBuilder.WithDiagnostics(diagnosticsDestination);
            });
        }
        [Test]
        public void LoggerBuilder_Diagnostics_Destination_Is_Not_Configured()
        {
            // Arrange
            var diagnosticsDestination = new Mock<ILogDestination>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().BuildLogger();

            // Assert
            Assert.IsNull(logger.DiagnosticsDestination);
        }
    }
}
