using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Components.Time;
using TacitusLogger.Destinations;
using TacitusLogger.Diagnostics;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;

namespace TacitusLogger.UnitTests.StrategyTests.ExceptionHandling
{
    [TestFixture]
    public class LogExceptionHandlingStrategyTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Diagnostics_Manager_Is_Null()
        {
            // Act
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();

            // Assert
            Assert.IsNull(logExceptionHandlingStrategy.DiagnosticsManager);
        }

        #endregion

        #region Tests for SetDiagnosticsManager method

        [Test]
        public void SetDiagnosticsManager_When_Called_Sets_Diagnostics_Manager()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            DiagnosticsManagerBase diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;

            // Act
            logExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManager);

            // Assert
            Assert.AreEqual(diagnosticsManager, logExceptionHandlingStrategy.DiagnosticsManager);
        }
        [Test]
        public void SetDiagnosticsManager_When_Called_With_Null_Throws_ArgumentNullException()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            DiagnosticsManagerBase diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logExceptionHandlingStrategy.SetDiagnosticsManager(null as DiagnosticsManagerBase);
            });
            Assert.AreEqual("diagnosticsManager", ex.ParamName);
        }

        #endregion

        #region Tests for ShouldRethrow property

        [Test]
        public void ShouldRethrow_Always_Returns_False()
        {
            // Act
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();

            // Assert
            Assert.IsFalse(logExceptionHandlingStrategy.ShouldRethrow);
        }

        #endregion

        #region Tests for HandleException property

        [Test]
        public void HandleException_When_Handler_Is_Not_Set_Does_Not_Throw()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            Exception ex = new Exception();

            // Act
            logExceptionHandlingStrategy.HandleException(new Exception(), "context1");
        }
        [Test]
        public void HandleException_When_Called_Creates_Log_Data_And_Passes_To_Log_Destination()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();     
            var diagnosticsManagerMock = new Mock<DiagnosticsManagerBase>();
            logExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManagerMock.Object);
            Exception ex = new Exception();

            // Act
            logExceptionHandlingStrategy.HandleException(ex, "context1");

            // Assert 
            diagnosticsManagerMock.Verify(x => x.WriteToDiagnostics(It.Is<Log>(d=> d.Type == LogType.Error &&
                                                                                   d.Description == "Logger threw an exception. See the log item." &&
                                                                                   d.Context == "context1" &&
                                                                                   d.Items.Count == 1 &&
                                                                                   d.Items[0].Name == "Exception" &&
                                                                                   d.Items[0].Value == ex)));
        }
        [Test]
        public void HandleException_When_Exception_Is_Thrown_Inside_Does_Not_Rethrow()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            // Destination manager that throws an exception.
            var diagnosticsManagerMock = new Mock<DiagnosticsManagerBase>();
            diagnosticsManagerMock.Setup(x => x.WriteToDiagnostics(It.IsAny<Log>())).Throws<Exception>(); 
            logExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManagerMock.Object); 

            // Act
            logExceptionHandlingStrategy.HandleException(new Exception(), "context");
        }

        #endregion

        #region Tests for HandleExceptionAsync property

        [Test]
        public async Task HandleExceptionAsync_When_Handler_Is_Not_Set_Does_Not_Throw()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            Exception ex = new Exception();

            // Act
            await logExceptionHandlingStrategy.HandleExceptionAsync(new Exception(), "context1");
        }
        [Test]
        public async Task HandleExceptionAsync_When_Called_Creates_Log_Data_And_Passes_To_Log_Destination()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            var diagnosticsManagerMock = new Mock<DiagnosticsManagerBase>();
            logExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManagerMock.Object);
            Exception ex = new Exception();
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await logExceptionHandlingStrategy.HandleExceptionAsync(ex, "context1");

            // Assert 
            diagnosticsManagerMock.Verify(x => x.WriteToDiagnosticsAsync(It.Is<Log>(d => d.Type == LogType.Error &&
                                                                                    d.Description == "Logger threw an exception. See the log item." &&
                                                                                    d.Context == "context1" &&
                                                                                    d.Items.Count == 1 &&
                                                                                    d.Items[0].Name == "Exception" &&
                                                                                    d.Items[0].Value == ex), cancellationToken), Times.Once);
        }
        [Test]
        public async Task HandleExceptionAsync_When_Exception_Is_Thrown_Inside_Does_Not_Rethrow()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            // Destination manager that throws an exception.
            var diagnosticsManagerMock = new Mock<DiagnosticsManagerBase>();
            diagnosticsManagerMock.Setup(x => x.WriteToDiagnostics(It.IsAny<Log>())).Throws<Exception>();
            logExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManagerMock.Object);

            // Act
            await logExceptionHandlingStrategy.HandleExceptionAsync(new Exception(), "context");
        }
        [Test]
        public void HandleExceptionAsync_When_Called_With_Cancelled_Cancellation_Token_Returns_Cancelled_Task()
        {
            // Arrange 
            LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
            // Destination manager that throws an exception.
            var diagnosticsManagerMock = new Mock<DiagnosticsManagerBase>(); 
            logExceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManagerMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Act
            Task task = logExceptionHandlingStrategy.HandleExceptionAsync(new Exception(), "context1", cancellationToken);

            // Assert
            Assert.IsTrue(task.IsCanceled);
            diagnosticsManagerMock.Verify(x => x.WriteToDiagnosticsAsync(It.IsAny<Log>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        //#region Tests for ResetTimeProvider property

        //[Test]
        //public void ResetTimeProvider_When_Called_Sets_Time_Provider()
        //{
        //    // Arrange
        //    LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
        //    var timeProvider = new Mock<ITimeProvider>().Object;

        //    // Act
        //    logExceptionHandlingStrategy.ResetTimeProvider(timeProvider);

        //    // Assert
        //    Assert.AreEqual(timeProvider, logExceptionHandlingStrategy.TimeProvider);
        //}
        //[Test]
        //public void ResetTimeProvider_When_Called_With_Null_Time_Provider_Throws_ArgumentNullException()
        //{
        //    // Arrange
        //    LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();

        //    // Assert
        //    var ex = Assert.Catch<ArgumentNullException>(() =>
        //    {
        //        // Act
        //        logExceptionHandlingStrategy.ResetTimeProvider(null as ITimeProvider);
        //    });
        //    Assert.AreEqual("timeProvider", ex.ParamName);
        //}

        //#endregion

        //#region Tests for ResetLogIdGenerator property

        //[Test]
        //public void ResetLogIdGenerator_When_Called_Sets_Log_Id_Generator()
        //{
        //    // Arrange
        //    LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();
        //    var logIdGenerator = new Mock<ILogIdGenerator>().Object;

        //    // Act
        //    logExceptionHandlingStrategy.ResetLogIdGenerator(logIdGenerator);

        //    // Assert
        //    Assert.AreEqual(logIdGenerator, logExceptionHandlingStrategy.LogIdGenerator);
        //}
        //[Test]
        //public void ResetLogIdGenerator_When_Called_With_Null_Log_Id_Generator_Throws_ArgumentNullException()
        //{
        //    // Arrange
        //    LogExceptionHandlingStrategy logExceptionHandlingStrategy = new LogExceptionHandlingStrategy();

        //    // Assert
        //    var ex = Assert.Catch<ArgumentNullException>(() =>
        //    {
        //        // Act
        //        logExceptionHandlingStrategy.ResetLogIdGenerator(null as ILogIdGenerator);
        //    });
        //    Assert.AreEqual("logIdGenerator", ex.ParamName);
        //}

        //#endregion
    }
}
