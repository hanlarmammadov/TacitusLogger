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
    public class RethrowExceptionHandlingStrategyTests
    {
        [Test]
        public void ShouldRethrow_Always_Returns_True()
        {
            // Act
            RethrowExceptionHandlingStrategy rethrowExceptionHandlingStrategy = new RethrowExceptionHandlingStrategy();

            // Assert
            Assert.IsTrue(rethrowExceptionHandlingStrategy.ShouldRethrow);
        }

        [Test]
        public void SetDiagnosticsManager_Returns_Successfully()
        {
            // Arrange
            RethrowExceptionHandlingStrategy rethrowExceptionHandlingStrategy = new RethrowExceptionHandlingStrategy();
            DiagnosticsManagerBase diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;

            // Act
            rethrowExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManager);
        } 
        [Test]
        public void HandleException_Returns_Successfully()
        {
            // Arrange
            RethrowExceptionHandlingStrategy rethrowExceptionHandlingStrategy = new RethrowExceptionHandlingStrategy();

            // Act
            rethrowExceptionHandlingStrategy.HandleException(new Exception(), "context");
        }

        [Test]
        public void HandleExceptionAsync_When_Called_Returns_Completed_Task()
        {
            // Arrange
            RethrowExceptionHandlingStrategy rethrowExceptionHandlingStrategy = new RethrowExceptionHandlingStrategy();
            CancellationToken cancellationToken = new CancellationToken(canceled: false);

            // Act
            var task = rethrowExceptionHandlingStrategy.HandleExceptionAsync(new Exception(), "context", cancellationToken);

            // Assert
            Assert.IsTrue(task.IsCompleted);
        }
        [Test]
        public void HandleExceptionAsync_When_Called_With_Cancelled_Cancellation_Token_Returns_Cancelled_Task()
        {
            // Arrange
            RethrowExceptionHandlingStrategy rethrowExceptionHandlingStrategy = new RethrowExceptionHandlingStrategy();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Act
            var task = rethrowExceptionHandlingStrategy.HandleExceptionAsync(new Exception(), "context", cancellationToken);

            // Assert
            Assert.IsTrue(task.IsCanceled);
        }
    }
}
