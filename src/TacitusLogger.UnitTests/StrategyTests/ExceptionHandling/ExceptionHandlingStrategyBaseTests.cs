using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using TacitusLogger.Diagnostics;
using TacitusLogger.Strategies.ExceptionHandling;

namespace TacitusLogger.UnitTests.StrategyTests.ExceptionHandling
{
    [TestFixture]
    public class ExceptionHandlingStrategyBaseTests
    {
        public class TestExceptionHandlingStrategy : ExceptionHandlingStrategyBase
        {
            public override bool ShouldRethrow { get; }

            public override void HandleException(Exception exception, string context)
            {
                throw new NotImplementedException();
            }
            public override Task HandleExceptionAsync(Exception exception, string context, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        #region Tests for InitStrategy method

        [Test]
        public void SetDiagnosticsManager_When_Called_Then_Sets_Provided_Destination_As_Hanlder_Destination()
        {
            DiagnosticsManagerBase diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;
            TestExceptionHandlingStrategy testExceptionHandlingStrategy = new TestExceptionHandlingStrategy();

            // Act
            testExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManager);

            // Assert
            Assert.AreEqual(diagnosticsManager, testExceptionHandlingStrategy.DiagnosticsManager);
        }
        [Test]
        public void SetDiagnosticsManager_When_Called_With_Null_Then_Throw_ArgumentNullException()
        {
            // Arrange
            TestExceptionHandlingStrategy testExceptionHandlingStrategy = new TestExceptionHandlingStrategy();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                testExceptionHandlingStrategy.SetDiagnosticsManager(null as DiagnosticsManagerBase);
            });
            Assert.AreEqual("diagnosticsManager", ex.ParamName);
        }

        #endregion
    }
}
