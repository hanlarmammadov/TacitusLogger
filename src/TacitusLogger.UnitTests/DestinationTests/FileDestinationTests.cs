using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using TacitusLogger.Serializers;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using TacitusLogger.Destinations.File;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests.DestinationTests
{
    [TestFixture]
    public class FileDestinationTests
    {
        #region Ctors tests

        [Test]
        public void Ctor_WithLogSerializerAndLogFilePathGenerator_WhenCalled_SetsLogSerializerAndLogFilePathGenerator()
        {
            // Arrange
            ILogSerializer logSerializerMock = new Mock<ILogSerializer>().Object;
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            // Act
            FileDestination fileDestination = new FileDestination(logSerializerMock, filePathGenerator);

            // Assert  
            Assert.AreEqual(logSerializerMock, fileDestination.LogSerializer);
            Assert.AreEqual(filePathGenerator, fileDestination.LogFilePathGenerator);
            Assert.NotNull(fileDestination.FileSystemFacade);
        } 
        [Test]
        public void Ctor_WithLogStringTemplateAndJsonSerializerSettingsAndLogFilePathGenerator_WhenCalled_SetsTextLogSerializerAndLogFilePathGenerator()
        {
            // Arrange
            var logStringTemplate = "template";
            var jsonSerializerSettings = new JsonSerializerSettings();
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            // Act
            FileDestination fileDestination = new FileDestination(logStringTemplate, jsonSerializerSettings, filePathGenerator);

            // Assert  
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            var logSerializer = (fileDestination.LogSerializer as ExtendedTemplateLogSerializer);
            Assert.AreEqual(logStringTemplate, logSerializer.Template);
            Assert.AreEqual(jsonSerializerSettings, logSerializer.JsonSerializerSettings);
            Assert.AreEqual(filePathGenerator, fileDestination.LogFilePathGenerator);
        }
        [Test]
        public void Ctor_WithLogStringTemplateAndLogFilePathGenerator_WhenCalled_SetsTextLogSerializerWithDefaultJsonSettingsAndLogFilePathGenerator()
        {
            // Arrange
            var logStringTemplate = "Log string template";
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            // Act
            FileDestination fileDestination = new FileDestination(logStringTemplate, filePathGenerator);

            // Assert  
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            var logSerializer = (fileDestination.LogSerializer as ExtendedTemplateLogSerializer);
            Assert.AreEqual(logStringTemplate, logSerializer.Template);
            Assert.NotNull(logSerializer.JsonSerializerSettings);
            Assert.AreEqual(filePathGenerator, fileDestination.LogFilePathGenerator);
        }
        [Test]
        public void Ctor_WithLogFilePathGenerator_WhenCalled_SetsTextLogSerializerWithDefaultsAndLogFilePathGenerator()
        {
            // Arrange 
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            // Act
            FileDestination fileDestination = new FileDestination(filePathGenerator);

            // Assert   
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(filePathGenerator, fileDestination.LogFilePathGenerator);
        }
        [Test]
        public void Ctor_WithLogSerializerAndFilePathTemplate_WhenCalled_SetsTextLogSerializerAndTemplateBasedLogFilePathGeneratorWithFilePathTemplate()
        {
            // Arrange 
            string filePathTemplate = "file path template";
            ILogSerializer logSerializer = new Mock<ILogSerializer>().Object;

            // Act
            FileDestination fileDestination = new FileDestination(logSerializer, filePathTemplate);

            // Assert   
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<FilePathTemplateLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathTemplate, (fileDestination.LogFilePathGenerator as FilePathTemplateLogSerializer).Template);
            Assert.AreEqual(logSerializer, fileDestination.LogSerializer);
        }
        [Test]
        public void Ctor_With_LogStringTemplate_JsonSettings_And_FilePathTemplate_WhenCalled_Sets_ExtendedTemplateLogSerializer_And_TemplateBasedLogFilePathGenerator()
        {
            // Arrange  
            var logStringTemplate = "logStringTemplate";
            var jsonSerializerSettings = new JsonSerializerSettings();
            var filePathTemplate = "filePathTemplate";

            // Act
            FileDestination fileDestination = new FileDestination(logStringTemplate, jsonSerializerSettings, filePathTemplate);

            // Assert  

            //TextLogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(logStringTemplate, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).Template);
            Assert.AreEqual(jsonSerializerSettings, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).JsonSerializerSettings);

            //TemplateBasedLogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<FilePathTemplateLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathTemplate, (fileDestination.LogFilePathGenerator as FilePathTemplateLogSerializer).Template);
        }
        [Test]
        public void Ctor_WithLogStringTemplateAndFilePathTemplate_WhenCalled_SetsExtendedTemplateLogSerializerAndTemplateBasedLogFilePathGenerator()
        {
            // Arrange  
            var logStringTemplate = "logStringTemplate";
            var filePathTemplate = "filePathTemplate";

            // Act
            FileDestination fileDestination = new FileDestination(logStringTemplate, filePathTemplate);

            // Assert  

            //TextLogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(logStringTemplate, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).Template);
            Assert.NotNull((fileDestination.LogSerializer as ExtendedTemplateLogSerializer).JsonSerializerSettings);

            //TemplateBasedLogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<FilePathTemplateLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathTemplate, (fileDestination.LogFilePathGenerator as FilePathTemplateLogSerializer).Template);
        }
        [Test]
        public void Ctor_WithFilePathTemplate_WhenCalled_SetsExtendedTemplateLogSerializerWithdefaultsAndTemplateBasedLogFilePathGenerator()
        {
            // Arrange
            var filePathTemplate = "filePathTemplate";

            // Act
            FileDestination fileDestination = new FileDestination(filePathTemplate);

            // Assert  

            //LogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(ExtendedTemplateLogSerializer.DefaultTemplate, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).Template);
            Assert.NotNull((fileDestination.LogSerializer as ExtendedTemplateLogSerializer).JsonSerializerSettings);

            //LogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<FilePathTemplateLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathTemplate, (fileDestination.LogFilePathGenerator as FilePathTemplateLogSerializer).Template);
        }
        [Test]
        public void Ctor_Default_WhenCalled_SetsExtendedTemplateLogSerializerWithDefaultsAndTemplateBasedLogFilePathGeneratorWithDefaults()
        {
            // Act
            FileDestination fileDestination = new FileDestination();

            // Assert  

            //LogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(ExtendedTemplateLogSerializer.DefaultTemplate, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).Template);
            Assert.NotNull((fileDestination.LogSerializer as ExtendedTemplateLogSerializer).JsonSerializerSettings);

            //LogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<FilePathTemplateLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(FilePathTemplateLogSerializer.DefaultTemplate, (fileDestination.LogFilePathGenerator as FilePathTemplateLogSerializer).Template);
        }
        [Test]
        public void Ctor_WithLogStringFactoryMethodAndFilePathTemplate_WhenCalled_SetsGeneratorFunctionLogSerializerAndTemplateBasedLogFilePathGeneratorWithDefaults()
        {
            // Act
            LogModelFunc<string> generatorFunc = (ld) => "log model string";
            string filePathTemplate = "filePathTemplate";

            FileDestination fileDestination = new FileDestination(generatorFunc, filePathTemplate);

            // Assert  

            //LogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(generatorFunc, (fileDestination.LogSerializer as GeneratorFunctionLogSerializer).GeneratorFunction);

            //LogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<FilePathTemplateLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathTemplate, (fileDestination.LogFilePathGenerator as FilePathTemplateLogSerializer).Template);
        }
        [Test]
        public void Ctor_WithLogStringTemplateAndFilePathFactoryMethod_WhenCalled_SetsExtendedTemplateLogSerializerAndDelegateBasedLogFilePathGenerator()
        {
            // Act 
            string logStringTemplate = "logStringTemplate";
            LogModelFunc<string> filePathDeleg = (ld) => "file path string";

            FileDestination fileDestination = new FileDestination(logStringTemplate, filePathDeleg);

            // Assert  

            //LogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(logStringTemplate, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).Template);
            Assert.NotNull((fileDestination.LogSerializer as ExtendedTemplateLogSerializer).JsonSerializerSettings);

            //LogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathDeleg, (fileDestination.LogFilePathGenerator as GeneratorFunctionLogSerializer).GeneratorFunction);
        }
        [Test]
        public void Ctor_WithLogStringTemplateJsonSettingsFilePathFactoryMethod_WhenCalled_SetsExtendedTemplateLogSerializerAndDelegateBasedLogFilePathGenerator()
        {
            // Arrange 
            string logStringTemplate = "logStringTemplate";
            LogModelFunc<string> filePathDeleg = (ld) => "file path string";
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            FileDestination fileDestination = new FileDestination(logStringTemplate, jsonSerializerSettings, filePathDeleg);

            // Assert   
            // LogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<ExtendedTemplateLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(logStringTemplate, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).Template);
            Assert.AreEqual(jsonSerializerSettings, (fileDestination.LogSerializer as ExtendedTemplateLogSerializer).JsonSerializerSettings);

            // LogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathDeleg, (fileDestination.LogFilePathGenerator as GeneratorFunctionLogSerializer).GeneratorFunction);
        }
        [Test]
        public void Ctor_WithLogStringFactoryMethodFilePathFactoryMethod_WhenCalled_SetsGeneratorFunctionLogSerializerAndDelegateBasedLogFilePathGenerator()
        {
            // Arrange  
            LogModelFunc<string> logStringDeleg = (ld) => "file path string";
            LogModelFunc<string> filePathDeleg = (ld) => "file path string";

            // Act
            FileDestination fileDestination = new FileDestination(logStringDeleg, filePathDeleg);

            // Assert  

            //LogSerializer asserts
            Assert.NotNull(fileDestination.LogSerializer);
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(fileDestination.LogSerializer);
            Assert.AreEqual(logStringDeleg, (fileDestination.LogSerializer as GeneratorFunctionLogSerializer).GeneratorFunction);

            //LogFilePathGenerator asserts
            Assert.NotNull(fileDestination.LogFilePathGenerator);
            Assert.IsInstanceOf<GeneratorFunctionLogSerializer>(fileDestination.LogFilePathGenerator);
            Assert.AreEqual(filePathDeleg, (fileDestination.LogFilePathGenerator as GeneratorFunctionLogSerializer).GeneratorFunction);
        }
        [Test]
        public void Ctor_WithLogSerializerAndLogFilePathGenerator_WhenCalledWithNullLogSerializer_ThrowsArgumentNullException()
        {
            // Arrange 
            ILogSerializer filePathGenerator = new Mock<ILogSerializer>().Object;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                FileDestination fileDestination = new FileDestination(null as ILogSerializer, filePathGenerator);
            });
        }
        [Test]
        public void Ctor_WithLogSerializerAndLogFilePathGenerator_WhenCalledWithLogFilePathGenerator_ThrowsArgumentNullException()
        {
            // Arrange
            ILogSerializer logSerializerMock = new Mock<ILogSerializer>().Object;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                FileDestination fileDestination = new FileDestination(logSerializerMock, null as ILogSerializer);
            });
        }

        #endregion

        #region ResetFileSystemFacade tests

        [Test]
        public void ResetFileSystemFacade_WhenCalled_ResetsFileSystemFacadeFromDefaultToProvided()
        {
            // Arrange
            var fileSystemFacade = new Mock<IFileSystemFacade>().Object;

            // Act
            FileDestination fileDestination = new FileDestination();
            fileDestination.ResetFileSystemFacade(fileSystemFacade);

            // Assert  
            Assert.AreEqual(fileSystemFacade, fileDestination.FileSystemFacade);
        }

        #endregion

        #region Send tests

        [Test]
        public void Send_WhenCalled_CallsFileSystemFacadeMethodAppendToFile()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logFilePath");
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logText");

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Act
            fileDestination.Send(new LogModel[] { logModel });

            // Assert  
            fileSystemFacadeMock.Verify(x => x.AppendToFile("logFilePath", $"logText{Environment.NewLine}"), Times.Once);
        }
        [Test]
        public void Send_WhenLogFilePathGeneratorReturnsNull_ThrowsException()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(null as string);

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Assert
            Assert.Catch<Exception>(() =>
            {
                // Act
                fileDestination.Send(new LogModel[] { logModel });
            });
            logSerializerMock.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            fileSystemFacadeMock.Verify(x => x.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        [Test]
        public void Send_WhenLogSerializerReturnsNull_ThrowsException()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(null as string);

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Assert
            Assert.Catch<Exception>(() =>
            {
                // Act
                fileDestination.Send(new LogModel[] { logModel });
            });
            filePathGeneratorMock.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Once);
            fileSystemFacadeMock.Verify(x => x.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        [Test]
        public void Send_WhenCalled_NoneOfDependantsAsyncMethodsAreCalled()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logFilePath");
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logText");

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object); 

            // Act
            fileDestination.Send(new LogModel[] { logModel });

            // Assert
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void Send_When_Called_With_N_Different_Paths_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Context = $"path{i}", Description = $"logText{i}" };
            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Context);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Act
            fileDestination.Send(logs);

            // Assert 
            fileSystemFacadeMock.Verify(x => x.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                fileSystemFacadeMock.Verify(x => x.AppendToFile($"path{i}", $"logText{i}{ Environment.NewLine}"), Times.Once);
        }
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void Send_When_Called_With_N_Same_Paths_Calls_Facade_Method_Once_With_N_Concated_Log_Texts(int N)
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            StringBuilder concatedLogTexts = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Description = $"logText{i}" };
                concatedLogTexts.AppendLine($"logText{i}");
            }
            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("commonPath");
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Act
            fileDestination.Send(logs);

            // Assert 
            fileSystemFacadeMock.Verify(x => x.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            for (int i = 0; i < N; i++)
                fileSystemFacadeMock.Verify(x => x.AppendToFile("commonPath", concatedLogTexts.ToString()), Times.Once);
        }
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(11)]
        [TestCase(15)]
        public void Send_When_Called_With_N_Logs_Resulting_In_M_Paths_Calls_Facade_Method_M_Times_With_Concated_Log_Texts(int N)
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            StringBuilder[] concatedLogTexts = new StringBuilder[5];
            for (int j = 0; j < 5; j++)
                concatedLogTexts[j] = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Context = $"path{i % 5}", Description = $"logText{i}" };
                concatedLogTexts[i % 5].AppendLine($"logText{i}");
            }
            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Context);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Act
            fileDestination.Send(logs);

            // Assert 
            fileSystemFacadeMock.Verify(x => x.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(5));
            for (int j = 0; j < 5; j++)
            {
                string expectedStr = concatedLogTexts[j].ToString();
                fileSystemFacadeMock.Verify(x => x.AppendToFile($"path{j}", expectedStr), Times.Once);
            }
        }

        #endregion

        #region SendAsync tests

        [Test]
        public async Task SendAsync_WhenCalled_CallsFileSystemFacadeMethodAppendToFile()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logFilePath");
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logText");

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await fileDestination.SendAsync(new LogModel[] { logModel }, cancellationToken);

            // Assert  
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync("logFilePath", $"logText{Environment.NewLine}", cancellationToken), Times.Once);
        }
        [Test]
        public void SendAsync_WhenLogFilePathGeneratorReturnsNull_ThrowsException()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(null as string);

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Assert
            Assert.CatchAsync<Exception>(async () =>
            {
                // Act
                await fileDestination.SendAsync(new LogModel[] { logModel });
            });
            logSerializerMock.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public void SendAsync_WhenLogSerializerReturnsNull_ThrowsException()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(null as string);

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Assert
            Assert.CatchAsync<Exception>(async () =>
            {
                // Act
                await fileDestination.SendAsync(new LogModel[] { logModel });
            });

            filePathGeneratorMock.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Once);
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task SendAsync_WhenCalled_NoneOfDependantsSyncMethodsAreCalled()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logModel = Samples.LogModels.Standard(LogType.Info);

            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logFilePath");
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("logText");

            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);

            // Act
            await fileDestination.SendAsync(new LogModel[] { logModel });

            // Assert
            fileSystemFacadeMock.Verify(x => x.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_N_Different_Paths_Calls_Facade_Method_N_Times(int N)
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            for (int i = 0; i < N; i++)
                logs[i] = new LogModel() { Context = $"path{i}", Description = $"logText{i}" };
            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Context);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await fileDestination.SendAsync(logs, cancellationToken);

            // Assert 
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Exactly(N));
            for (int i = 0; i < N; i++)
                fileSystemFacadeMock.Verify(x => x.AppendToFileAsync($"path{i}", $"logText{i}{ Environment.NewLine}", cancellationToken), Times.Once);
        }
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_N_Same_Paths_Calls_Facade_Method_Once_With_N_Concated_Log_Texts(int N)
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            StringBuilder concatedLogTexts = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Description = $"logText{i}" };
                concatedLogTexts.AppendLine($"logText{i}");
            }
            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns("commonPath");
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await fileDestination.SendAsync(logs, cancellationToken);

            // Assert 
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Once);
            for (int i = 0; i < N; i++)
                fileSystemFacadeMock.Verify(x => x.AppendToFileAsync("commonPath", concatedLogTexts.ToString(), cancellationToken), Times.Once);
        }
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(9)]
        [TestCase(11)]
        [TestCase(15)]
        public async Task SendAsync_When_Called_With_N_Logs_Resulting_In_M_Paths_Calls_Facade_Method_M_Times_With_Concated_Log_Texts(int N)
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var logSerializerMock = new Mock<ILogSerializer>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logs = new LogModel[N];
            StringBuilder[] concatedLogTexts = new StringBuilder[5];
            for (int j = 0; j < 5; j++)
                concatedLogTexts[j] = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                logs[i] = new LogModel() { Context = $"path{i % 5}", Description = $"logText{i}" };
                concatedLogTexts[i % 5].AppendLine($"logText{i}");
            }
            filePathGeneratorMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Context);
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns((LogModel x) => x.Description);
            FileDestination fileDestination = new FileDestination(logSerializerMock.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await fileDestination.SendAsync(logs, cancellationToken);

            // Assert 
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Exactly(5));
            for (int j = 0; j < 5; j++)
            {
                string expectedStr = concatedLogTexts[j].ToString();
                fileSystemFacadeMock.Verify(x => x.AppendToFileAsync($"path{j}", expectedStr, cancellationToken), Times.Once);
            }
        }
        [Test]
        public void SendAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var logSerializer = new Mock<ILogSerializer>();
            FileDestination fileDestination = new FileDestination(logSerializer.Object, filePathGeneratorMock.Object);
            fileDestination.ResetFileSystemFacade(fileSystemFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert  
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await fileDestination.SendAsync(new LogModel[] { Samples.LogModels.Standard(LogType.Info) }, cancellationToken);
            });
            logSerializer.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            filePathGeneratorMock.Verify(x => x.Serialize(It.IsAny<LogModel>()), Times.Never);
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Destination()
        {
            // Arrange
            FileDestination fileDestination = new FileDestination();

            // Act
            var result = fileDestination.ToString();

            // Arrange
            Assert.NotNull(result);
            Assert.IsTrue(result.Contains("TacitusLogger.Destinations.File.FileDestination"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Serializer()
        {
            // Arrange
            var logSerializerMock = new Mock<ILogSerializer>();
            var logSerializerDescription = "logSerializerDescription";
            logSerializerMock.Setup(x => x.ToString()).Returns(logSerializerDescription);
            FileDestination fileDestination = new FileDestination(logSerializerMock.Object);

            // Act
            var result = fileDestination.ToString();

            // Arrange
            logSerializerMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logSerializerDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_File_Path_Generator()
        {
            // Arrange
            var filePathGeneratorMock = new Mock<ILogSerializer>();
            var filePathGeneratorDescription = "filePathGeneratorDescription";
            filePathGeneratorMock.Setup(x => x.ToString()).Returns(filePathGeneratorDescription);
            FileDestination fileDestination = new FileDestination(filePathGeneratorMock.Object);

            // Act
            var result = fileDestination.ToString();

            // Arrange
            filePathGeneratorMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(filePathGeneratorDescription));
        }

        #endregion
    }
}
