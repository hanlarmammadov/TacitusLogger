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

namespace TacitusLogger.UnitTests.DiagnosticsTests
{
    [TestFixture]
    public class DiagnosticsManagerTests
    {
        #region Ctor tests

        public void Ctor_Taking_LogIdGenerator_And_Bool_Flag_When_Called_Sets_Log_Id_Generator_Time_Provider_And_UseUtcTime_Flag()
        {
            // Arrange
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            var useUtcTime = true;

            // Act
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager(logIdGenerator, useUtcTime);

            // Assert
            Assert.AreEqual(logIdGenerator, diagnosticsManager.LogIdGenerator);
            Assert.AreEqual(logIdGenerator, diagnosticsManager.UseUtcTime);
            Assert.IsInstanceOf<SystemTimeProvider>(diagnosticsManager.TimeProvider);
        }
        [TestCase(true)]
        [TestCase(false)]
        public void Ctor_Taking_LogIdGenerator_And_Bool_Flag_When_Called_Sets_UseUtcTime_Flag_Correctly(bool useUtcTime)
        {
            // Act
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager(new Mock<ILogIdGenerator>().Object, useUtcTime);

            // Assert
            Assert.AreEqual(useUtcTime, diagnosticsManager.UseUtcTime);
        }
        public void Ctor_Default_When_Called_Sets_Log_Id_Generator_Time_Provider_And_UseUtcTime_Flag_To_Defaults()
        {
            // Act
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager();

            // Assert
            Assert.IsInstanceOf<ILogIdGenerator>(diagnosticsManager.LogIdGenerator);
            Assert.IsInstanceOf<SystemTimeProvider>(diagnosticsManager.TimeProvider);
            Assert.IsFalse(diagnosticsManager.UseUtcTime);
        }

        #endregion

        #region Tests for WriteToDiagnostics method

        [Test]
        public void WriteToDiagnostics_When_Called_Creates_Log_Model_And_Passes_It_To_Log_Destination()
        {
            // Log id generator
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.Generate(It.IsAny<LogModel>())).Returns("log id");
            //
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager(logIdGeneratorMock.Object, false);
            //
            var logDestinationMock = new Mock<ILogDestination>();
            var loggerName = "logger name";
            //
            diagnosticsManager.SetDependencies(logDestinationMock.Object, loggerName);

            // Time provider
            var timeProviderMock = new Mock<ITimeProvider>();
            var logDateLocal = new DateTime(2020, 1, 2);
            timeProviderMock.Setup(x => x.GetLocalTime()).Returns(logDateLocal);
            diagnosticsManager.ResetTimeProvider(timeProviderMock.Object);
            //
            LogItem item = LogItem.FromObj("name", "value");
            Log log = Log.Critical("description1").From("context1").With(item);

            // Act
            diagnosticsManager.WriteToDiagnostics(log);

            // Assert
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                         m[0].Context == "context1" &&
                                                                         m[0].Description == "description1" &&
                                                                         m[0].LogId == "log id" &&
                                                                         m[0].Source == loggerName &&
                                                                         m[0].IsCritical &&
                                                                         m[0].LogItems.Length == 1 &&
                                                                         m[0].LogItems[0].Name == "name" &&
                                                                         (m[0].LogItems[0].Value as string) == "value" &&
                                                                         m[0].LogDate == logDateLocal)), Times.Once);
            logDestinationMock.VerifyNoOtherCalls();
        }
        [TestCase(true)]
        [TestCase(false)]
        public void WriteToDiagnostics_When_Called_Considers_UtcTime_Flag_When_Creating_Log_Model(bool useUtcTime)
        {
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager(new Mock<ILogIdGenerator>().Object, useUtcTime);
            //
            var logDestinationMock = new Mock<ILogDestination>();
            //
            diagnosticsManager.SetDependencies(logDestinationMock.Object, "logger name");
            // Time provider
            var timeProviderMock = new Mock<ITimeProvider>();
            var logDateLocal = new DateTime(2020, 1, 2);
            var logDateUtc = new DateTime(2020, 1, 3);
            timeProviderMock.Setup(x => x.GetLocalTime()).Returns(logDateLocal);
            timeProviderMock.Setup(x => x.GetUtcTime()).Returns(logDateUtc);
            diagnosticsManager.ResetTimeProvider(timeProviderMock.Object);

            // Act
            diagnosticsManager.WriteToDiagnostics(new Log());

            // Assert
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(m => m[0].LogDate == ((useUtcTime) ? logDateUtc : logDateLocal))), Times.Once);
        }
        [Test]
        public void WriteToDiagnostics_When_Called_Given_That_No_Destination_Was_Specified_Does_Not_Throw()
        {
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager();

            LogItem item = LogItem.FromObj("name", "value");
            Log log = Log.Critical("description1").From("context1").With(item);

            // Act
            diagnosticsManager.WriteToDiagnostics(log);
        }

        #endregion


        #region Tests for WriteToDiagnosticsAsync method

        [Test]
        public async Task WriteToDiagnosticsAsync_When_Called_Creates_Log_Model_And_Passes_It_To_Log_Destination()
        {
            // Log id generator
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync("log id");
            //
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager(logIdGeneratorMock.Object, false);
            //
            var logDestinationMock = new Mock<ILogDestination>();
            var loggerName = "logger name";
            //
            diagnosticsManager.SetDependencies(logDestinationMock.Object, loggerName);

            // Time provider
            var timeProviderMock = new Mock<ITimeProvider>();
            var logDateLocal = new DateTime(2020, 1, 2);
            timeProviderMock.Setup(x => x.GetLocalTime()).Returns(logDateLocal);
            diagnosticsManager.ResetTimeProvider(timeProviderMock.Object);
            //
            LogItem item = LogItem.FromObj("name", "value");
            Log log = Log.Critical("description1").From("context1").With(item);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await diagnosticsManager.WriteToDiagnosticsAsync(log, cancellationToken);

            // Assert
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                         m[0].Context == "context1" &&
                                                                         m[0].Description == "description1" &&
                                                                         m[0].LogId == "log id" &&
                                                                         m[0].Source == loggerName &&
                                                                         m[0].IsCritical &&
                                                                         m[0].LogItems.Length == 1 &&
                                                                         m[0].LogItems[0].Name == "name" &&
                                                                         (m[0].LogItems[0].Value as string) == "value" &&
                                                                         m[0].LogDate == logDateLocal), cancellationToken), Times.Once);
        }
        [TestCase(true)]
        [TestCase(false)]
        public async Task WriteToDiagnosticsAsync_When_Called_Considers_UtcTime_Flag_When_Creating_Log_Model(bool useUtcTime)
        {
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager(new Mock<ILogIdGenerator>().Object, useUtcTime);
            //
            var logDestinationMock = new Mock<ILogDestination>();
            //
            diagnosticsManager.SetDependencies(logDestinationMock.Object, "logger name");
            // Time provider
            var timeProviderMock = new Mock<ITimeProvider>();
            var logDateLocal = new DateTime(2020, 1, 2);
            var logDateUtc = new DateTime(2020, 1, 3);
            timeProviderMock.Setup(x => x.GetLocalTime()).Returns(logDateLocal);
            timeProviderMock.Setup(x => x.GetUtcTime()).Returns(logDateUtc);
            diagnosticsManager.ResetTimeProvider(timeProviderMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await diagnosticsManager.WriteToDiagnosticsAsync(new Log(), cancellationToken);

            // Assert
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m[0].LogDate == ((useUtcTime) ? logDateUtc : logDateLocal)), cancellationToken), Times.Once);
        }
        [Test]
        public async Task WriteToDiagnosticsAsync_When_Called_Given_That_No_Destination_Was_Specified_Does_Not_Throw()
        {
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager();

            LogItem item = LogItem.FromObj("name", "value");
            Log log = Log.Critical("description1").From("context1").With(item);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await diagnosticsManager.WriteToDiagnosticsAsync(log, cancellationToken);
        }
        [Test]
        public void WriteToDiagnosticsAsync_When_Called_With_Cancelled_Token_Returns_Cancelled_Task()
        {
            // Log id generator
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            //
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager(logIdGeneratorMock.Object, false);
            //
            var logDestinationMock = new Mock<ILogDestination>();
            //
            diagnosticsManager.SetDependencies(logDestinationMock.Object, "logger name");
            // Time provider
            var timeProviderMock = new Mock<ITimeProvider>();
            diagnosticsManager.ResetTimeProvider(timeProviderMock.Object);
            // Canceled token
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await diagnosticsManager.WriteToDiagnosticsAsync(new Log(), cancellationToken);
            });
            logIdGeneratorMock.VerifyNoOtherCalls();
            logDestinationMock.VerifyNoOtherCalls();
            timeProviderMock.VerifyNoOtherCalls();
        }

        #endregion

        #region Tests for ResetTimeProvider method

        [Test]
        public void ResetTimeProvider_When_Called_Sets_Time_Provider()
        {
            // Arrange
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager();
            var timeProvider = new Mock<ITimeProvider>().Object;

            // Act
            diagnosticsManager.ResetTimeProvider(timeProvider);

            // Assert
            Assert.AreEqual(timeProvider, diagnosticsManager.TimeProvider);
        }
        [Test]
        public void ResetTimeProvider_When_Called_With_Null_Time_Provider_Throws_ArgumentNullException()
        {
            // Arrange
            DiagnosticsManager diagnosticsManager = new DiagnosticsManager();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                diagnosticsManager.ResetTimeProvider(null as ITimeProvider);
            });
            Assert.AreEqual("timeProvider", ex.ParamName);
        }

        #endregion
    }
}
