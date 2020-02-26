using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TacitusLogger.Builders;
using TacitusLogger.Contributors;
using TacitusLogger.Destinations;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class LoggerBuilderTests
    {
        #region Tests for Logger method

        [Test]
        public void Logger_With_No_Params_When_Called_Returns_Logger_Builder_Instance()
        {
            // Act
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Assert  
            Assert.NotNull(loggerBuilder);
            Assert.IsInstanceOf<LoggerBuilder>(loggerBuilder);
        }
        [Test]
        public void Logger_With_No_Params_When_Called_Initiate_All_Properties_Of_Returned_Logger_Builder_Instance()
        {
            // Act
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Assert   
            Assert.NotNull(loggerBuilder.LoggerName);

            Assert.NotNull(loggerBuilder.LogGroups);
            Assert.AreEqual(0, loggerBuilder.LogGroups.Count);
            Assert.NotNull(loggerBuilder.LogContributors);
            Assert.AreEqual(0, loggerBuilder.LogContributors.Count);
            Assert.NotNull(loggerBuilder.LogTransformers);
            Assert.AreEqual(0, loggerBuilder.LogTransformers.Count);
            Assert.IsTrue(loggerBuilder.RecordConfigurationAfterBuild);

            Assert.IsNull(loggerBuilder.LogCreationStrategy);
            Assert.IsNull(loggerBuilder.ExceptionHandlingStrategy);
            Assert.IsNull(loggerBuilder.LogLevel);
            Assert.IsNull(loggerBuilder.SelfMonitoringDestination);
        }

        [Test]
        public void Logger_With_No_Params_When_Called_Several_Times_Returns_New_Logger_Builder_Instance_Each_Time()
        {
            // Act
            ILoggerBuilder loggerBuilder1 = LoggerBuilder.Logger();
            ILoggerBuilder loggerBuilder2 = LoggerBuilder.Logger();

            // Assert  
            Assert.AreNotEqual(loggerBuilder1, loggerBuilder2);
        }
        [Test]
        public void Logger_With_No_Params_When_Called_Returns_LoggerBuilder_With_Initialized_LogGroups_Null_LogIdGenerator_Default_LoggerName()
        {
            // Act
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Assert  
            Assert.NotNull(loggerBuilder.LogGroups);
            Assert.Null(loggerBuilder.LogIdGenerator);
            Assert.NotNull(loggerBuilder.LoggerName);
            Assert.IsTrue(loggerBuilder.LoggerName.Contains("Logger_"));
            Assert.NotNull(loggerBuilder.LogContributors);
        }
        [Test]
        public void Logger_Taking_LoggerName_When_Called_Returns_LoggerBuilder_Instance_With_Specified_Logger_Name()
        {
            // Act
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger("Logger1");

            // Assert  
            Assert.NotNull(loggerBuilder);
            Assert.IsInstanceOf<LoggerBuilder>(loggerBuilder);
            Assert.AreEqual("Logger1", ((LoggerBuilder)loggerBuilder).LoggerName);
        }
        [Test]
        public void Logger_Taking_LoggerName_When_Called_Initiate_All_Properties_Of_Returned_Logger_Builder_Instance()
        {
            // Act
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger("Logger1");

            // Assert   
            Assert.AreEqual("Logger1", loggerBuilder.LoggerName);

            Assert.NotNull(loggerBuilder.LogGroups);
            Assert.AreEqual(0, loggerBuilder.LogGroups.Count);
            Assert.NotNull(loggerBuilder.LogContributors);
            Assert.AreEqual(0, loggerBuilder.LogContributors.Count);
            Assert.NotNull(loggerBuilder.LogTransformers);
            Assert.AreEqual(0, loggerBuilder.LogTransformers.Count);

            Assert.IsNull(loggerBuilder.LogCreationStrategy);
            Assert.IsNull(loggerBuilder.ExceptionHandlingStrategy);
            Assert.IsNull(loggerBuilder.LogLevel);
            Assert.IsNull(loggerBuilder.SelfMonitoringDestination);
        }

        #endregion

        #region Tests for WithLogIdGenerator method

        [Test]
        public void WithLogIdGenerator_WhenCalled_ReturnsSelf()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;

            // Act
            var self = loggerBuilder.WithLogIdGenerator(logIdGenerator);

            // Assert  
            Assert.AreEqual(loggerBuilder, self);
        }
        [Test]
        public void WithLogIdGenerator_WhenCalled_InitializesLogIdGeneratorField()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;

            // Act
            loggerBuilder.WithLogIdGenerator(logIdGenerator);

            // Assert  
            Assert.AreEqual(logIdGenerator, loggerBuilder.LogIdGenerator);
        }
        [Test]
        public void WithLogIdGenerator_WhenCalledWithNullLogIdGenerator_ThrowsAnArgumentNullException()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                loggerBuilder.WithLogIdGenerator(null);
            });
        }

        #endregion

        #region Tests for WithLogCreation method

        [Test]
        public void WithLogCreation_When_Called_Sets_Log_Creation_Strategy()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var logCreationStrategy = new Mock<LogCreationStrategyBase>().Object;

            // Act
            loggerBuilder.WithLogCreation(logCreationStrategy);

            // Assert  
            Assert.AreEqual(logCreationStrategy, loggerBuilder.LogCreationStrategy);
        }
        [Test]
        public void WithLogCreation_When_Called_Returns_Same_Logger_Builder()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            var loggerBuilderReturned = loggerBuilder.WithLogCreation(new Mock<LogCreationStrategyBase>().Object);

            // Assert  
            Assert.AreEqual(loggerBuilder, loggerBuilderReturned);
        }
        [Test]
        public void WithLogCreation_When_Called_With_Null_Log_Creation_Strategy_Throws_ArgumentNullException()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                loggerBuilder.WithLogCreation(null as LogCreationStrategyBase);
            });
            Assert.AreEqual("logCreationStrategy", ex.ParamName);
        }

        #endregion

        #region Tests for WithExceptionHandling method

        [Test]
        public void WithExceptionHandling_When_Called_Sets_Exception_Handling_Strategy()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            // Act
            loggerBuilder.WithExceptionHandling(exceptionHandlingStrategy);

            // Assert  
            Assert.AreEqual(exceptionHandlingStrategy, loggerBuilder.ExceptionHandlingStrategy);
        }
        [Test]
        public void WithExceptionHandling_When_Called_Returns_Same_Logger_Builder()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            var loggerBuilderReturned = loggerBuilder.WithExceptionHandling(new Mock<ExceptionHandlingStrategyBase>().Object);

            // Assert  
            Assert.AreEqual(loggerBuilder, loggerBuilderReturned);
        }
        [Test]
        public void WithExceptionHandling_When_Called_With_Null_Exception_Handling_Strategy_Throws_ArgumentNullException()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                loggerBuilder.WithExceptionHandling(null as ExceptionHandlingStrategyBase);
            });
            Assert.AreEqual("exceptionHandlingStrategy", ex.ParamName);
        }

        #endregion

        #region Tests for WithSelfMonitoringDestination method

        [Test]
        [Ignore("Test not implemented")]
        public void WithSelfMonitoringDestination_When_Called_Sets_Self_Monitoring_Destination()
        {

        }
        [Test]
        [Ignore("Test not implemented")]
        public void WithSelfMonitoringDestination_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {

        }
        [Test]
        [Ignore("Test not implemented")]
        public void WithSelfMonitoringDestination_When_Called_With_Null_Self_Monitoring_Destination_Throws_ArgumentNullException()
        {

        }

        #endregion

        #region Tests for NewLogGroup() method

        [Test]
        public void NewLogGroup_When_Called_Returns_ILogGroupBuilder()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            ILogGroupBuilder logGroupBuilder = loggerBuilder.NewLogGroup();

            // Assert  
            Assert.NotNull(logGroupBuilder);
        }
        [Test]
        public void NewLogGroup_When_Called_Does_Not_Add_LogGroup_To_LoggerBuilder_Immediately()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            loggerBuilder.NewLogGroup();

            // Assert  
            Assert.AreEqual(0, loggerBuilder.LogGroups.Count);
        }
        [Test]
        public void NewLogGroup_When_Called_Several_Times_Returns_Fresh_New_LogGroupBuilder_Each_Time()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            ILogGroupBuilder logGroupBuilder1 = loggerBuilder.NewLogGroup();
            ILogGroupBuilder logGroupBuilder2 = loggerBuilder.NewLogGroup();

            // Assert  
            Assert.AreNotEqual(logGroupBuilder1, logGroupBuilder2);
        }
        [Test]
        public void NewLogGroup_When_Called_Provides_LogGroupBuilder_With_BuildCallback()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            ILogGroupBuilder logGroupBuilder = loggerBuilder.NewLogGroup();

            // Assert  
            Assert.NotNull(((LogGroupBuilder)logGroupBuilder).BuildCallback);
        }

        #endregion

        #region Tests for NewLogGroup(string) method

        [Test]
        public void NewLogGroup_WithLogGroupName_WhenCalled_ReturnsLogGroupBuilderWithName()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();
            string name = "LogGroupName";

            // Act
            ILogGroupBuilder logGroupBuilder = loggerBuilder.NewLogGroup(name);

            // Assert  
            Assert.AreEqual(name, ((LogGroupBuilder)logGroupBuilder).Name);
        }
        [Test]
        public void NewLogGroup_WithLogGroupName_WhenCalled_DoesNotAddLogGroupToLoggerBuilderImmediately()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            loggerBuilder.NewLogGroup("LogGroupName");

            // Assert  
            Assert.AreEqual(0, loggerBuilder.LogGroups.Count);
        }
        [Test]
        public void NewLogGroup_WithLogGroupName_WhenCalledSeveralTimes_ReturnsFreshNewLogGroupBuilderEachTime()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            ILogGroupBuilder logGroupBuilder1 = loggerBuilder.NewLogGroup("LogGroupName");
            ILogGroupBuilder logGroupBuilder2 = loggerBuilder.NewLogGroup("LogGroupName");

            // Assert  
            Assert.AreNotEqual(logGroupBuilder1, logGroupBuilder2);
        }
        [Test]
        public void NewLogGroup_Taking_LogGroup_Name_When_Called_Provides_LogGroupBuilder_With_BuildCallback()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            ILogGroupBuilder logGroupBuilder = loggerBuilder.NewLogGroup("LogGroupName");

            // Assert
            Assert.IsInstanceOf<LogGroupBuilder>(logGroupBuilder);
            Assert.NotNull(((LogGroupBuilder)logGroupBuilder).BuildCallback);
        }

        #endregion

        #region Tests for AddLogGroup method

        [Test]
        public void AddLogGroup_WhenCalled_AddProvidedLogGroupToLogGroupsCollection()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            LogGroup logGroup = new LogGroup(x => true);

            // Act
            loggerBuilder.NewLogGroup(logGroup);

            // Assert  
            Assert.AreEqual(1, loggerBuilder.LogGroups.Count);
            Assert.AreEqual(logGroup, loggerBuilder.LogGroups[0]);
        }
        [Test]
        public void AddLogGroup_WhenCalledSeveralTimesWithSameLogGroup_AddsLogGroupToCollectionRepeatedly()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            LogGroup logGroup = new LogGroup(x => true);

            // Act
            loggerBuilder.NewLogGroup(logGroup);
            loggerBuilder.NewLogGroup(logGroup);

            // Assert  
            Assert.AreEqual(2, loggerBuilder.LogGroups.Count);
            Assert.AreEqual(logGroup, loggerBuilder.LogGroups[0]);
            Assert.AreEqual(logGroup, loggerBuilder.LogGroups[1]);
        }

        #endregion

        #region Tests for LogContributorsBuildCallback method

        [Test]
        public void LogContributorsBuildCallback_When_Called_Sets_Internal_List_With_Provided_Contributors()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var logContributorsList = new List<LogContributorBase>() { new Mock<LogContributorBase>("").Object };

            // Act
            loggerBuilder.LogContributorsBuildCallback(logContributorsList);

            // Assert
            Assert.AreEqual(logContributorsList, loggerBuilder.LogContributors);
        }
        [Test]
        public void LogContributorsBuildCallback_When_Called_Returns_LoggerBuilder()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            var loggerBuilderReturned = loggerBuilder.LogContributorsBuildCallback(new List<LogContributorBase>() { });

            // Assert
            Assert.AreEqual(loggerBuilder, loggerBuilderReturned);
        }

        #endregion

        #region Tests for Contributors method

        [Test]
        public void Contributors_When_Called_Returns_New_ContributorsBuilder()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            ILogContributorsBuilder logContributorsBuilder = loggerBuilder.Contributors();

            // Assert 
            Assert.IsInstanceOf<LogContributorsBuilder>(logContributorsBuilder);
        }
        [Test]
        public void Contributors_When_Called_Several_Times_Returns_New_ContributorsBuilder_Each_Time()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            ILogContributorsBuilder logContributorsBuilder1 = loggerBuilder.Contributors();
            ILogContributorsBuilder logContributorsBuilder2 = loggerBuilder.Contributors();

            // Assert 
            Assert.NotNull(logContributorsBuilder1);
            Assert.NotNull(logContributorsBuilder2);
            Assert.AreNotEqual(logContributorsBuilder1, logContributorsBuilder2);
        }

        #endregion

        #region Tests for Transformers method

        [Test]
        public void Transformers_When_Called_Returns_New_TransformersBuilder()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            ILogTransformersBuilder logTransformersBuilder = loggerBuilder.Transformers();

            // Assert 
            Assert.IsInstanceOf<LogTransformersBuilder>(logTransformersBuilder);
        }
        [Test]
        public void Transformers_When_Called_Several_Times_Returns_New_TransformersBuilder_Each_Time()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            ILogTransformersBuilder logTransformersBuilder1 = loggerBuilder.Transformers();
            ILogTransformersBuilder logTransformersBuilder2 = loggerBuilder.Transformers();

            // Assert 
            Assert.NotNull(logTransformersBuilder1);
            Assert.NotNull(logTransformersBuilder2);
            Assert.AreNotEqual(logTransformersBuilder1, logTransformersBuilder2);
        }

        #endregion

        #region Tests for BuildLogger method

        [Test]
        public void BuildLogger_When_Called_Creates_New_Logger()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            ILogger logger = loggerBuilder.BuildLogger();

            // Assert  
            Assert.NotNull(logger);
        }
        [Test]
        public void BuildLogger_When_Called_Several_Times_Creates_New_Logger_Each_Time()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            ILogger logger1 = loggerBuilder.BuildLogger();
            ILogger logger2 = loggerBuilder.BuildLogger();

            // Assert  
            Assert.AreNotEqual(logger1, logger2);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_No_LogId_Generator_Was_Provided_Creates_New_Logger_With_Default_LogId_Generator()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            Assert.AreEqual(Defaults.LogIdGenerator, logger.LogIdGenerator);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_No_Log_Creation_Strategy_Was_Provided_Creates_New_Logger_With_Default_Log_Creation_Strategy()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            Assert.IsInstanceOf<StandardLogCreationStrategy>(logger.LogCreationStrategy);
        }
        [Test]
        public void BuildLogger_When_Called_Sets_Provided_Log_Creation_Strategy()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();
            var logCreationStrategy = new Mock<LogCreationStrategyBase>().Object;
            loggerBuilder.WithLogCreation(logCreationStrategy);

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            Assert.AreEqual(logCreationStrategy, logger.LogCreationStrategy);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_No_Exception_Handling_Strategy_Was_Provided_Creates_New_Logger_With_Default_Exception_Handling_Strategy()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            Assert.IsInstanceOf<SilentExceptionHandlingStrategy>(logger.ExceptionHandlingStrategy);
        }
        [Test]
        public void BuildLogger_When_Called_Sets_Provided_Exception_Handling_Strategy()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();
            var exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            loggerBuilder.WithExceptionHandling(exceptionHandlingStrategy);

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            Assert.AreEqual(exceptionHandlingStrategy, logger.ExceptionHandlingStrategy);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_No_Logger_Name_Was_Provided_Creates_New_Logger_With_Default_Logger_Name()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            Assert.NotNull(logger.Name);
            Assert.IsTrue(logger.Name.Contains("Logger_"));
        }
        [Test]
        public void BuildLogger_When_Called_Initializes_Logger_Name_And_LogId_Generator_With_Ones_That_Was_Set_To_LoggerBuilder()
        {
            // Arrange
            string loggerName = "LoggerName";
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger(loggerName);

            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;

            // Act
            loggerBuilder.WithLogIdGenerator(logIdGenerator);
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            Assert.AreEqual(logIdGenerator, logger.LogIdGenerator);
            Assert.AreEqual(loggerName, logger.Name);
        }
        [Test]
        public void BuildLogger_When_Called_All_Log_Groups()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();
            LogGroup logGroup1 = new LogGroup(x => true);
            LogGroup logGroup2 = new LogGroup(x => true);

            // Act
            loggerBuilder.NewLogGroup(logGroup1);
            loggerBuilder.NewLogGroup(logGroup2);
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert   
            List<LogGroupBase> logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(2, logGroups.Count);
            Assert.AreEqual(logGroup1, logGroups[0]);
            Assert.AreEqual(logGroup2, logGroups[1]);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_No_Log_Contributors_Was_Provided_During_The_Build_Return_Logger_With_Empty_LogContributors_List()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = LoggerBuilder.Logger();

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert    
            Assert.AreEqual(0, logger.LogContributors.Count());
        }
        [Test]
        public void BuildLogger_When_Called_Return_Logger_With_Log_Contributors_List_That_Was_Created_During_Build_Process()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var logContributor = new Mock<LogContributorBase>("").Object;
            loggerBuilder.LogContributorsBuildCallback(new List<LogContributorBase>() { logContributor });

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert    
            Assert.AreEqual(1, logger.LogContributors.Count());
            Assert.AreEqual(logContributor, logger.LogContributors.First());
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_Log_Level_Value_Provider_Was_Not_Provided_Sets_To_Default()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert    
            Assert.IsInstanceOf<MutableSetting<LogLevel>>(logger.LogLevel);
            Assert.AreEqual(Defaults.LogLevel, logger.LogLevel.Value);
        }
        [Test]
        public void BuildLogger_When_Called_Creates_Logger_And_Sets_Its_Log_Level()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var logLevelValueProviderMock = new Mock<Setting<LogLevel>>();
            logLevelValueProviderMock.Setup(x => x.ToString()).Returns("");
            loggerBuilder.WithLogLevel(logLevelValueProviderMock.Object);

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert    
            Assert.AreEqual(logLevelValueProviderMock.Object, logger.LogLevel);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_Self_Monitoring_Destination_Was_Not_Set_During_The_Build_Does_Not_Sets_Loggers_Self_Monitoring_Destination()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert    
            Assert.IsNull(logger.SelfMonitoringDestination);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_Self_Monitoring_Destination_Was_Set_During_The_Build_Sets_Loggers_Self_Monitoring_Destination()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var selfMonitoringDestination = new Mock<ILogDestination>().Object;
            loggerBuilder.WithSelfMonitoring(selfMonitoringDestination);

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert    
            Assert.AreEqual(selfMonitoringDestination, logger.SelfMonitoringDestination);
        } 
        [Test]
        public void BuildLogger_When_Called_Given_That_Flag_Is_Set_To_True()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var selfMonitoringDestinationMock = new Mock<ILogDestination>();
            loggerBuilder.WithSelfMonitoring(selfMonitoringDestinationMock.Object);
            loggerBuilder.WriteLoggerConfigurationToDiagnostics(true);

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            selfMonitoringDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Once);
        }
        [Test]
        public void BuildLogger_When_Called_Given_That_Flag_Is_Set_To_False()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            var selfMonitoringDestinationMock = new Mock<ILogDestination>();
            loggerBuilder.WithSelfMonitoring(selfMonitoringDestinationMock.Object);
            loggerBuilder.WriteLoggerConfigurationToDiagnostics(false);

            // Act
            Logger logger = (Logger)loggerBuilder.BuildLogger();

            // Assert  
            selfMonitoringDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }

        #endregion

        #region Tests for WithLogLevel method

        [Test]
        public void WithLogLevel_When_Called_Sets_Log_Level_Value_Provider()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();
            Setting<LogLevel> logLevel = new Mock<Setting<LogLevel>>().Object;

            // Act
            loggerBuilder.WithLogLevel(logLevel);

            // Assert  
            Assert.AreEqual(logLevel, loggerBuilder.LogLevel);
        }
        [Test]
        public void WithLogLevel_When_Called_Returns_Same_Logger_Builder()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            var loggerBuilderReturned = loggerBuilder.WithLogLevel(new Mock<Setting<LogLevel>>().Object);

            // Assert  
            Assert.AreEqual(loggerBuilder, loggerBuilderReturned);
        }

        #endregion

        #region Tests for WriteLoggerConfigurationToDiagnostics method

        [TestCase(true)]
        [TestCase(false)]
        public void WriteLoggerConfigurationToDiagnostics_When_Called_Sets_The_Property(bool shouldWrite)
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            loggerBuilder.WriteLoggerConfigurationToDiagnostics(shouldWrite);

            // Assert
            Assert.AreEqual(shouldWrite, loggerBuilder.RecordConfigurationAfterBuild);
        }
        [Test]
        public void WriteLoggerConfigurationToDiagnostics_Returns_LoggerBuilder()
        {
            // Arrange
            LoggerBuilder loggerBuilder = (LoggerBuilder)LoggerBuilder.Logger();

            // Act
            var result = loggerBuilder.WriteLoggerConfigurationToDiagnostics(true);

            // Assert
            Assert.AreEqual(loggerBuilder, result);
        }

        #endregion
    }
}
