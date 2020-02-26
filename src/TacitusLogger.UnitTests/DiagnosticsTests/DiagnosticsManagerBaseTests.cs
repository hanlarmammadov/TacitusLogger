using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using TacitusLogger.Diagnostics;

namespace TacitusLogger.UnitTests.DiagnosticsTests
{
    [TestFixture]
    public class DiagnosticsManagerBaseTests
    {
        public class TestDiagnosticsManager : DiagnosticsManagerBase
        {
            public override void WriteToDiagnostics(Log log)
            {
                throw new NotImplementedException();
            }

            public override Task WriteToDiagnosticsAsync(Log log, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        #region Tests for SetDependencies method

        [Test]
        public void SetDependencies_When_Called_Sets_LogDestination_And_Logger_Name()
        {
            // Arrange
            TestDiagnosticsManager testDiagnosticsManager = new TestDiagnosticsManager();
            var diagnosticsDestination = new Mock<ILogDestination>().Object;
            var loggerName = "logger name";

            // Act
            testDiagnosticsManager.SetDependencies(diagnosticsDestination, loggerName);

            // Assert
            Assert.AreEqual(diagnosticsDestination, testDiagnosticsManager.LogDestination);
            Assert.AreEqual(loggerName, testDiagnosticsManager.LoggerName);
        }
        [Test]
        public void SetDependencies_When_Called_With_Null_LogDestination_Does_Not_Throw()
        {
            // Arrange
            TestDiagnosticsManager testDiagnosticsManager = new TestDiagnosticsManager();

            // Act
            testDiagnosticsManager.SetDependencies(null as ILogDestination, "logger name");

            // Assert
            Assert.IsNull(testDiagnosticsManager.LogDestination);
        }
        [Test]
        public void SetDependencies_When_Called_With_Null_Logger_Name_Does_Not_Throw()
        {
            // Arrange
            TestDiagnosticsManager testDiagnosticsManager = new TestDiagnosticsManager();

            // Act
            testDiagnosticsManager.SetDependencies(new Mock<ILogDestination>().Object, null as string);

            // Assert 
            Assert.IsNull(testDiagnosticsManager.LoggerName);
        }

        #endregion
    }
}
