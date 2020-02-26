using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using TacitusLogger.Destinations;
using TacitusLogger.Diagnostics;
using TacitusLogger.Strategies.ExceptionHandling;

namespace TacitusLogger.UnitTests.StrategyTests.ExceptionHandling
{
    [TestFixture]
    public class SilentExceptionHandlingStrategyTests
    {
        [Test]
        public void ShouldRethrow_Always_Returns_False()
        {
            // Act
            SilentExceptionHandlingStrategy silentExceptionHandlingStrategy = new SilentExceptionHandlingStrategy();

            // Assert
            Assert.IsFalse(silentExceptionHandlingStrategy.ShouldRethrow);
        }

        [Test]
        public void SetDiagnosticsManager_Returns_Successfully()
        {
            // Arrange
            SilentExceptionHandlingStrategy silentExceptionHandlingStrategy = new SilentExceptionHandlingStrategy();
            DiagnosticsManagerBase diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;

            // Act
            silentExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManager);
        }

        [Test]
        public void HandleException_Returns_Successfully()
        {
            // Arrange
            SilentExceptionHandlingStrategy silentExceptionHandlingStrategy = new SilentExceptionHandlingStrategy();

            // Act
            silentExceptionHandlingStrategy.HandleException(new Exception(), "context");
        }

        [Test]
        public void HandleExceptionAsync_When_Called_Returns_Completed_Task()
        {
            // Arrange
            SilentExceptionHandlingStrategy silentExceptionHandlingStrategy = new SilentExceptionHandlingStrategy();
            CancellationToken cancellationToken = new CancellationToken(canceled: false);

            // Act
            var task = silentExceptionHandlingStrategy.HandleExceptionAsync(new Exception(), "context", cancellationToken);

            // Assert
            Assert.IsTrue(task.IsCompleted);
        }
        [Test]
        public void HandleExceptionAsync_When_Called_With_Cancelled_Cancellation_Token_Returns_Cancelled_Task()
        {
            // Arrange
            SilentExceptionHandlingStrategy silentExceptionHandlingStrategy = new SilentExceptionHandlingStrategy();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Act
            var task = silentExceptionHandlingStrategy.HandleExceptionAsync(new Exception(), "context", cancellationToken);

            // Assert
            Assert.IsTrue(task.IsCanceled);
        }
    }
}
