using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using TacitusLogger.Builders;
using TacitusLogger.Destinations;
using TacitusLogger.Destinations.File;

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class LogContributorTests : IntegrationTestBase
    {
        [Test]
        public void Logger_Containing_Several_Log_Contributors_And_One_Log_Group_With_Several_Custom_Destinations()
        {
            // Arrange 
            // Build logger with two log groups
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            Logger logger = (Logger)LoggerBuilder.Logger().Contributors().StackTrace()
                                                                         .BuildContributors()
                                                          .NewLogGroup().ForAllLogs()
                                                                        .CustomDestination(logDestination1Mock.Object)
                                                                        .CustomDestination(logDestination2Mock.Object)
                                                                        .BuildLogGroup()
                                                                        .BuildLogger();
            // Act
            logger.Log("context1", LogType.Event, "Description", new { });

            // Assert  
            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].LogItems.FirstOrDefault(a => a.Name == "Stack trace") != null)), Times.Once);

            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].LogItems.FirstOrDefault(a => a.Name == "Stack trace") != null)), Times.Once);
        }


        [Test]
        public void Logger_Containing_Several_Log_Contributors_And_One_Log_Group_With_Several_Destinations()
        {
            // Arrange 
            // Build logger with two log groups
            Logger logger = (Logger)LoggerBuilder.Logger().Contributors()
                                                              .StackTrace()
                                                          .BuildContributors()
                                                          .NewLogGroup("Group1").ForAllLogs()
                                                                                .Console().WithExtendedTemplateLogText().Add()
                                                                                .Debug().WithExtendedTemplateLogText().Add()
                                                                                .File().WithExtendedTemplateLogText().Add()
                                                                                .BuildLogGroup()
                                                                                .BuildLogger();
            // Create facade mocks
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            var fileFacadeMock = new Mock<IFileSystemFacade>();

            // Replace default facades with mocks 
            ResetFacadeForConsoleDestinations(logger.GetLogGroup("Group1"), consoleFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.GetLogGroup("Group1"), debugFacadeMock.Object);
            ResetFacadeForFileDestinations(logger.GetLogGroup("Group1"), fileFacadeMock.Object);

            // Act
            logger.Log("context1", LogType.Event, "Description", new { });

            // Assert  
            // These destinations provided with logs with greedy strategy.
            consoleFacadeMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Stack trace")), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Stack trace"))), Times.Once);
            fileFacadeMock.Verify(x => x.AppendToFile(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("Stack trace"))), Times.Once);
        }

        [Test]
        public void Logger_Containing_Several_Log_Contributors_And_Two_Log_Groups_With_Several_Custom_Destinations()
        {
            // Arrange 
            // Build logger with two log groups
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            var logDestination4Mock = new Mock<ILogDestination>();
            Logger logger = (Logger)LoggerBuilder.Logger().Contributors()
                                                              .StackTrace()
                                                          .BuildContributors()
                                                          .NewLogGroup()
                                                          .ForAllLogs()
                                                              .CustomDestination(logDestination1Mock.Object)
                                                              .CustomDestination(logDestination2Mock.Object)
                                                          .BuildLogGroup()
                                                          .NewLogGroup()
                                                          .ForAllLogs()
                                                              .CustomDestination(logDestination3Mock.Object)
                                                              .CustomDestination(logDestination4Mock.Object)
                                                          .BuildLogGroup()
                                                          .BuildLogger();
            // Act
            logger.Log("context1", LogType.Event, "Description", new { });

            // Assert  
            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].LogItems.FirstOrDefault(a => a.Name == "Stack trace") != null)), Times.Once);

            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].LogItems.FirstOrDefault(a => a.Name == "Stack trace") != null)), Times.Once);

            logDestination3Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].LogItems.FirstOrDefault(a => a.Name == "Stack trace") != null)), Times.Once);

            logDestination4Mock.Verify(x => x.Send(It.Is<LogModel[]>(d => d[0].LogItems.FirstOrDefault(a => a.Name == "Stack trace") != null)), Times.Once);
        }
    }
}
