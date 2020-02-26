using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;
using TacitusLogger.Destinations.Console;
using TacitusLogger.Serializers;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.DestinationTests
{
    [TestFixture]
    public class ConsoleDestinationTests : UnitTestBase
    {
        #region Ctors tests

        [Test]
        public void MaxParamConstructor_WhenCalled_LogSerializerAndColorSchemeAreSetRight()
        {
            // Arrange
            SimpleTemplateLogSerializer logSerializer = new SimpleTemplateLogSerializer();
            var colorScheme = Samples.ColorSchemes.Custom();

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializer, colorScheme);

            // Assert  
            Assert.AreEqual(logSerializer, consoleDestination.LogSerializer);
            Assert.AreEqual(colorScheme, consoleDestination.ColorScheme);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        [Test]
        public void ConstructorWithOnlyLogSerializer_WhenCalled_ColorSchemeIsSetToDefault()
        {
            // Arrange
            SimpleTemplateLogSerializer logSerializer = new SimpleTemplateLogSerializer();

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializer);

            // Assert  
            Assert.AreEqual(logSerializer, consoleDestination.LogSerializer);
            Assert.AreEqual(ConsoleDestination.GetDefaultColorScheme(), consoleDestination.ColorScheme);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        [Test]
        public void ParameterlessConstructor_WhenCalled_LogSerializerAndColorSchemeAreSetToDefault()
        {
            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination();

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(consoleDestination.LogSerializer);
            Assert.AreEqual(ConsoleDestination.GetDefaultColorScheme(), consoleDestination.ColorScheme);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        [Test]
        public void ConstructorWithLogTemplateAndColorScheme_WhenCalled_LogTemplateAreUserForSerializerandColorSchemeIsSet()
        {
            // Arrange
            var colorScheme = Samples.ColorSchemes.Custom();
            var logTemplate = "sampleLogTemplate";

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination(logTemplate, colorScheme);

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(consoleDestination.LogSerializer);
            Assert.AreEqual(logTemplate, (consoleDestination.LogSerializer as SimpleTemplateLogSerializer).Template);
            Assert.AreEqual(colorScheme, consoleDestination.ColorScheme);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        [Test]
        public void ConstructorWithLogTemplate_WhenCalled_LogTemplateArePassedForLogSerializerAndColorSchemeIsSetToDefault()
        {
            // Arrange
            var logTemplate = "sampleLogTemplate";

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination(logTemplate);

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(consoleDestination.LogSerializer);
            SimpleTemplateLogSerializer textLogSerializer = (consoleDestination.LogSerializer as SimpleTemplateLogSerializer);
            Assert.AreEqual(logTemplate, textLogSerializer.Template);
            Assert.AreEqual(ConsoleDestination.GetDefaultColorScheme(), consoleDestination.ColorScheme);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        [Test]
        public void ConstructorWithLogTemplateAndColorScheme_WhenCalled_LogSerializerIsSetToDefault()
        {
            // Arrange
            var logTemplate = "sampleLogTemplate";
            var colorScheme = Samples.ColorSchemes.Custom();

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination(logTemplate, colorScheme);

            // Assert  
            Assert.IsInstanceOf<SimpleTemplateLogSerializer>(consoleDestination.LogSerializer);
            SimpleTemplateLogSerializer textLogSerializer = (consoleDestination.LogSerializer as SimpleTemplateLogSerializer);
            Assert.AreEqual(logTemplate, textLogSerializer.Template);
            Assert.AreEqual(colorScheme, consoleDestination.ColorScheme);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        [Test]
        public void ConstructorWithLogStringFactoryMethod_WhenCalled_LogSerializerIsSetToGeneratorFunctionLogSerializer()
        {
            // Arrange
            LogModelFunc<string> generatorFunc = x => "test";

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination(generatorFunc);

            // Assert  
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(consoleDestination.LogSerializer);
            GeneratorFunctionLogSerializer generatorFunctionLogSerializer = (consoleDestination.LogSerializer as GeneratorFunctionLogSerializer);
            Assert.AreEqual(generatorFunc, generatorFunctionLogSerializer.GeneratorFunction);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        [Test]
        public void ConstructorWithLogStringFactoryMethodAndColorScheme_WhenCalled_LogSerializerIsSetToGeneratorFunctionLogSerializerAndColorSchemeProvided()
        {
            // Arrange
            LogModelFunc<string> generatorFunc = x => "test";
            var colorScheme = Samples.ColorSchemes.Custom();

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination(generatorFunc, colorScheme);

            // Assert  
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(consoleDestination.LogSerializer);
            GeneratorFunctionLogSerializer generatorFunctionLogSerializer = (consoleDestination.LogSerializer as GeneratorFunctionLogSerializer);
            Assert.AreEqual(generatorFunc, generatorFunctionLogSerializer.GeneratorFunction);
            Assert.AreEqual(colorScheme, consoleDestination.ColorScheme);
            Assert.IsInstanceOf<StandardColorConsoleFacade>(consoleDestination.ConsoleFacade);
        }

        #endregion

        #region GetDefaultColorScheme tests

        [Test]
        public void DefaultColorSchemeShouldBeConsistent()
        {
            // Act
            var defaultColorScheme = ConsoleDestination.GetDefaultColorScheme();

            // Assert
            Assert.NotNull(defaultColorScheme);
            Assert.AreEqual(7, defaultColorScheme.Count);
            Assert.AreEqual(defaultColorScheme[LogType.Success], ConsoleColor.DarkGreen);
            Assert.AreEqual(defaultColorScheme[LogType.Info], ConsoleColor.DarkCyan);
            Assert.AreEqual(defaultColorScheme[LogType.Event], ConsoleColor.Cyan);
            Assert.AreEqual(defaultColorScheme[LogType.Warning], ConsoleColor.DarkYellow);
            Assert.AreEqual(defaultColorScheme[LogType.Failure], ConsoleColor.DarkMagenta);
            Assert.AreEqual(defaultColorScheme[LogType.Error], ConsoleColor.Red);
            Assert.AreEqual(defaultColorScheme[LogType.Critical], ConsoleColor.DarkRed);
        }

        #endregion

        #region ResetConsole tests

        [Test]
        public void ResetConsole_WhenCalled_ResetsConsoleFacadeFromDefaultToProvided()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>().Object;

            // Act
            ConsoleDestination consoleDestination = new ConsoleDestination();
            consoleDestination.ResetConsoleFacade(consoleFacadeMock);

            // Assert  
            Assert.AreEqual(consoleFacadeMock, consoleDestination.ConsoleFacade);
        }

        #endregion

        #region Send tests

        [Test]
        public void Send_WhenCalled_ProvidesConsoleFacadeWithGeneratedLogText()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("resulting text");
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializer.Object);
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            consoleDestination.Send(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine("resulting text", It.IsAny<ConsoleColor>()), Times.Once);
        }

        [Test]
        public void Send_WhenCalled_NoneOfDependantsAsyncMethodsAreCalled()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            ConsoleDestination consoleDestination = new ConsoleDestination();
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            consoleDestination.Send(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<string>(), It.IsAny<ConsoleColor>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void Send_When_Called_Given_That_Custom_Color_Scheme_Does_Not_Contain_The_Color_For_Given_LogType_Does_Not_Break_And_Provides_Facade_With_Default_Color()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Error);
            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("");
            var colorSchemeWithRemovedKey = Samples.ColorSchemes.Custom();
            colorSchemeWithRemovedKey.Remove(LogType.Error);
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializer.Object, colorSchemeWithRemovedKey);
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            consoleDestination.Send(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>(), ConsoleColor.White), Times.Once);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void Send_When_Called_With_Collection_Of_N_Logs_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Description = $"logText{i}", LogType = LogType.Error };

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializerMock.Object);
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            consoleDestination.Send(logs);

            // Assert 
            var defaultColorScheme = ConsoleDestination.GetDefaultColorScheme();
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>(), It.IsAny<ConsoleColor>()), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                consoleFacadeMock.Verify(x => x.WriteLine($"logText{i}", defaultColorScheme[logs[i].LogType]), Times.Once);
        }

        #endregion

        #region SendAsync tests

        [Test]
        public async Task SendAsync_When_Called_Provides_Console_Facade_With_Generated_Log_Text()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("resulting text");
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializer.Object);
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await consoleDestination.SendAsync(new LogModel[] { logModel }, cancellationToken);

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLineAsync("resulting text", It.IsAny<ConsoleColor>(), cancellationToken), Times.Once);
        }

        [Test]
        public async Task SendAsync_WhenCalled_NoneOfDependantsSyncMethodsAreCalled()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logModel = Samples.LogModels.Standard(LogType.Info);
            ConsoleDestination consoleDestination = new ConsoleDestination();
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);

            // Act
            await consoleDestination.SendAsync(new LogModel[] { logModel });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>(), It.IsAny<ConsoleColor>()), Times.Never);
        }

        [Test]
        public async Task SendAsync_WhenCalledGivenThatCustomColorSchemeDoesNotContainTheColorForGivenLogType_DoesNotBreakAndProvidesFacadeWithDefaultColor()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Error);
            logSerializer.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("");
            var colorSchemeWithRemovedKey = Samples.ColorSchemes.Custom();
            colorSchemeWithRemovedKey.Remove(LogType.Error);
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializer.Object, colorSchemeWithRemovedKey);
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object); 
            CancellationToken cancellationToken = new CancellationToken();
             
            // Act
            await consoleDestination.SendAsync(new LogModel[] { logModel }, cancellationToken);

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<string>(), ConsoleColor.White, cancellationToken), Times.Once);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_Collection_Of_N_Logs_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Description = $"logText{i}", LogType = LogType.Error };

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializerMock.Object);
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await consoleDestination.SendAsync(logs, cancellationToken);

            // Assert 
            var defaultColorScheme = ConsoleDestination.GetDefaultColorScheme();
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<string>(), It.IsAny<ConsoleColor>(), cancellationToken), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                consoleFacadeMock.Verify(x => x.WriteLineAsync($"logText{i}", defaultColorScheme[logs[i].LogType], cancellationToken), Times.Once);
        }

        [Test]
        public void SendAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var logSerializer = new Mock<ILogSerializer>();
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializer.Object);
            consoleDestination.ResetConsoleFacade(consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert  
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await consoleDestination.SendAsync(new LogModel[] { Samples.LogModels.Standard(LogType.Info) }, cancellationToken);
            });
            logSerializer.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            consoleFacadeMock.Verify(x => x.WriteLineAsync(It.IsAny<String>(), It.IsAny<ConsoleColor>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Destination()
        {
            // Arrange
            ConsoleDestination consoleDestination = new ConsoleDestination();

            // Act
            var result = consoleDestination.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Destinations.Console.ConsoleDestination"));
        } 
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Serializer()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            var logSerializerDescription = "logSerializerDescription";
            logSerializerMock.Setup(x => x.ToString()).Returns(logSerializerDescription);
            ConsoleDestination consoleDestination = new ConsoleDestination(logSerializerMock.Object);

            // Act
            var result = consoleDestination.ToString();

            // Arrange
            logSerializerMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logSerializerDescription));
        }

        #endregion
    }
}
