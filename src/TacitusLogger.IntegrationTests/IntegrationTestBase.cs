using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TacitusLogger.Destinations;
using TacitusLogger.Destinations.Console;
using TacitusLogger.Destinations.Debug;
using TacitusLogger.Destinations.File;
using TacitusLogger.LogIdGenerators; 
using TacitusLogger.Serializers;

namespace TacitusLogger.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected bool IsValidGuidString(string str)
        {
            Guid guid = new Guid(str);
            return guid != Guid.Empty;
        }
        protected private void ResetFacadeForConsoleDestinations(LogGroupBase logGroup, IColoredOutputDeviceFacade consoleFacade)
        {
            foreach (ILogDestination dest in ((LogGroup)logGroup).LogDestinations)
                if (dest is ConsoleDestination)
                    (dest as ConsoleDestination).ResetConsoleFacade(consoleFacade);
        }
        protected private void ResetFacadeForFileDestinations(LogGroupBase logGroup, IFileSystemFacade fileSystemFacade)
        {
            foreach (ILogDestination dest in ((LogGroup)logGroup).LogDestinations)
                if (dest is FileDestination)
                    (dest as FileDestination).ResetFileSystemFacade(fileSystemFacade);
        }
        protected private void ResetFacadeForDebugDestinations(LogGroupBase logGroup, IOutputDeviceFacade consoleFacade)
        {
            foreach (ILogDestination dest in ((LogGroup)logGroup).LogDestinations)
                if (dest is DebugDestination)
                    (dest as DebugDestination).ResetConsoleFacade(consoleFacade);
        }
        protected private Mock<ILogSerializer> LogSerializerThatReturnsPredefinedString(string str)
        {
            Mock<ILogSerializer> logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(str);
            return logSerializerMock;
        }
        protected private Mock<ILogSerializer> LogFilePathGeneratorThatReturnsPredefinedString(string str)
        {
            Mock<ILogSerializer> logSerializerMock = new Mock<ILogSerializer>();
            logSerializerMock.Setup(x => x.Serialize(It.IsAny<LogModel>())).Returns(str);
            return logSerializerMock;
        }
        protected private Mock<ILogIdGenerator> LogIdGeneratorThatReturnsPredefinedString(string str)
        {
            Mock<ILogIdGenerator> logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.Generate(It.IsAny<LogModel>())).Returns(str);
            logIdGeneratorMock.Setup(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(str);
            return logIdGeneratorMock;
        }
        protected string GetDefaultLogFilePath()
        { 
            return $".{Path.DirectorySeparatorChar}Logs-{DateTime.Now.ToString("dd-MMM-yyyy")}.log"; 
        }
        protected string GetTempFolderPath(string folderName)
        {
            return Directory.CreateDirectory($"{Path.GetTempPath()}{Path.DirectorySeparatorChar}{folderName}").FullName;
        }
        protected string GenerateRandomTempFilePath(string folderName)
        { 
            var str = $"{GetTempFolderPath(folderName)}{Path.DirectorySeparatorChar}{Guid.NewGuid().ToString("N").Substring(0, 9)}.log";
            return str;
        }
        protected string GenerateRandomFolderName()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 9);
        }
    }
}
