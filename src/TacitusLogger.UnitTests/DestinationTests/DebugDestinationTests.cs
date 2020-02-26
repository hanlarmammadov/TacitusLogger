using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Destinations;
using TacitusLogger.Serializers;
using System.Threading.Tasks;
using System.Threading;
using TacitusLogger.Destinations.Debug;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.DestinationTests
{
    [TestFixture]
    public class DebugDestinationTests
    {
        #region Ctors tests

        [Test]
        public void Ctor_WithLogSerializer_WhenCalled_LogSerializerAndConsoleFacadeAreSetRight()
        {
            // Arrange
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            DebugDestination debugDestination = new DebugDestination(logSerializer);

            // Assert  
            Assert.AreEqual(logSerializer, debugDestination.LogSerializer);
            Assert.IsInstanceOf<DebugConsoleFacade>(debugDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogSerializer_WhenCalledWithNullLogSerializer_ThrowsArgumentNullException()
        {

            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                DebugDestination debugDestination = new DebugDestination(null as ILogSerializer);
            });
        }

        [Test]
        public void Ctor_Default_WhenCalled_LogSerializerIsSetToDefault()
        {
            // Act
            DebugDestination debugDestination = new DebugDestination();

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(debugDestination.LogSerializer);
            Assert.IsInstanceOf<DebugConsoleFacade>(debugDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogStringTemplate_WhenCalled_LogSerializerIsSetToTextLogSerializerWithProvidedTemplate()
        {
            // Arrange
            var logTemplate = "sampleLogTemplate";

            // Act
            DebugDestination debugDestination = new DebugDestination(logTemplate);

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(debugDestination.LogSerializer);
            Assert.AreEqual(logTemplate, (debugDestination.LogSerializer as SimpleTemplateLogSerializer).Template);
            Assert.IsInstanceOf<DebugConsoleFacade>(debugDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogStringTemplate_WhenCalledWithNullLogStringTemplate_ThrowsArgumentIsNullException()
        {
            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                DebugDestination debugDestination = new DebugDestination(null as string);
            });
        }

        [Test]
        public void Ctor_WithLogModelFunc_WhenCalled_LogSerializerIsSetToGeneratorFunctionLogSerializerWithProvidedDeligate()
        {
            // Arrange
            LogModelFunc<string> generatorFunc = (ld) => "";

            // Act
            DebugDestination debugDestination = new DebugDestination(generatorFunc);

            // Assert  
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(debugDestination.LogSerializer);
            Assert.AreEqual(generatorFunc, (debugDestination.LogSerializer as GeneratorFunctionLogSerializer).GeneratorFunction);
            Assert.IsInstanceOf<DebugConsoleFacade>(debugDestination.ConsoleFacade);
        }

        [Test]
        public void Ctor_WithLogModelFunc_WhenCalledWithNullDelegate_ThrowsArgumentIsNullException()
        {
            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                DebugDestination debugDestination = new DebugDestination(null as LogModelFunc<string>);
            });
        }

        #endregion

        #region ResetConsole tests

        [Test]
        public void ResetConsole_WhenCalled_ResetsConsoleFacadeFromDefaultToProvided()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>().Object;

            // Act

            DebugDestination debugDestination = new DebugDestination();
            debugDestination.ResetConsoleFacade(consoleFacadeMock);

            // Assert  
            Assert.AreEqual(consoleFacadeMock, debugDestination.ConsoleFacade);
        }

        #endregion

        #region Send tests

        [Test]
        public void Send_WhenCalled_ProvidesConsoleFacadeWithGeneratedLogText()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();

            var logModel = Samples.LogModels.Standard(LogType.Info);

            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("resulting text");

            DebugDestination debugDestination = new DebugDestination(logSerializer.Object);
            debugDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            debugDestination.Send(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine("resulting text"), Times.Once);
        }
        
        [Test]
        public void Send_WhenCalled_NoneOfDependantsAsyncMethodsAreCalled()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            DebugDestination debugDestination = new DebugDestination();
            debugDestination.ResetConsoleFacade(consoleFacadeMock.Object); 

            // Act
            debugDestination.Send(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void Send_When_Called_With_Collection_Of_N_Logs_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var debugConsoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Description = $"logText{i}" };

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            DebugDestination debugDestination = new DebugDestination(logSerializerMock.Object);
            debugDestination.ResetConsoleFacade(debugConsoleFacadeMock.Object);

            // Act
            debugDestination.Send(logs);

            // Assert  
            debugConsoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                debugConsoleFacadeMock.Verify(x => x.WriteLine($"logText{i}"), Times.Once);
        }

        #endregion

        #region SendAsync tests

        [Test]
        public async Task SendAsync_WhenCalled_ProvidesConsoleFacadeWithGeneratedLogText()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();

            var logModel = Samples.LogModels.Standard(LogType.Info);

            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("resulting text");

            DebugDestination debugDestination = new DebugDestination(logSerializer.Object);
            debugDestination.ResetConsoleFacade(consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await debugDestination.SendAsync(new LogModel[] { logModel }, cancellationToken);

            // Assert
            consoleFacadeMock.Verify(x => x.WriteLineAsync("resulting text", cancellationToken), Times.Once);
        }

        [Test]
        public void SendAsync_WhenLogSerializerReturnsNull_ThrowsAnException()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(null as string);
            var logModel = Samples.LogModels.Standard(LogType.Info);

            DebugDestination debugDestination = new DebugDestination(logSerializer.Object);
            debugDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Assert
            Assert.CatchAsync<Exception>(async () =>
            {
                // Act
                await debugDestination.SendAsync(new LogModel[] { logModel });
            });
        }
        
        [Test]
        public async Task SendAsync_WhenCalled_NoneOfDependantsSyncMethodsAreCalled()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            DebugDestination debugDestination = new DebugDestination();
            debugDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            await debugDestination.SendAsync(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_Collection_Of_N_Logs_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var debugConsoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Description = $"logText{i}" };

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            DebugDestination debugDestination = new DebugDestination(logSerializerMock.Object);
            debugDestination.ResetConsoleFacade(debugConsoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await debugDestination.SendAsync(logs);

            // Assert  
            debugConsoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<string>(), cancellationToken), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                debugConsoleFacadeMock.Verify(x => x.WriteLineAsync($"logText{i}", cancellationToken), Times.Once);
        }

        [Test]
        public void SendAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            DebugDestination debugDestination = new DebugDestination(logSerializer.Object);
            debugDestination.ResetConsoleFacade(consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert  
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await debugDestination.SendAsync(new LogModel[] { Samples.LogModels.Standard(LogType.Info) }, cancellationToken);
            });
            logSerializer.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<String>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Returns_Information_About_The_Destination()
        {
            // Arrange
            DebugDestination debugDestination = new DebugDestination();

            // Act
            var result = debugDestination.ToString();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Destinations.Debug.DebugDestination")); 
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Serializer()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            var logSerializerDescription = "logSerializerDescription";
            logSerializerMock.Setup(x => x.ToString()).Returns(logSerializerDescription);
            DebugDestination debugDestination = new DebugDestination(logSerializerMock.Object);

            // Act
            var result = debugDestination.ToString();

            // Arrange
            logSerializerMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logSerializerDescription));
        }

        #endregion
    }
}
