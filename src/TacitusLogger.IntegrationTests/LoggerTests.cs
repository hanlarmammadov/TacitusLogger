using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Builders;
using TacitusLogger.Destinations;
using TacitusLogger.Destinations.Console;
using TacitusLogger.Destinations.File;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class LoggerTests : IntegrationTestBase
    {
        #region Logger with one console log destination for all logs

        [Test]
        public void Logger_WithOneConsoleDestinationWithDefaultsForAllLogs_SendsLogToConsoleFacadeSuccessfully()
        {
            //Build log
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneConsoleDestinationWithLogSerializerForAllLogs_SendsLogToConsoleFacadeSuccessfully()
        {
            //Build log
            var logSerializerMocks = LogSerializerThatReturnsPredefinedString("serialized log");
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().WithLogSerializer(logSerializerMocks.Object).Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine("serialized log", It.IsAny<ConsoleColor>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneConsoleDestinationWithTextLogSerializerForAllLogs_SendsLogToConsoleFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().WithSimpleTemplateLogText("$Context and $Description").Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description1", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine("Context1 and Description1", It.IsAny<ConsoleColor>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneConsoleDestinationWithGeneratorFunctionLogSerializerForAllLogs_SendsLogToConsoleFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().WithGeneratorFuncLogText(x => x.Context).Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine("Context1", It.IsAny<ConsoleColor>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneConsoleDestinationWithDefaultColorSchemeForAllLogs_SendsLogToConsoleFacadeSuccessfully()
        {
            //Build log 
            var colorScheme = ConsoleDestination.GetDefaultColorScheme();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);


            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>(), colorScheme[LogType.Event]), Times.Once);
        }

        [Test]
        public void Logger_WithOneConsoleDestinationWithCustomColorSchemeForAllLogs_SendsLogToConsoleFacadeSuccessfully()
        {
            //Build log 
            var customColorScheme = Samples.ColorSchemes.Custom();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().WithCustomColors(customColorScheme).Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsAny<string>(), customColorScheme[LogType.Event]), Times.Once);
        }

        #endregion

        #region Logger with one file log destination for all logs

        [Test]
        public void Logger_WithOneFileDestinationWithDefaultsForAllLogs_SendsLogToFileFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().File().Add().BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default file system facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            fileSystemFacadeMock.Verify(x => x.AppendToFile(GetDefaultLogFilePath(), It.IsNotNull<string>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneFileDestinationWithLogSerializerForAllLogs_SendsLogToFileFacadeSuccessfully()
        {
            //Build log
            var logSerializerMocks = LogSerializerThatReturnsPredefinedString("serialized log");
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().File().WithLogSerializer(logSerializerMocks.Object).Add().BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default file system facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            fileSystemFacadeMock.Verify(x => x.AppendToFile(GetDefaultLogFilePath(), "serialized log" + Environment.NewLine), Times.Once);
        }

        [Test]
        public void Logger_WithOneFileDestinationWithTextLogSerializerForAllLogs_SendsLogToFileFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().File().WithExtendedTemplateLogText("$Context and $Description").Add().BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default file system facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description1", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            fileSystemFacadeMock.Verify(x => x.AppendToFile(GetDefaultLogFilePath(), "Context1 and Description1" + Environment.NewLine), Times.Once);
        }

        [Test]
        public void Logger_WithOneFileDestinationWithGeneratorFunctionLogSerializerForAllLogs_SendsLogToFileFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().File().WithGeneratorFuncLogText(x => x.Context).Add().BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default file system facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            fileSystemFacadeMock.Verify(x => x.AppendToFile(GetDefaultLogFilePath(), "Context1" + Environment.NewLine), Times.Once);
        }

        [Test]
        public void Logger_WithOneFileDestinationWithPathForAllLogs_SendsLogToFileFacadeSuccessfully()
        {
            //Build log 
            var filePathGenerator = LogFilePathGeneratorThatReturnsPredefinedString("file path");
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().File().WithPath(filePathGenerator.Object).Add().BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default file system facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            fileSystemFacadeMock.Verify(x => x.AppendToFile("file path", It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneFileDestinationWithTemplateBasedLogFilePathGeneratorForAllLogs_SendsLogToFileFacadeSuccessfully()
        {
            //Build log  
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().File().WithPath("$Context.log").Add().BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default file system facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            fileSystemFacadeMock.Verify(x => x.AppendToFile("Context1.log", It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneFileDestinationWithDelegateBasedLogFilePathGeneratorForAllLogs_SendsLogToFileFacadeSuccessfully()
        {
            //Build log  
            LogModelFunc<string> generatorFunc = x => $"{x.Context}.log";
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().File().WithPath(generatorFunc).Add().BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default file system facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            fileSystemFacadeMock.Verify(x => x.AppendToFile("Context1.log", It.IsAny<string>()), Times.Once);
        }

        #endregion

        #region Logger with one debug log destination for all logs

        [Test]
        public void Logger_WithOneDebugDestinationWithDefaultsForAllLogs_SendsLogToDebugFacadeSuccessfully()
        {
            //Build log
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Debug().Add().BuildLogger();
            //Create facade mocks 
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default debug facade with mock
            ResetFacadeForDebugDestinations(logger.LogGroups.First(), debugFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
        }

        [Test]
        public void Logger_WithOneDebugDestinationWithLogSerializerForAllLogs_SendsLogToDebugFacadeSuccessfully()
        {
            //Build log
            var logSerializerMocks = LogSerializerThatReturnsPredefinedString("serialized log");
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Debug().WithLogSerializer(logSerializerMocks.Object).Add().BuildLogger();
            //Create facade mocks 
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default debug facade with mock
            ResetFacadeForDebugDestinations(logger.LogGroups.First(), debugFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            debugFacadeMock.Verify(x => x.WriteLine("serialized log"), Times.Once);
        }

        [Test]
        public void Logger_WithOneDebugDestinationWithTextLogSerializerForAllLogs_SendsLogToDebugFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Debug().WithSimpleTemplateLogText("$Context and $Description").Add().BuildLogger();
            //Create facade mocks 
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default debug facade with mock
            ResetFacadeForDebugDestinations(logger.LogGroups.First(), debugFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description1", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            debugFacadeMock.Verify(x => x.WriteLine("Context1 and Description1"), Times.Once);
        }

        [Test]
        public void Logger_WithOneDebugDestinationWithGeneratorFunctionLogSerializerForAllLogs_SendsLogToDebugFacadeSuccessfully()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Debug().WithGeneratorFuncLogText(x => x.Context).Add().BuildLogger();
            //Create facade mocks 
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default debug facade with mock
            ResetFacadeForDebugDestinations(logger.LogGroups.First(), debugFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            debugFacadeMock.Verify(x => x.WriteLine("Context1"), Times.Once);
        }

        #endregion
        
        #region Logger with one TextWriter log destination for all logs

        [Test]
        public void Logger_WithOneTextWriterDestinationWithDefaultsForAllLogs_SendsLogToTextWriterSuccessfully()
        {
            //Build log
            var textWriterMock = new Mock<TextWriter>();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().TextWriter().WithWriter(textWriterMock.Object).Add().BuildLogger();

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            textWriterMock.Verify(x => x.Write(It.IsNotNull<string>()), Times.Once);
        }

        [Test]
        public void Logger_With_One_TextWriter_Destination_With_Custom_Log_Data_Serializer_For_All_Logs_Sends_Log_To_TextWriter_Successfully()
        {
            //Build log
            var textWriterMock = new Mock<TextWriter>();
            var logSerializerMocks = LogSerializerThatReturnsPredefinedString("serialized log");
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().TextWriter()
                                                                       .WithWriter(textWriterMock.Object)
                                                                       .WithLogSerializer(logSerializerMocks.Object)
                                                                       .Add()
                                                                       .BuildLogger();
            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            textWriterMock.Verify(x => x.Write("serialized log" + Environment.NewLine), Times.Once);
        }

        [Test]
        public void Logger_With_One_TextWriter_Destination_With_MultiLineLogText_For_All_Logs_Sends_Log_To_TextWriter_Successfully()
        {
            //Build log 
            var textWriterMock = new Mock<TextWriter>();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                          .TextWriter()
                                                          .WithWriter(x => textWriterMock.Object)
                                                          .WithExtendedTemplateLogText("$Context and $Description")
                                                          .Add()
                                                          .BuildLogger();
            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description1", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            textWriterMock.Verify(x => x.Write("Context1 and Description1"+Environment.NewLine), Times.Once);
        }

        [Test]
        public void Logger_With_One_TextWriter_Destination_With_GeneratorFunctionLogSerializer_For_All_Logs_Sends_Log_To_TextWriter_Successfully()
        {
            //Build log 
            var textWriterMock = new Mock<TextWriter>();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                          .TextWriter()
                                                          .WithWriter(x => textWriterMock.Object)
                                                          .WithGeneratorFuncLogText(x => x.Context)
                                                          .Add()
                                                          .BuildLogger();
            //Call Logger's method to send log
            string logId = logger.Log("Context1", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            textWriterMock.Verify(x => x.Write("Context1" + Environment.NewLine), Times.Once);
        }


        #endregion

        #region Logger with several log destinations for all logs

        [Test]
        public void Logger_With_Console_File_TextWriter_Destinations_With_Defaults_For_All_Logs_Sends_Logs_Successfully()
        {
            //Build log
            var textWriterMock = new Mock<TextWriter>();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().Add()
                                                                       .File().Add()
                                                                       .TextWriter().WithWriter(textWriterMock.Object).Add()
                                                                       .BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var fileFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            string defaultFilePath = $".{Path.DirectorySeparatorChar}Logs-{DateTime.Now.ToString("dd-MMM-yyyy")}.log";
            fileFacadeMock.Verify(x => x.AppendToFile(defaultFilePath, It.IsNotNull<string>()), Times.Once);
            textWriterMock.Verify(x => x.Write(It.IsNotNull<string>()), Times.Once);
        }

        [Test]
        public void Logger_With_Console_And_Debug_Destinations_With_Defaults_For_All_Logs_Sends_Log_To_Facades_Successfully()
        {
            //Build log
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().Console().Add()
                                                                       .Debug().Add()
                                                                       .BuildLogger();
            //Create facade mocks
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            //Replace default console facades with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.LogGroups.First(), debugFacadeMock.Object);

            //Call Logger's method to send log
            string logId = logger.Log("Context", LogType.Event, "Description", new { });

            // Assert 
            Assert.IsTrue(IsValidGuidString(logId));
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
        }

        #endregion

        #region Logger with several log groups

        [Test]
        public void Logger_With_Two_Log_Groups_Each_Including_Two_Log_Destinations_Sends_Log_To_Facades_Successfully()
        {
            // Arrange
            var textWriterMock = new Mock<TextWriter>();
            //Build logger with two log groups
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("Group1").ForRule(x => x.Context == "context1").Console().Add()
                                                                                                                      .Debug().Add()
                                                                                                                      .TextWriter().WithWriter(textWriterMock.Object).Add()
                                                                                                                      .BuildLogGroup()
                                                          .NewLogGroup("Group2").ForRule(x => x.Context == "context2").File().Add().BuildLogGroup()
                                                          .BuildLogger();

            //Create facade mocks
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            var fileFacadeMock = new Mock<IFileSystemFacade>();

            //Replace default facades with mocks 
            ResetFacadeForConsoleDestinations(logger.GetLogGroup("Group1"), consoleFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.GetLogGroup("Group1"), debugFacadeMock.Object);
            ResetFacadeForFileDestinations(logger.GetLogGroup("Group2"), fileFacadeMock.Object);


            logger.Log("context1", LogType.Event, "Description", new { });

            //Should be called once at this point
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
            textWriterMock.Verify(x => x.Write(It.IsNotNull<string>()), Times.Once);
            //Should not be called at this point
            fileFacadeMock.Verify(x => x.AppendToFile(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Never);

            // Act
            logger.Log("context2", LogType.Event, "Description", new { });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
            textWriterMock.Verify(x => x.Write(It.IsNotNull<string>()), Times.Once);
            fileFacadeMock.Verify(x => x.AppendToFile(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Once);
        }

        [Test]
        public void Logger_With_Two_Log_Groups_Each_Including_Two_Log_Destinations_And_One_Of_Log_Groups_Is_Inactive_Sends_Log_To_Facades_Of_Destinations_Of_Only_Active_LogGroup()
        {
            // Arrange
            //Build logger with two log groups
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("Group1").ForRule(x => x.Context != null).Console().Add().Debug().Add().BuildLogGroup()
                                                          .NewLogGroup("Group2").SetStatus(LogGroupStatus.Inactive).ForRule(x => x.Context != null).File().Add().BuildLogGroup()
                                                          .BuildLogger();

            //Create facade mocks
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            var fileFacadeMock = new Mock<IFileSystemFacade>();

            //Replace default facades with mocks 
            ResetFacadeForConsoleDestinations(logger.GetLogGroup("Group1"), consoleFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.GetLogGroup("Group1"), debugFacadeMock.Object);
            ResetFacadeForFileDestinations(logger.GetLogGroup("Group2"), fileFacadeMock.Object);

            // Act
            logger.Log("context1", LogType.Event, "Description", new { });

            // Assert
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
            fileFacadeMock.Verify(x => x.AppendToFile(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Never);
        }
         
        [Test]
        public void Logger_WithTwoLogGroupsEachIncludingOneFileDestinationWithLogSerializerAndFilePathGen_SendsLogToFacadesSuccessfully()
        {
            // Arrange
            var logSerializer1 = LogSerializerThatReturnsPredefinedString("resultFromFirstSerializer");
            var logSerializer2 = LogSerializerThatReturnsPredefinedString("resultFromSecondSerializer");
            var filePathGenerator1 = LogFilePathGeneratorThatReturnsPredefinedString("resultFromFirstFilePathGenerator");
            var filePathGenerator2 = LogFilePathGeneratorThatReturnsPredefinedString("resultFromSecondFilePathGenerator");

            //Build logger with two log groups
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("Group1").ForRule(x => x.Context == "context1").File().WithLogSerializer(logSerializer1.Object)
                                                                                                                      .WithPath(filePathGenerator1.Object)
                                                                                                                      .Add().BuildLogGroup()
                                                          .NewLogGroup("Group2").ForRule(x => x.Context == "context2").File().WithLogSerializer(logSerializer2.Object)
                                                                                                                      .WithPath(filePathGenerator2.Object)
                                                                                                                      .Add().BuildLogGroup()
                                                          .BuildLogger();
            //Create facade mocks 
            var fileFacade1Mock = new Mock<IFileSystemFacade>();
            var fileFacade2Mock = new Mock<IFileSystemFacade>();

            //Replace default facades with mocks 
            ResetFacadeForFileDestinations(logger.GetLogGroup("Group1"), fileFacade1Mock.Object);
            ResetFacadeForFileDestinations(logger.GetLogGroup("Group2"), fileFacade2Mock.Object);

            // Act 1
            logger.Log("context1", LogType.Event, "Description", new { });

            //Should be called once at this point 
            fileFacade1Mock.Verify(x => x.AppendToFile("resultFromFirstFilePathGenerator", "resultFromFirstSerializer" + Environment.NewLine), Times.Once);
            //Should not be called at this point
            fileFacade2Mock.Verify(x => x.AppendToFile("resultFromSecondFilePathGenerator", "resultFromSecondSerializer" + Environment.NewLine), Times.Never);

            // Act 2
            logger.Log("context2", LogType.Event, "Description", new { });

            //Should be called once at this point 
            fileFacade1Mock.Verify(x => x.AppendToFile("resultFromFirstFilePathGenerator", "resultFromFirstSerializer" + Environment.NewLine), Times.Once);
            //Should not be called at this point
            fileFacade2Mock.Verify(x => x.AppendToFile("resultFromSecondFilePathGenerator", "resultFromSecondSerializer" + Environment.NewLine), Times.Once);
        }

        [Test]
        public async Task Logger_With_Two_Log_Groups_Each_Including_Two_Log_Destinations_Calls_Facade_Methods_Expected_Times()
        {
            // Arrange
            //Build logger with two log groups
            var logSerializerMock = LogSerializerThatReturnsPredefinedString("expectedLogText");
            var filePathGeneratorMock = LogFilePathGeneratorThatReturnsPredefinedString("expectedFilePath");

            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("Group1").ForRule(x => x.LogType == LogType.Info)
                                                                                .File().WithPath(filePathGeneratorMock.Object)
                                                                                       .WithLogSerializer(logSerializerMock.Object).Add()
                                                                                .Console().WithLogSerializer(logSerializerMock.Object).Add()
                                                                                .BuildLogGroup()
                                                                                .BuildLogger();

            //Create facade mocks
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();

            //Replace default facades with mocks 
            ResetFacadeForFileDestinations(logger.GetLogGroup("Group1"), fileSystemFacadeMock.Object);
            ResetFacadeForConsoleDestinations(logger.GetLogGroup("Group1"), consoleFacadeMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act 
            for (int i = 0; i < 30; i++)
                await logger.LogAsync("context1", LogType.Info, "Description", new { }, cancellationToken);

            // Assert 
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync("expectedFilePath", "expectedLogText" + Environment.NewLine, cancellationToken), Times.Exactly(30));
            consoleFacadeMock.Verify(x => x.WriteLineAsync("expectedLogText", It.IsAny<ConsoleColor>(), cancellationToken), Times.Exactly(30));
        }

        #endregion

        #region Various

        [Test]
        public void Logger_When_Calling_Extension_Log_Methods_Sends_Logs_To_Destinations_Successfully()
        {
            //Build log 
            var logDestinationMock = new Mock<ILogDestination>();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().CustomDestination(logDestinationMock.Object).BuildLogger();

            // Act
            logger.LogSuccess("Success", "Description", new { });
            logger.LogInfo("Info", "Description", new { });
            logger.LogEvent("Event", "Description", new { });
            logger.LogWarning("Warning", "Description", new { });
            logger.LogFailure("Failure", "Description", new { });
            logger.LogError("Error", "Description", new { });
            logger.LogCritical("Critical", "Description", new { });

            // Assert  
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].Context == "Success" && d[0].LogType == LogType.Success)), Times.Once);
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].Context == "Info" && d[0].LogType == LogType.Info)), Times.Once);
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].Context == "Event" && d[0].LogType == LogType.Event)), Times.Once);
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].Context == "Warning" && d[0].LogType == LogType.Warning)), Times.Once);
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].Context == "Failure" && d[0].LogType == LogType.Failure)), Times.Once);
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].Context == "Error" && d[0].LogType == LogType.Error)), Times.Once);
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].Context == "Critical" && d[0].LogType == LogType.Critical)), Times.Once);
        }

        [Test]
        public async Task Logger_When_Calling_Async_Extension_Log_Methods_Sends_Logs_To_Destinations_Successfully()
        {
            //Build log 
            var logDestinationMock = new Mock<ILogDestination>();
            Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs().CustomDestination(logDestinationMock.Object).BuildLogger();

            // Act
            await logger.LogSuccessAsync("Success", "Description", new { });
            await logger.LogInfoAsync("Info", "Description", new { });
            await logger.LogEventAsync("Event", "Description", new { });
            await logger.LogWarningAsync("Warning", "Description", new { });
            await logger.LogFailureAsync("Failure", "Description", new { });
            await logger.LogErrorAsync("Error", "Description", new { });
            await logger.LogCriticalAsync("Critical", "Description", new { });

            // Assert  
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0].Context == "Success" && d[0].LogType == LogType.Success), default), Times.Once);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0].Context == "Info" && d[0].LogType == LogType.Info), default), Times.Once);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0].Context == "Event" && d[0].LogType == LogType.Event), default), Times.Once);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0].Context == "Warning" && d[0].LogType == LogType.Warning), default), Times.Once);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0].Context == "Failure" && d[0].LogType == LogType.Failure), default), Times.Once);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0].Context == "Error" && d[0].LogType == LogType.Error), default), Times.Once);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d[0].Context == "Critical" && d[0].LogType == LogType.Critical), default), Times.Once);
        }

        [Test]
        public async Task Logger_When_Log_Called_With_LoggerName_And_Template_Containing_Source_Sends_Log_Text_Containing_Source_To_Facade()
        {
            //Build log
            string loggerName = "logger1";
            Logger logger = (Logger)LoggerBuilder.Logger(loggerName).ForAllLogs().Console().WithExtendedTemplateLogText("$Source").Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);

            //Call Logger's method to send log
            logger.Log("Context", LogType.Event, "Description", new { });
            await logger.LogAsync("Context", LogType.Event, "Description", new { });

            // Assert  
            consoleFacadeMock.Verify(x => x.WriteLine(loggerName, It.IsAny<ConsoleColor>()), Times.Once);
            consoleFacadeMock.Verify(x => x.WriteLineAsync(loggerName, It.IsAny<ConsoleColor>(), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task Logger_With_Custom_Log_Id_Generator_Generates_Right_Log_Id()
        {
            //Build log
            var logIdGenerator = LogIdGeneratorThatReturnsPredefinedString("log id");
            Logger logger = (Logger)LoggerBuilder.Logger().WithLogIdGenerator(logIdGenerator.Object).ForAllLogs().Console().WithSimpleTemplateLogText("$LogId").Add().BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);

            //Call Logger's method to send log
            var logId = logger.Log("Context", LogType.Event, "Description", new { });
            var logIdAsync = await logger.LogAsync("Context", LogType.Event, "Description", new { });

            // Assert  
            Assert.IsTrue(logId == logIdAsync);
            Assert.AreEqual("log id", logId);
            consoleFacadeMock.Verify(x => x.WriteLine("log id", It.IsAny<ConsoleColor>()), Times.Once);
            consoleFacadeMock.Verify(x => x.WriteLineAsync("log id", It.IsAny<ConsoleColor>(), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task Logger_With_Custom_Log_Text_Template_Generates_Right_Log_Text()
        {
            //Build log
            var logIdGenerator = LogIdGeneratorThatReturnsPredefinedString("logId1");
            Logger logger = (Logger)LoggerBuilder.Logger("Logger1").WithLogIdGenerator(logIdGenerator.Object)
                                                          .ForAllLogs().Console()
                                                                       .WithExtendedTemplateLogText("$LogId-$Source-$Context-$LogType-$Description-$LogDate(dd.MM.yyyy)")
                                                                       .Add()
                                                          .BuildLogger();
            //Create facade mocks 
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            //Replace default console facade with mock
            ResetFacadeForConsoleDestinations(logger.LogGroups.First(), consoleFacadeMock.Object);
            object loggingObj = new { someField = "some value" };
            string loggingObjJson = Newtonsoft.Json.JsonConvert.SerializeObject(loggingObj);

            //Call Logger's method to send log
            var logId = logger.Log("Context1", LogType.Event, "Description1", loggingObj);
            var logIdAsync = await logger.LogAsync("Context1", LogType.Event, "Description1", loggingObj);

            // Assert 
            var expectedLogString = $"logId1-Logger1-Context1-Event-Description1-{DateTime.Now.ToString("dd.MM.yyy")}";
            consoleFacadeMock.Verify(x => x.WriteLine(expectedLogString, It.IsAny<ConsoleColor>()), Times.Once);
            consoleFacadeMock.Verify(x => x.WriteLineAsync(expectedLogString, It.IsAny<ConsoleColor>(), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task Logger_With_Custom_Log_File_Path_Template_Generates_Right_Log_File_Path()
        {
            //Build log 
            Logger logger = (Logger)LoggerBuilder.Logger("Logger1").ForAllLogs().File()
                                                                                .WithPath("$Source-$Context-$LogType-$LogDate(dd.MM.yyyy)")
                                                                                .Add()
                                                                   .BuildLogger();
            //Create facade mocks 
            var fileSystemFacadeMock = new Mock<IFileSystemFacade>();
            //Replace default console facade with mock
            ResetFacadeForFileDestinations(logger.LogGroups.First(), fileSystemFacadeMock.Object);
            object loggingObj = new { someField = "some value" };

            //Call Logger's method to send log
            logger.Log("Context1", LogType.Event, "Description1", loggingObj);
            await logger.LogAsync("Context1", LogType.Event, "Description1", loggingObj);

            // Assert 
            var expectedLogString = $"Logger1-Context1-Event-{DateTime.Now.ToString("dd.MM.yyy")}";
            fileSystemFacadeMock.Verify(x => x.AppendToFile(expectedLogString, It.IsAny<string>()), Times.Once);
            fileSystemFacadeMock.Verify(x => x.AppendToFileAsync(expectedLogString, It.IsAny<string>(), default(CancellationToken)), Times.Once);
        }

        #endregion
                 
        #region Logger description

        [Test]
        public void ToString_When_Called_Returns_Comprehensive_Description_Of_Logger_Configuration()
        {

            MutableSetting<LogLevel> logLevel = LogLevel.Error;
            // Arrange
            Logger logger = LoggerBuilder.Logger().WithGuidLogId("N", 5)
                                                  .WithGuidLogId()
                                                  .WithLogLevel(logLevel)
                                                  .WithExceptionHandling(ExceptionHandling.Log)
                                                  .WithDiagnostics(new ConsoleDestination())
                                                  .Contributors()
                                                      .StackTrace()
                                                  .BuildContributors()
                                                  .Transformers()
                                                      .StringsManual((ref string s) => { }, true, "First transformer")
                                                      .StringsManual((ref string s) => { }, true, "Second transformer")
                                                  .BuildTransformers()
                                                  .ForAllLogs()
                                                      .File().WithPath("./log.txt")
                                                             .WithExtendedTemplateLogText("Some template")
                                                             .Add()
                                                      .Console().WithExtendedTemplateLogText()
                                                                .Add()
                                                      .TextWriter().WithWriter(Console.Out)
                                                                   .Add()
                                                  .BuildLogGroup()
                                                  .NewLogGroup("group2")
                                                  .WithCaching(10, 20000, true)
                                                  .WithFirstSuccessDestinationFeeding()
                                                  .ForErrorLogs()
                                                      .File().WithPath("./log.txt")
                                                             .Add()
                                                      .Console().Add()
                                                  .BuildLogGroup()
                                                  .BuildLogger();

            // Act
            string str = logger.ToString();

            // Assert
            Assert.IsNotNull(str); 
        }


        #endregion

    }
}
