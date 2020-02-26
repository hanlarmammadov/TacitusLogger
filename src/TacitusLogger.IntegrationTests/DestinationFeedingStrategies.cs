using Moq;
using NUnit.Framework;
using System; 
using TacitusLogger.Builders;
using TacitusLogger.Destinations;
using TacitusLogger.Destinations.File; 

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class DestinationFeedingStrategies : IntegrationTestBase
    {
        [Test]
        public void Logger_With_One_Log_Group_With_Greedy_Strategy()
        {
            // Arrange 
            // Build logger with two log groups
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("GreedyGroup").WithGreedyDestinationFeeding()
                                                                                     .ForAllLogs()
                                                                                     .Console().Add()
                                                                                     .Debug().Add()
                                                                                     .File().Add()
                                                                                     .BuildLogGroup()
                                                          .BuildLogger();
            // Create facade mocks
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            debugFacadeMock.Setup(x => x.WriteLine(It.IsAny<string>())).Throws(new Exception());
            var fileFacadeMock = new Mock<IFileSystemFacade>();

            // Replace default facades with mocks 
            ResetFacadeForConsoleDestinations(logger.GetLogGroup("GreedyGroup"), consoleFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.GetLogGroup("GreedyGroup"), debugFacadeMock.Object);
            ResetFacadeForFileDestinations(logger.GetLogGroup("GreedyGroup"), fileFacadeMock.Object);

            // Act
            logger.Log("context1", LogType.Event, "Description", new { });

            // Assert  
            // These destinations provided with logs with greedy strategy.
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
            fileFacadeMock.Verify(x => x.AppendToFile(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Once);
        }

        [Test]
        public void Logger_With_One_Log_Group_With_First_Success_Strategy()
        {
            // Arrange 
            // Build logger with two log groups
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("FirstSuccessGroup")
                                                          .WithFirstSuccessDestinationFeeding()
                                                          .ForAllLogs()
                                                              .Console().Add()
                                                              .Debug().Add()
                                                              .File().Add()
                                                          .BuildLogGroup()
                                                          .BuildLogger();
            // Create facade mocks
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            consoleFacadeMock.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<ConsoleColor>())).Throws(new Exception());
            var debugFacadeMock = new Mock<IOutputDeviceFacade>(); 
            var fileFacadeMock = new Mock<IFileSystemFacade>();

            // Replace default facades with mocks 
            ResetFacadeForConsoleDestinations(logger.GetLogGroup("FirstSuccessGroup"), consoleFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.GetLogGroup("FirstSuccessGroup"), debugFacadeMock.Object);
            ResetFacadeForFileDestinations(logger.GetLogGroup("FirstSuccessGroup"), fileFacadeMock.Object);

            // Act
            logger.Log("context1", LogType.Event, "Description", new { });

            // Assert  
            // These destinations provided with logs with greedy strategy.
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
            fileFacadeMock.Verify(x => x.AppendToFile(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Never);
        }

        [Test]
        public void Logger_With_Two_Log_Groups_One_With_Greedy_Strategy_Another_With_First_Success_Strategy()
        {
            // Arrange 
            // Build logger with two log groups
            Logger logger = (Logger)LoggerBuilder.Logger().NewLogGroup("Group1").WithGreedyDestinationFeeding()
                                                                                .ForAllLogs()
                                                                                .Console().Add()
                                                                                .Debug().Add()
                                                                                .BuildLogGroup()
                                                          .NewLogGroup("Group2").WithFirstSuccessDestinationFeeding()
                                                                                .ForAllLogs() 
                                                                                .Debug().Add()
                                                                                .File().Add()
                                                                                .BuildLogGroup()
                                                          .BuildLogger();

            // Create facade mocks
            var consoleFacadeMock = new Mock<IColoredOutputDeviceFacade>();
            var debugFacadeMock = new Mock<IOutputDeviceFacade>();
            var debug2FacadeMock = new Mock<IOutputDeviceFacade>();
            var fileFacadeMock = new Mock<IFileSystemFacade>();

            // Replace default facades with mocks 
            ResetFacadeForConsoleDestinations(logger.GetLogGroup("Group1"), consoleFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.GetLogGroup("Group1"), debugFacadeMock.Object);
            ResetFacadeForFileDestinations(logger.GetLogGroup("Group2"), fileFacadeMock.Object);
            ResetFacadeForDebugDestinations(logger.GetLogGroup("Group2"), debug2FacadeMock.Object);

            // Act
            logger.Log("context1", LogType.Event, "Description", new { });

            // Assert  
            // These destinations provided with logs with greedy strategy.
            consoleFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>(), It.IsAny<ConsoleColor>()), Times.Once);
            debugFacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
            // These destinations provided with logs with first success strategy.
            debug2FacadeMock.Verify(x => x.WriteLine(It.IsNotNull<string>()), Times.Once);
            fileFacadeMock.Verify(x => x.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
