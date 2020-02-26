using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Components.Time;
using TacitusLogger.Destinations;
using TacitusLogger.Exceptions;
using TacitusLogger.Contributors;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;
using TacitusLogger.Transformers;
using TacitusLogger.Builders;
using TacitusLogger.Destinations.Console;
using TacitusLogger.Caching;
using TacitusLogger.Diagnostics;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LoggerTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Sets_Provided_Data()
        {
            // Arrange
            string loggerName = "logger name";
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            LoggerSettings loggerSettings = new LoggerSettings();

            // Act
            Logger logger = new Logger(loggerName, logIdGenerator, loggerSettings);

            // Assert
            Assert.AreEqual(loggerName, logger.Name);
            Assert.AreEqual(logIdGenerator, logger.LogIdGenerator);
            Assert.AreEqual(loggerSettings, logger.LoggerSettings);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Initializes_Log_Groups()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings());

            // Assert 
            Assert.IsNotNull(logger.LogGroups);
            Assert.AreEqual(0, logger.LogGroups.Count());
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Initializes_Log_Contributors()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings());

            // Assert 
            Assert.IsNotNull(logger.LogContributors);
            Assert.AreEqual(0, logger.LogContributors.Count());
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Initializes_Log_Transformers()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings());

            // Assert 
            Assert.IsNotNull(logger.LogTransformers);
            Assert.AreEqual(0, logger.LogTransformers.Count());
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Sets_Log_Creation_Strategy()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings());

            // Assert 
            Assert.IsNotNull(logger.LogCreationStrategy);
            Assert.IsInstanceOf<StandardLogCreationStrategy>(logger.LogCreationStrategy);
            StandardLogCreationStrategy logCreationStrategy = (StandardLogCreationStrategy)logger.LogCreationStrategy;
            Assert.AreEqual(logger.LogContributors, logCreationStrategy.LogContributors);
            Assert.AreEqual(logger.LogIdGenerator, logCreationStrategy.LogIdGenerator);
            Assert.IsInstanceOf<SystemTimeProvider>(logCreationStrategy.TimeProvider);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Sets_Diagnostics_Manager()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings());

            // Assert 
            Assert.IsInstanceOf<DiagnosticsManager>(logger.DiagnosticsManager);
            Assert.AreEqual(logger.DiagnosticsManager, logger.ExceptionHandlingStrategy.DiagnosticsManager);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Sets_Exception_Handling()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings() { ExceptionHandling = ExceptionHandling.Rethrow });

            // Assert 
            Assert.IsNotNull(logger.ExceptionHandlingStrategy);
            Assert.IsInstanceOf<RethrowExceptionHandlingStrategy>(logger.ExceptionHandlingStrategy);
            RethrowExceptionHandlingStrategy exceptionHandlingStrategy = (RethrowExceptionHandlingStrategy)logger.ExceptionHandlingStrategy;
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_Sets_Log_Level()
        {
            // Act
            var logLevelValueProvider = Setting<LogLevel>.From.Variable(LogLevel.Warning);
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings() { LogLevel = logLevelValueProvider });

            // Assert 
            Assert.AreEqual(logLevelValueProvider, logger.LogLevel);
            Assert.AreEqual(LogLevel.Warning, logger.LogLevel.Value);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_With_Settings_Containing_Null_LogLevelValueProvider_Sets_Log_Level()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, new LoggerSettings() { LogLevel = null });

            // Assert 
            Assert.NotNull(logger.LogLevel);
            Assert.IsInstanceOf<MutableSetting<LogLevel>>(logger.LogLevel);
            Assert.AreEqual(Defaults.LogLevel, logger.LogLevel.Value);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_With_Null_Name_Sets_Null_As_Logger_Name()
        {
            // Act
            Logger logger = new Logger(null as string, new Mock<ILogIdGenerator>().Object, new LoggerSettings());

            // Assert
            Assert.AreEqual(null, logger.Name);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_With_Null_LogIdGenerator_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
             {
                 // Act
                 Logger logger = new Logger("logger name", null as ILogIdGenerator, new LoggerSettings());
             });
            Assert.AreEqual("logIdGenerator", ex.ParamName);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_LoggerSettings_When_Called_With_Null_LoggerSettings_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object, null as LoggerSettings);
            });
            Assert.AreEqual("loggerSettings", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_When_Called_Sets_Provided_Data()
        {
            // Arrange
            string loggerName = "logger name";
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;

            // Act
            Logger logger = new Logger(loggerName, logIdGenerator);

            // Assert
            Assert.AreEqual(loggerName, logger.Name);
            Assert.AreEqual(logIdGenerator, logger.LogIdGenerator);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_When_Called_Sets_Default_Logger_Settings()
        {
            // Act
            Logger logger = new Logger("logger name", new Mock<ILogIdGenerator>().Object);

            // Assert 
            Assert.AreEqual(Defaults.LoggerSettings, logger.LoggerSettings);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_When_Called_With_Null_Name_Sets_Null_As_Logger_Name()
        {
            // Act
            Logger logger = new Logger(null as string, new Mock<ILogIdGenerator>().Object);

            // Assert
            Assert.AreEqual(null, logger.Name);
        }
        [Test]
        public void Ctor_Taking_Name_LogIdGenerator_When_Called_With_Null_LogIdGenerator_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                Logger logger = new Logger("logger name", null as ILogIdGenerator);
            });
            Assert.AreEqual("logIdGenerator", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_Name_LoggerSettings_When_Called_Sets_Provided_Data()
        {
            // Arrange
            string loggerName = "logger name";
            var loggerSettings = new LoggerSettings();

            // Act
            Logger logger = new Logger(loggerName, loggerSettings);

            // Assert
            Assert.AreEqual(loggerName, logger.Name);
            Assert.AreEqual(loggerSettings, logger.LoggerSettings);
        }
        [Test]
        public void Ctor_Taking_Name_LoggerSettings_When_Called_Sets_Default_LogIdGenerator()
        {
            // Act
            Logger logger = new Logger("logger name", new LoggerSettings());

            // Assert
            Assert.IsInstanceOf<GuidLogIdGenerator>(logger.LogIdGenerator);
        }
        [Test]
        public void Ctor_Taking_Name_LoggerSettings_When_Called_With_Null_Name_Sets_Logger_Name_As_Null()
        {
            // Act
            Logger logger = new Logger(null as string, new LoggerSettings());

            // Assert
            Assert.AreEqual(null, logger.Name);
        }
        [Test]
        public void Ctor_Taking_Name_LoggerSettings_When_Called_With_Null_LoggerSettings_Throws_ArgumentNullException()
        {
            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                Logger logger = new Logger("logger name", null as LoggerSettings);
            });
            Assert.AreEqual("loggerSettings", ex.ParamName);
        }

        [Test]
        public void Ctor_Taking_Name_When_Called_Sets_Provided_Data()
        {
            // Arrange
            string loggerName = "logger name";

            // Act
            Logger logger = new Logger(loggerName);

            // Assert
            Assert.AreEqual(loggerName, logger.Name);
        }
        [Test]
        public void Ctor_Taking_Name_When_Called_Sets_Default_LogIdGenerator()
        {
            // Act
            Logger logger = new Logger("logger name");

            // Assert
            Assert.IsInstanceOf<GuidLogIdGenerator>(logger.LogIdGenerator);
        }
        [Test]
        public void Ctor_Taking_Name_When_Called_Sets_Default_LoggerSettings()
        {
            // Act
            Logger logger = new Logger("logger name");

            // Assert
            Assert.AreEqual(Defaults.LoggerSettings, logger.LoggerSettings);
        }
        [Test]
        public void Ctor_Taking_Name_When_Called_With_Null_Name_Sets_Logger_Name_As_Null()
        {
            // Act
            Logger logger = new Logger(null as string, new LoggerSettings());

            // Assert
            Assert.AreEqual(null, logger.Name);
        }

        [Test]
        public void Ctor_Default_When_Called_Sets_Default_Logger_Name()
        {
            // Act
            Logger logger = new Logger();

            // Assert
            Assert.IsNotNull(logger.Name);
        }
        [Test]
        public void Ctor_Default_When_Called_Sets_Default_LogIdGenerator()
        {
            // Act
            Logger logger = new Logger();

            // Assert
            Assert.IsInstanceOf<GuidLogIdGenerator>(logger.LogIdGenerator);
        }
        [Test]
        public void Ctor_Default_When_Called_Sets_Default_LoggerSettings()
        {
            // Act
            Logger logger = new Logger();

            // Assert
            Assert.AreEqual(Defaults.LoggerSettings, logger.LoggerSettings);
        }

        #endregion

        #region Tests for Log method

        [Test]
        public void Log_Taking_Log_When_Called_Uses_Log_Creation_Strategy_To_Create_Log_Data()
        {
            // Arrange  
            Logger logger = new Logger("logger1");
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(new LogModel() { LogId = "logId" });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            Log log = new Log();

            // Act
            var logId = logger.Log(log);

            // Assert
            logCreationStrategyMock.Verify(x => x.CreateLogModel(log, logger.Name), Times.Once);
            Assert.AreEqual("logId", logId);
        }
        [Test]
        public void Log_Taking_Log_When_Called_Given_That_Log_Type_Is_More_Than_Or_Equal_To_Logger_Log_Level_Serves_The_Log()
        {
            // Arrange
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Logger logger = new Logger("", new LoggerSettings() { LogLevel = LogLevel.Error });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            var errorLog = Log.Error("");
            var criticalLog = Log.Critical("");

            // Act
            logger.Log(errorLog);
            logger.Log(criticalLog);

            // Assert
            logCreationStrategyMock.Verify(x => x.CreateLogModel(errorLog, It.IsAny<string>()), Times.Once);
            logCreationStrategyMock.Verify(x => x.CreateLogModel(criticalLog, It.IsAny<string>()), Times.Once);
        }
        [Test]
        public void Log_Taking_Log_When_Called_Given_That_Log_Type_Is_Less_Than_Logger_Log_Level_Does_Not_Serve_The_Log()
        {
            // Arrange
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Logger logger = new Logger("", new LoggerSettings() { LogLevel = LogLevel.Error });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            var logid = logger.Log(Log.Info(""));

            // Assert
            logCreationStrategyMock.Verify(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>()), Times.Never);
            Assert.IsNull(logid);
        }
        [Test]
        public void Log_Taking_Log_When_Called_Serves_LogsDepending_On_The_Changing_Log_Level()
        {
            // Arrange
            var logLevelValueProvider = new MutableSetting<LogLevel>(LogLevel.All);
            Logger logger = new Logger("", new LoggerSettings() { LogLevel = logLevelValueProvider });
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            // #1
            logLevelValueProvider.SetValue(LogLevel.All);
            logger.Log(Log.Info(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>()), Times.Once);
            logCreationStrategyMock.Reset();
            // #2
            logLevelValueProvider.SetValue(LogLevel.Error);
            logger.Log(Log.Info(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>()), Times.Never);
            logCreationStrategyMock.Reset();
            // #3
            logLevelValueProvider.SetValue(LogLevel.None);
            logger.Log(Log.Critical(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>()), Times.Never);
            logCreationStrategyMock.Reset();
            // #3
            logLevelValueProvider.SetValue(LogLevel.Error);
            logger.Log(Log.Critical(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>()), Times.Once);
            logCreationStrategyMock.Reset();
        }
        [Test]
        public void Log_Taking_Log_When_Executed_Successfully_Returns_Log_Id_Generated_By_Log_Id_Generator()
        {
            // Arrange
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.Generate(It.IsAny<LogModel>())).Returns("logId");
            Logger logger = new Logger(null, logIdGeneratorMock.Object);
            Log log = new Log();

            // Act
            var logId = logger.Log(log);

            // Assert
            Assert.AreEqual("logId", logId);
        }
        [Test]
        public void Log_Taking_Log_When_Called_Given_That_Logger_Has_No_Log_Groups_Method_Executes_Successfully()
        {
            // Arrange 
            Logger logger = new Logger();

            // Act
            var logId = logger.Log(Log.Critical(""));

            // Assert
            Assert.AreEqual(0, logger.LogGroups.Count());
            Assert.NotNull(logId);
        }
        [Test]
        public void Log_Taking_Log_When_Executed_Successfully_Sends_Log_Data_To_All_Active_And_Eligible_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log groups
            var logGroup1 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            var logGroup2 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logger.AddLogGroup(logGroup1.Object);
            logger.AddLogGroup(logGroup2.Object);
            // Log creation strategy
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Log log = new Log();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(log, logger.Name)).Returns(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            logger.Log(log);

            // Assert
            logGroup1.Verify(x => x.Send(logModel), Times.Once);
            logGroup2.Verify(x => x.Send(logModel), Times.Once);
        }
        [Test]
        public void Log_Taking_Log_When_Executed_Successfully_Does_Not_Sends_Log_Data_To_Inactive_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log groups
            var inactiveLogGroup = Fakes.LogGroups.Standard(LogGroupStatus.Inactive, isEligible: true);
            var activeLogGroup = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logger.AddLogGroup(inactiveLogGroup.Object);
            logger.AddLogGroup(activeLogGroup.Object);
            // Log creation strategy
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Log log = new Log();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(log, logger.Name)).Returns(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            logger.Log(log);

            // Assert
            inactiveLogGroup.Verify(x => x.Send(logModel), Times.Never);
            activeLogGroup.Verify(x => x.Send(logModel), Times.Once);
        }
        [Test]
        public void Log_Taking_Log_When_Executed_Successfully_Does_Not_Sends_Log_Data_To_Not_Eligible_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log groups
            var logGroup1 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: false);
            var logGroup2 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logger.AddLogGroup(logGroup1.Object);
            logger.AddLogGroup(logGroup2.Object);
            // Log creation strategy
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Log log = new Log();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(log, logger.Name)).Returns(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            logger.Log(log);

            // Assert
            logGroup1.Verify(x => x.Send(logModel), Times.Never);
            logGroup2.Verify(x => x.Send(logModel), Times.Once);
        }
        [Test]
        public void Log_Taking_Log_When_Called_Sends_LogModel_To_Destinations_Of_Only_Eligible_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            var log = Log.Info("");
            var logModel = new LogModel() { LogType = log.Type };
            // Log creation strategy.
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(log, logger.Name)).Returns(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            // Destinations. 
            var infoDestinationMock = new Mock<ILogDestination>();
            var successDestinationMock = new Mock<ILogDestination>();
            var eventDestinationMock = new Mock<ILogDestination>();
            var warningDestinationMock = new Mock<ILogDestination>();
            var failureDestinationMock = new Mock<ILogDestination>();
            var errorDestinationMock = new Mock<ILogDestination>();
            var criticalDestinationMock = new Mock<ILogDestination>();
            // Log Groups each containing one of the above destinations. 
            logger.NewLogGroup(x => x.LogType == LogType.Info).AddDestinations(infoDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Success).AddDestinations(successDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Event).AddDestinations(eventDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Warning).AddDestinations(warningDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Failure).AddDestinations(errorDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Error).AddDestinations(errorDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Critical).AddDestinations(criticalDestinationMock.Object);

            // Act
            logger.Log(log);

            // Assert
            infoDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(d => d.Length == 1 && d[0] == logModel)), Times.Once);
            successDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            eventDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            warningDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            failureDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            errorDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            criticalDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }
        [Test]
        public void Log_Taking_Log_When_Called_None_Of_Dependants_Async_Methods_Are_Called()
        {
            // Arrange
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            var logDestinationMock = new Mock<ILogDestination>();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            Logger logger = new Logger(null, logIdGeneratorMock.Object);
            logger.NewLogGroup(x => true).AddDestinations(logDestinationMock.Object);
            logger.AddLogGroup(logGroupMock.Object);

            // Act
            var logId = logger.Log(Log.Critical(""));

            // Assert
            logIdGeneratorMock.Verify(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>()), Times.Never);
            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
            logGroupMock.Verify(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public void Log_Taking_Log_When_Exception_Is_Thrown_In_Log_Method_Exception_Is_Handled_Using_Exception_Handling_Strategy()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log group that throws
            var exception = new Exception();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logGroupMock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws(exception);
            logger.AddLogGroup(logGroupMock.Object);
            // Log exception handling strategy
            var logExceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            logExceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            logger.ResetExceptionHandlingStrategy(logExceptionHandlingStrategyMock.Object);
            Log log = new Log();

            // Act
            logger.Log(log);

            // Assert 
            logExceptionHandlingStrategyMock.Verify(x => x.HandleException(It.Is<LoggerException>(e => e.InnerException is AggregateException &&
                                                                                           (e.InnerException as AggregateException).InnerExceptions.Count == 1 &&
                                                                                           (e.InnerException as AggregateException).InnerExceptions[0] == exception), "Log method"), Times.Once);
        }
        [Test]
        public void Log_Taking_Log_When_Exception_Is_Thrown_In_Log_Method_Exception_Is_Handled_Using_Exception_Handling_Strategy2()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log group that throws
            var exception = new Exception();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logGroupMock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws(exception);
            logger.AddLogGroup(logGroupMock.Object);
            // Log exception handling strategy
            var logExceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            logExceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(true);
            logger.ResetExceptionHandlingStrategy(logExceptionHandlingStrategyMock.Object);
            Log log = new Log();

            // Assert
            Assert.Catch<Exception>(() =>
            {
                // Act
                logger.Log(log);
            });
            logExceptionHandlingStrategyMock.Verify(x => x.HandleException(It.Is<LoggerException>(e => e.InnerException is AggregateException &&
                                                                                                       (e.InnerException as AggregateException).InnerExceptions.Count == 1 &&
                                                                                                       (e.InnerException as AggregateException).InnerExceptions[0] == exception), "Log method"), Times.Once);
        }
        [Test]
        public void Log_Taking_Log_When_Exception_Is_Thrown_In_Exception_Handler_Strategy_Exception_Is_Swallowed()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log group that throws
            var exception = new Exception();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logGroupMock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws(exception);
            logger.AddLogGroup(logGroupMock.Object);
            // Log exception handling strategy that itself throws an exception in HandleException method.
            var logExceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            logExceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            logExceptionHandlingStrategyMock.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>())).Throws<Exception>();
            logger.ResetExceptionHandlingStrategy(logExceptionHandlingStrategyMock.Object);
            Log log = new Log();

            // Act
            logger.Log(log);

            //Assert
            logExceptionHandlingStrategyMock.Verify(x => x.HandleException(It.Is<LoggerException>(e => e.InnerException is AggregateException &&
                                                                                                       (e.InnerException as AggregateException).InnerExceptions.Count == 1 &&
                                                                                                       (e.InnerException as AggregateException).InnerExceptions[0] == exception), "Log method"), Times.Once);
        }
        [Test]
        public void Log_When_Called_Sends_Logs_Depending_On_Log_Group_Status()
        {
            // Arrange  
            Logger logger = new Logger("logger1");
            // Log creation strategy that returns stub log model.
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(new LogModel() { LogId = "logId" });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            // Status value provider.
            var statusValueProvider = new MutableSetting<LogGroupStatus>(LogGroupStatus.Inactive);
            // Log group with one mocked log destination.
            LogGroup logGroup = new LogGroup(new LogGroupSettings() { Status = statusValueProvider });
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            logger.AddLogGroup(logGroup);

            // Act
            // #1
            statusValueProvider.SetValue(LogGroupStatus.Inactive);
            logger.Log(new Log());
            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestinationMock.Reset();
            // #2
            statusValueProvider.SetValue(LogGroupStatus.Active);
            logger.Log(new Log());
            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Once);
            logDestinationMock.Reset();
            // #3
            statusValueProvider.SetValue(LogGroupStatus.Inactive);
            logger.Log(new Log());
            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestinationMock.Reset();
        }
        [Test]
        public void Log_Taking_Log_When_Called_Applies_All_Active_Log_Transformers()
        {
            // Arrange  
            Logger logger = new Logger();
            // Create and add log transformer mocks
            var logTransformer1Mock = new Mock<LogTransformerBase>("transformer1");
            logger.AddLogTransformer(logTransformer1Mock.Object);
            //
            var logTransformer2Mock = new Mock<LogTransformerBase>("transformer2");
            logger.AddLogTransformer(logTransformer2Mock.Object);
            //
            var logTransformer3Mock = new Mock<LogTransformerBase>("transformer3");
            logger.AddLogTransformer(logTransformer3Mock.Object);

            LogModel logModelFromStrategy = new LogModel();

            // Configure log creation strategy to return a predefined log model. 
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModelFromStrategy);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            var logId = logger.Log(new Log());

            // Assert
            logTransformer1Mock.Verify(x => x.Transform(logModelFromStrategy), Times.Once);
            logTransformer2Mock.Verify(x => x.Transform(logModelFromStrategy), Times.Once);
            logTransformer3Mock.Verify(x => x.Transform(logModelFromStrategy), Times.Once);
        }
        [Test]
        public void Log_Taking_Log_When_Called_Does_Not_Apply_Inactive_Log_Transformers()
        {
            // Arrange   
            Logger logger = new Logger();
            // Create and add log transformer mocks
            var logTransformer1Mock = new Mock<LogTransformerBase>("transformer1");
            logTransformer1Mock.Object.SetActive(false);
            logger.AddLogTransformer(logTransformer1Mock.Object);
            //
            var logTransformer2Mock = new Mock<LogTransformerBase>("transformer2");
            logger.AddLogTransformer(logTransformer2Mock.Object);
            //
            var logTransformer3Mock = new Mock<LogTransformerBase>("transformer3");
            logTransformer3Mock.Object.SetActive(false);
            logger.AddLogTransformer(logTransformer3Mock.Object);
            //
            var logTransformer4Mock = new Mock<LogTransformerBase>("transformer4");
            logger.AddLogTransformer(logTransformer4Mock.Object);

            LogModel logModelFromStrategy = new LogModel();

            // Configure log creation strategy to return a predefined log model. 
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModelFromStrategy);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            var logId = logger.Log(new Log());

            // Assert
            logTransformer1Mock.Verify(x => x.Transform(logModelFromStrategy), Times.Never);
            logTransformer2Mock.Verify(x => x.Transform(logModelFromStrategy), Times.Once);
            logTransformer3Mock.Verify(x => x.Transform(logModelFromStrategy), Times.Never);
            logTransformer4Mock.Verify(x => x.Transform(logModelFromStrategy), Times.Once);
        }

        #endregion

        #region Tests for LogAsync method



        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Uses_Log_Creation_Strategy_To_Create_Log_Data()
        {
            // Arrange  
            Logger logger = new Logger("logger1");
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new LogModel() { LogId = "logId" });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            Log log = new Log();

            // Act
            var logId = await logger.LogAsync(log);

            // Assert
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(log, logger.Name, It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual("logId", logId);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Given_That_Log_Type_Is_More_Than_Or_Equal_To_Logger_Log_Level_Serves_The_Log()
        {
            // Arrange
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Logger logger = new Logger("", new LoggerSettings() { LogLevel = LogLevel.Error });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            var errorLog = Log.Error("");
            var criticalLog = Log.Critical("");

            // Act
            await logger.LogAsync(errorLog);
            await logger.LogAsync(criticalLog);

            // Assert
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(errorLog, It.IsAny<string>(), default(CancellationToken)), Times.Once);
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(criticalLog, It.IsAny<string>(), default(CancellationToken)), Times.Once);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Given_That_Log_Type_Is_Less_Than_Logger_Log_Level_Does_Not_Serve_The_Log()
        {
            // Arrange
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Logger logger = new Logger("", new LoggerSettings() { LogLevel = LogLevel.Error });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            var logid = await logger.LogAsync(Log.Info(""));

            // Assert
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsNull(logid);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Serves_Logs_Depending_On_The_Changing_Log_Level()
        {
            // Arrange
            var logLevelValueProvider = new MutableSetting<LogLevel>(LogLevel.All);
            Logger logger = new Logger("", new LoggerSettings() { LogLevel = logLevelValueProvider });
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            // #1
            logLevelValueProvider.SetValue(LogLevel.All);
            await logger.LogAsync(Log.Info(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            logCreationStrategyMock.Reset();
            // #2
            logLevelValueProvider.SetValue(LogLevel.Error);
            await logger.LogAsync(Log.Info(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            logCreationStrategyMock.Reset();
            // #3
            logLevelValueProvider.SetValue(LogLevel.None);
            await logger.LogAsync(Log.Critical(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            logCreationStrategyMock.Reset();
            // #3
            logLevelValueProvider.SetValue(LogLevel.Error);
            await logger.LogAsync(Log.Critical(""));
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            logCreationStrategyMock.Reset();
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Executed_Successfully_Returns_Log_Id_Generated_By_Log_Id_Generator()
        {
            // Arrange
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ReturnsAsync("logId");
            Logger logger = new Logger(null, logIdGeneratorMock.Object);
            Log log = new Log();

            // Act
            var logId = await logger.LogAsync(log);

            // Assert
            Assert.AreEqual("logId", logId);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Given_That_Logger_Has_No_Log_Groups_Method_Executes_Successfully()
        {
            // Arrange 
            Logger logger = new Logger();

            // Act
            var logId = await logger.LogAsync(Log.Critical(""));

            // Assert
            Assert.AreEqual(0, logger.LogGroups.Count());
            Assert.NotNull(logId);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Executed_Successfully_Sends_Log_Data_To_All_Active_And_Eligible_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log groups
            var logGroup1 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            var logGroup2 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logger.AddLogGroup(logGroup1.Object);
            logger.AddLogGroup(logGroup2.Object);
            // Log creation strategy
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Log log = new Log();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(log, logger.Name, It.IsAny<CancellationToken>())).ReturnsAsync(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            await logger.LogAsync(log);

            // Assert
            logGroup1.Verify(x => x.SendAsync(logModel, default(CancellationToken)), Times.Once);
            logGroup2.Verify(x => x.SendAsync(logModel, default(CancellationToken)), Times.Once);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Executed_Successfully_Does_Not_Sends_Log_Data_To_Inactive_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log groups
            var inactiveLogGroup = Fakes.LogGroups.Standard(LogGroupStatus.Inactive, isEligible: true);
            var activeLogGroup = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logger.AddLogGroup(inactiveLogGroup.Object);
            logger.AddLogGroup(activeLogGroup.Object);
            // Log creation strategy
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Log log = new Log();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(log, logger.Name, It.IsAny<CancellationToken>())).ReturnsAsync(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            await logger.LogAsync(log);

            // Assert
            inactiveLogGroup.Verify(x => x.SendAsync(logModel, default(CancellationToken)), Times.Never);
            activeLogGroup.Verify(x => x.SendAsync(logModel, default(CancellationToken)), Times.Once);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Executed_Successfully_Does_Not_Sends_Log_Data_To_Not_Eligible_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log groups
            var logGroup1 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: false);
            var logGroup2 = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logger.AddLogGroup(logGroup1.Object);
            logger.AddLogGroup(logGroup2.Object);
            // Log creation strategy
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Log log = new Log();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(log, logger.Name, It.IsAny<CancellationToken>())).ReturnsAsync(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            await logger.LogAsync(log);

            // Assert
            logGroup1.Verify(x => x.SendAsync(logModel, It.IsAny<CancellationToken>()), Times.Never);
            logGroup2.Verify(x => x.SendAsync(logModel, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Sends_LogModel_To_Destinations_Of_Only_Eligible_Groups()
        {
            // Arrange 
            Logger logger = new Logger();
            var log = Log.Info("");
            var logModel = new LogModel() { LogType = log.Type };
            // Log creation strategy.
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(log, logger.Name, It.IsAny<CancellationToken>())).ReturnsAsync(logModel);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            // Destinations. 
            var infoDestinationMock = new Mock<ILogDestination>();
            var successDestinationMock = new Mock<ILogDestination>();
            var eventDestinationMock = new Mock<ILogDestination>();
            var warningDestinationMock = new Mock<ILogDestination>();
            var failureDestinationMock = new Mock<ILogDestination>();
            var errorDestinationMock = new Mock<ILogDestination>();
            var criticalDestinationMock = new Mock<ILogDestination>();
            // Log Groups each containing one of the above destinations. 
            logger.NewLogGroup(x => x.LogType == LogType.Info).AddDestinations(infoDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Success).AddDestinations(successDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Event).AddDestinations(eventDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Warning).AddDestinations(warningDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Failure).AddDestinations(errorDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Error).AddDestinations(errorDestinationMock.Object);
            logger.NewLogGroup(x => x.LogType == LogType.Critical).AddDestinations(criticalDestinationMock.Object);

            // Act
            await logger.LogAsync(log);

            // Assert
            infoDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(d => d.Length == 1 && d[0] == logModel), It.IsAny<CancellationToken>()), Times.Once);
            successDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
            eventDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
            warningDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
            failureDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
            errorDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
            criticalDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_None_Of_Dependants_Sync_Methods_Are_Called()
        {
            // Arrange
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            var logDestinationMock = new Mock<ILogDestination>();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            Logger logger = new Logger(null, logIdGeneratorMock.Object);
            logger.NewLogGroup(x => true).AddDestinations(logDestinationMock.Object);
            logger.AddLogGroup(logGroupMock.Object);

            // Act
            var logId = await logger.LogAsync(Log.Critical(""));

            // Assert
            logIdGeneratorMock.Verify(x => x.Generate(It.IsAny<LogModel>()), Times.Never);
            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logGroupMock.Verify(x => x.Send(It.IsAny<LogModel>()), Times.Never);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Exception_Is_Thrown_In_Log_Method_Exception_Is_Handled_Using_Exception_Handling_Strategy()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log group that throws
            var exception = new Exception();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logGroupMock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), default(CancellationToken))).ThrowsAsync(exception);
            logger.AddLogGroup(logGroupMock.Object);
            // Log exception handling strategy
            var logExceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            logExceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            logger.ResetExceptionHandlingStrategy(logExceptionHandlingStrategyMock.Object);
            Log log = new Log();
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await logger.LogAsync(log, cancellationToken);

            // Assert
            logExceptionHandlingStrategyMock.Verify(x => x.HandleExceptionAsync(It.Is<LoggerException>(e => e.InnerException is AggregateException &&
                                                                                           (e.InnerException as AggregateException).InnerExceptions.Count == 1 &&
                                                                                           (e.InnerException as AggregateException).InnerExceptions[0] == exception), "LogAsync method", It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void LogAsync_Taking_Log_When_Exception_Is_Thrown_In_Log_Method_Exception_Is_Handled_Using_Exception_Handling_Strategy2()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log group that throws
            var exception = new Exception();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logGroupMock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            logger.AddLogGroup(logGroupMock.Object);
            // Log exception handling strategy
            var logExceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            logExceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(true);
            logger.ResetExceptionHandlingStrategy(logExceptionHandlingStrategyMock.Object);
            Log log = new Log();

            // Assert
            Assert.CatchAsync<Exception>(async () =>
            {
                // Act
                await logger.LogAsync(log);
            });
            logExceptionHandlingStrategyMock.Verify(x => x.HandleExceptionAsync(It.Is<LoggerException>(e => e.InnerException is AggregateException &&
                                                                                           (e.InnerException as AggregateException).InnerExceptions.Count == 1 &&
                                                                                           (e.InnerException as AggregateException).InnerExceptions[0] == exception), "LogAsync method", It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Exception_Is_Thrown_In_Exception_Handler_Strategy_Exception_Is_Swallowed()
        {
            // Arrange 
            Logger logger = new Logger();
            // Log group that throws
            var exception = new Exception();
            var logGroupMock = Fakes.LogGroups.Standard(LogGroupStatus.Active, isEligible: true);
            logGroupMock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            logger.AddLogGroup(logGroupMock.Object);
            // Log exception handling strategy that itself throws an exception in HandleException method.
            var logExceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            logExceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);
            logExceptionHandlingStrategyMock.Setup(x => x.HandleExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            logger.ResetExceptionHandlingStrategy(logExceptionHandlingStrategyMock.Object);
            Log log = new Log();

            // Act
            await logger.LogAsync(log);

            //Assert 
            logExceptionHandlingStrategyMock.Verify(x => x.HandleExceptionAsync(It.Is<LoggerException>(e => e.InnerException is AggregateException &&
                                                                                                      (e.InnerException as AggregateException).InnerExceptions.Count == 1 &&
                                                                                                      (e.InnerException as AggregateException).InnerExceptions[0] == exception), "LogAsync method", It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task LogAsync_When_Called_Sends_Logs_Depending_On_Log_Group_Status()
        {
            // Arrange  
            Logger logger = new Logger("logger1");
            // Log creation strategy that returns stub log model.
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new LogModel() { LogId = "logId" });
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            // Status value provider.
            var statusValueProvider = new MutableSetting<LogGroupStatus>(LogGroupStatus.Inactive);
            // Log group with one mocked log destination.
            LogGroup logGroup = new LogGroup(new LogGroupSettings() { Status = statusValueProvider });
            var logDestinationMock = new Mock<ILogDestination>();
            logGroup.AddDestinations(logDestinationMock.Object);
            logger.AddLogGroup(logGroup);

            // Act
            // #1
            statusValueProvider.SetValue(LogGroupStatus.Inactive);
            await logger.LogAsync(new Log());
            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), default(CancellationToken)), Times.Never);
            logDestinationMock.Reset();
            // #2
            statusValueProvider.SetValue(LogGroupStatus.Active);
            await logger.LogAsync(new Log());
            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), default(CancellationToken)), Times.Once);
            logDestinationMock.Reset();
            // #3
            statusValueProvider.SetValue(LogGroupStatus.Inactive);
            await logger.LogAsync(new Log());
            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), default(CancellationToken)), Times.Never);
            logDestinationMock.Reset();
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Applies_All_Active_Log_Transformers()
        {
            // Arrange  
            Logger logger = new Logger();
            // Create and add log transformer mocks
            var logTransformer1Mock = new Mock<LogTransformerBase>("transformer1");
            logger.AddLogTransformer(logTransformer1Mock.Object);
            //
            var logTransformer2Mock = new Mock<LogTransformerBase>("transformer2");
            logger.AddLogTransformer(logTransformer2Mock.Object);
            //
            var logTransformer3Mock = new Mock<LogTransformerBase>("transformer3");
            logger.AddLogTransformer(logTransformer3Mock.Object);

            LogModel logModelFromStrategy = new LogModel();

            // Configure log creation strategy to return a predefined log model. 
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModelFromStrategy);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            var logId = await logger.LogAsync(new Log());

            // Assert
            logTransformer1Mock.Verify(x => x.TransformAsync(logModelFromStrategy, default(CancellationToken)), Times.Once);
            logTransformer2Mock.Verify(x => x.TransformAsync(logModelFromStrategy, default(CancellationToken)), Times.Once);
            logTransformer3Mock.Verify(x => x.TransformAsync(logModelFromStrategy, default(CancellationToken)), Times.Once);
        }
        [Test]
        public async Task LogAsync_Taking_Log_When_Called_Does_Not_Apply_Inactive_Log_Transformers()
        {
            // Arrange   
            Logger logger = new Logger();
            // Create and add log transformer mocks
            var logTransformer1Mock = new Mock<LogTransformerBase>("transformer1");
            logTransformer1Mock.Object.SetActive(false);
            logger.AddLogTransformer(logTransformer1Mock.Object);
            //
            var logTransformer2Mock = new Mock<LogTransformerBase>("transformer2");
            logger.AddLogTransformer(logTransformer2Mock.Object);
            //
            var logTransformer3Mock = new Mock<LogTransformerBase>("transformer3");
            logTransformer3Mock.Object.SetActive(false);
            logger.AddLogTransformer(logTransformer3Mock.Object);
            //
            var logTransformer4Mock = new Mock<LogTransformerBase>("transformer4");
            logger.AddLogTransformer(logTransformer4Mock.Object);

            LogModel logModelFromStrategy = new LogModel();

            // Configure log creation strategy to return a predefined log model. 
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModelFromStrategy);
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            var logId = await logger.LogAsync(new Log());

            // Assert
            logTransformer1Mock.Verify(x => x.TransformAsync(logModelFromStrategy, It.IsAny<CancellationToken>()), Times.Never);
            logTransformer2Mock.Verify(x => x.TransformAsync(logModelFromStrategy, It.IsAny<CancellationToken>()), Times.Once);
            logTransformer3Mock.Verify(x => x.TransformAsync(logModelFromStrategy, It.IsAny<CancellationToken>()), Times.Never);
            logTransformer4Mock.Verify(x => x.TransformAsync(logModelFromStrategy, It.IsAny<CancellationToken>()), Times.Once);
        }




        [Test]
        public void LogAsync_When_Called_With_Cancelled_Cancellation_Token_Immediately_Returns_Cancelled_Task()
        {
            // Arrange  
            Logger logger = new Logger("loggerName");
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);
            Log log = new Log();
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await logger.LogAsync(log, cancellationToken);
            });
            logCreationStrategyMock.Verify(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region NewLogGroup(LogGroup logGroup)

        [Test]
        public void NewLogGroup_Taking_Log_Group_When_Called_Adds_Log_Group_To_Logger()
        {
            // Arrange
            Logger logger = new Logger();
            LogModelFunc<bool> predicate = (l) => false;
            LogGroup logGroup = new LogGroup("logGroup", predicate, new List<ILogDestination>());

            // Act
            logger.AddLogGroup(logGroup);

            // Assert    
            Assert.AreEqual(1, logger.LogGroups.Count());
            Assert.AreEqual("logGroup", logger.LogGroups.First().Name);
        }

        [Test]
        public void NewLogGroup_Taking_Log_Group_When_Called_With_Null_Log_Group_Throws_ArgumentNullException()
        {
            // Arrange
            Logger logger = new Logger();

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.AddLogGroup(null as LogGroup);
            });
        }

        [Test]
        public void NewLogGroup_Taking_Log_Group_When_Log_Group_With_This_Name_Already_Added_To_Logger_Throws_InvalidOperationException()
        {
            // Arrange
            Logger logger = new Logger();
            LogGroup logGroup1 = new LogGroup("logGroup", (l) => false, new List<ILogDestination>());
            LogGroup logGroup2 = new LogGroup("logGroup", (l) => false, new List<ILogDestination>());

            logger.AddLogGroup(logGroup1);

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                logger.AddLogGroup(logGroup2);
            });
        }

        #endregion

        #region Tests for NewLogGroup method

        [Test]
        public void NewLogGroup_WithLogModelPredicate_WhenCalled_ReturnsNewLogGroupWithPredicateAndEmptyDestinationsList()
        {
            // Arrange
            Logger logger = new Logger();
            LogModelFunc<bool> predicate = (l) => false;

            // Act
            var logGroup = logger.NewLogGroup(predicate);

            // Assert    
            Assert.AreEqual(predicate, logGroup.Rule);
            Assert.NotNull(logGroup.LogDestinations);
            Assert.AreEqual(0, logGroup.LogDestinations.Count);
        }
        [Test]
        public void NewLogGroup_WithLogModelPredicate_WhenCalledWithNullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            Logger logger = new Logger();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                var logGroup = logger.NewLogGroup(null as LogModelFunc<bool>);
            });
        }
        [Test]
        public void NewLogGroup_WithLogModelPredicate_WhenCalled_AddsNewCreatedLogGroupToLoggersCollection()
        {
            // Arrange
            Logger logger = new Logger();

            // Act
            var logGroup = logger.NewLogGroup((l) => false);

            // Assert
            Assert.AreEqual(1, logger.LogGroups.Count());
            Assert.AreEqual(logGroup, logger.LogGroups.First());
        }

        [Test]
        public void GetLogGroup_WithLogGroupName_WhenCalledProvidingNameOfExistingLogGroup_ReturnsThatLogGroup()
        {
            // Arrange
            Logger logger = new Logger();
            string groupName = "group name";
            LogGroup logGroup = new LogGroup(groupName, (l) => false, new List<ILogDestination>());
            logger.AddLogGroup(logGroup);

            // Act
            var logGroupFound = logger.GetLogGroup(groupName);

            // Assert     
            Assert.AreEqual(logGroup, logGroupFound);
            Assert.AreEqual(groupName, logGroupFound.Name);
        }
        [Test]
        public void GetLogGroup_WithLogGroupName_WhenCalledProvidingNameOfNotExistingLogGroup_ReturnsNull()
        {
            // Arrange
            Logger logger = new Logger();
            LogGroup logGroup = new LogGroup("group name1", (l) => false, new List<ILogDestination>());
            logger.AddLogGroup(logGroup);

            // Act
            var logGroupFound = logger.GetLogGroup("group name2");

            // Assert     
            Assert.IsNull(logGroupFound);
        }
        [Test]
        public void GetLogGroup_WithLogGroupName_WhenCalled_SearchIsCaseSensitive()
        {
            // Arrange
            Logger logger = new Logger();
            string groupName = "group name";
            LogGroup logGroup = new LogGroup(groupName, (l) => false, new List<ILogDestination>());
            logger.AddLogGroup(logGroup);

            // Act
            var logGroupFound = logger.GetLogGroup(groupName.ToUpper());

            // Assert     
            Assert.IsNull(logGroupFound);
        }

        #endregion

        #region Tests for ResetLogCreationStrategy method

        [Test]
        public void ResetLogCreationStrategy_When_Called_Calls_Provided_Strategies_InitStrategy_Method_And_Sets_It_As_New_LogCreationStrategy()
        {
            // Arrange
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            Logger logger = new Logger();

            // Act
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Assert
            ILogIdGenerator logIdGenerator = logger.LogIdGenerator;
            IEnumerable<LogContributorBase> logContributors = logger.LogContributors;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = logger.ExceptionHandlingStrategy;

            Assert.AreEqual(logCreationStrategyMock.Object, logger.LogCreationStrategy);
            Assert.AreEqual(logIdGenerator, logCreationStrategyMock.Object.LogIdGenerator);
            Assert.AreEqual(logContributors, logCreationStrategyMock.Object.LogContributors);
            Assert.AreEqual(exceptionHandlingStrategy, logCreationStrategyMock.Object.ExceptionHandlingStrategy);
        }
        [Test]
        public void ResetLogCreationStrategy_When_Called_With_Null_LogCreationStrategy_Throws_ArgumentNullException()
        {
            // Arrange
            Logger logger = new Logger();

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.ResetLogCreationStrategy(null as LogCreationStrategyBase);
            });
        }

        #endregion

        #region Tests for SetDiagnosticsDestination method

        [Test]
        public void SetDiagnosticsDestination_When_Called_Sets_DiagnosticsDestination()
        {
            // Arrange
            Logger logger = new Logger();
            var logdestination = new Mock<ILogDestination>().Object;

            // Act
            logger.SetDiagnosticsDestination(logdestination);

            // Assert
            Assert.AreEqual(logdestination, logger.DiagnosticsDestination);
        }
        [Test]
        public void SetDiagnosticsDestination_When_Called_WithNull_Hanlder_Throws_ArgumentNullException()
        {
            // Arrange
            Logger logger = new Logger();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.SetDiagnosticsDestination(null as ILogDestination);
            });
            Assert.AreEqual("diagnosticsDestination", ex.ParamName);
        }

        #endregion

        #region Tests for ResetDiagnosticsManager method

        [Test]
        public void ResetDiagnosticsManager_When_Called_Sets_Diagnostics_Manager()
        {
            // Arrange
            Logger logger = new Logger();
            var diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;

            // Act
            logger.ResetDiagnosticsManager(diagnosticsManager);

            // Assert
            Assert.AreEqual(diagnosticsManager, logger.DiagnosticsManager);
            Assert.AreEqual(diagnosticsManager, logger.ExceptionHandlingStrategy.DiagnosticsManager); 
        }
        [Test]
        public void ResetDiagnosticsManager_When_Called_Sets_Provides_Diagnostics_Manager_With_Diagnostics_Destination()
        {
            // Arrange
            Logger logger = new Logger();
            var diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;
            var diagnosticsDestination = new Mock<ILogDestination>().Object;
            logger.SetDiagnosticsDestination(diagnosticsDestination);

            // Act
            logger.ResetDiagnosticsManager(diagnosticsManager);

            // Assert
            Assert.AreEqual(diagnosticsDestination, logger.DiagnosticsManager.LogDestination);
        }
        [Test]
        public void ResetDiagnosticsManager_When_Called_Provides_Diagnostics_Manager_With_Logger_Name()
        {
            // Arrange
            Logger logger = new Logger();
            var diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object; 

            // Act
            logger.ResetDiagnosticsManager(diagnosticsManager);

            // Assert
            Assert.AreEqual(logger.Name, logger.DiagnosticsManager.LoggerName);
        }
        [Test]
        public void ResetDiagnosticsManager_When_Called_With_Null_Diagnostics_Manager_Throws_ArgumentNullException()
        {
            // Arrange
            Logger logger = new Logger();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.ResetDiagnosticsManager(null as DiagnosticsManagerBase);
            });
            Assert.AreEqual("diagnosticsManager", ex.ParamName);
        }

        #endregion

        #region Tests for ResetExceptionHandlingStrategy method

        [Test]
        public void ResetExceptionHandlingStrategy_When_Called_Sets_ExceptionHandlingStrategy()
        {
            // Arrange
            var exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            Logger logger = new Logger();

            // Act
            logger.ResetExceptionHandlingStrategy(exceptionHandlingStrategy);

            // Assert
            Assert.AreEqual(exceptionHandlingStrategy, logger.ExceptionHandlingStrategy);
        }
        [Test]
        public void ResetExceptionHandlingStrategy_When_Called_With_Null_ExceptionHandlingStrategy_Throws_ArgumentNullException()
        {
            // Arrange 
            Logger logger = new Logger();

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.ResetExceptionHandlingStrategy(null as ExceptionHandlingStrategyBase);
            });

            Assert.AreEqual("exceptionHandlingStrategy", ex.ParamName);
        }
        [Test]
        public void ResetExceptionHandlingStrategy_When_Called_Provider_Strategy_With_LogDestination()
        {
            // Arrange
            Logger logger = new Logger();
            var diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;
            logger.ResetDiagnosticsManager(diagnosticsManager);
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();

            // Act
            logger.ResetExceptionHandlingStrategy(exceptionHandlingStrategyMock.Object);

            // Assert
            Assert.AreEqual(diagnosticsManager, exceptionHandlingStrategyMock.Object.DiagnosticsManager);
        }

        #endregion

        #region Tests for AddLogContributor

        [Test]
        public void AddLogContributor_When_Called_Adds_Provided_LogContributor_To_Logger()
        {
            // Arrange        
            Logger logger = new Logger();
            var logContributor = new Mock<LogContributorBase>("").Object;

            // Act
            logger.AddLogContributor(logContributor);

            // Assert 
            Assert.AreEqual(1, logger.LogContributors.Count());
            Assert.AreEqual(logContributor, logger.LogContributors.First());
        }
        [Test]
        public void AddLogContributor_When_Called_With_Null_Log_Contributor_Throws_ArgumentNullException()
        {
            // Arrange        
            Logger logger = new Logger();

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.AddLogContributor(null as LogContributorBase);
            });
        }

        #endregion

        #region Tests for AddLogTransformer

        [Test]
        public void AddLogTransformer_When_Called_Adds_Provided_LogTransformer_To_Logger()
        {
            // Arrange        
            Logger logger = new Logger();
            var logTransformer = new Mock<LogTransformerBase>("").Object;

            // Act
            logger.AddLogTransformer(logTransformer);

            // Assert 
            Assert.AreEqual(1, logger.LogTransformers.Count());
            Assert.AreEqual(logTransformer, logger.LogTransformers.First());
        }
        [Test]
        public void AddLogTransformer_When_Called_With_Null_Log_Transformer_Throws_ArgumentNullException()
        {
            // Arrange        
            Logger logger = new Logger();

            //Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.AddLogTransformer(null as LogTransformerBase);
            });
        }

        #endregion

        #region Tests for Send method

        [Test]
        public void Send_When_Called_Sends_Logs_To_All_Active_And_Eligible_Log_Groups()
        {
            // Arrange
            Logger logger = new Logger();
            // First active log group.
            var activeLogGroupBase1Mock = new Mock<LogGroupBase>();
            activeLogGroupBase1Mock.SetupGet(x => x.Name).Returns("firstActive");
            activeLogGroupBase1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            activeLogGroupBase1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Second active log group.
            var activeLogGroupBase2Mock = new Mock<LogGroupBase>();
            activeLogGroupBase1Mock.SetupGet(x => x.Name).Returns("secondActive");
            activeLogGroupBase2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            activeLogGroupBase2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Add log groups to logger
            logger.AddLogGroup(activeLogGroupBase1Mock.Object);
            logger.AddLogGroup(activeLogGroupBase2Mock.Object);
            // Logs
            LogModel logModel1 = Samples.LogModels.Standard(LogType.Error);
            LogModel logModel2 = Samples.LogModels.Standard(LogType.Info);

            // Act
            logger.Send(new LogModel[] { logModel1, logModel2 });

            // Assert
            Assert.AreEqual(2, logger.LogGroups.Count());
            // that logs were written to first active log group
            activeLogGroupBase1Mock.Verify(x => x.Send(logModel1), Times.Once);
            activeLogGroupBase1Mock.Verify(x => x.Send(logModel2), Times.Once);
            // and to second active log group.
            activeLogGroupBase2Mock.Verify(x => x.Send(logModel1), Times.Once);
            activeLogGroupBase2Mock.Verify(x => x.Send(logModel2), Times.Once);
        }
        [Test]
        public void Send_When_Called_Does_Not_Sends_Logs_To_Inactive_Log_Groups()
        {
            // Arrange
            Logger logger = new Logger();
            // Inactive log group.
            var inactiveLogGroupBaseMock = new Mock<LogGroupBase>();
            inactiveLogGroupBaseMock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Inactive));
            inactiveLogGroupBaseMock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Add log group to logger
            logger.AddLogGroup(inactiveLogGroupBaseMock.Object);
            // Logs
            LogModel logModel1 = Samples.LogModels.Standard(LogType.Error);
            LogModel logModel2 = Samples.LogModels.Standard(LogType.Info);

            // Act
            logger.Send(new LogModel[] { logModel1, logModel2 });

            // Assert 
            Assert.AreEqual(1, logger.LogGroups.Count());
            inactiveLogGroupBaseMock.Verify(x => x.Send(logModel1), Times.Never);
            inactiveLogGroupBaseMock.Verify(x => x.Send(logModel2), Times.Never);
        }
        [Test]
        public void Send_When_Called_Does_Not_Sends_Logs_To_Not_Eligible_Log_Groups()
        {
            // Arrange
            Logger logger = new Logger();
            // Inactive log group.
            var notEligibleLogGroupBaseMock = new Mock<LogGroupBase>();
            notEligibleLogGroupBaseMock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            notEligibleLogGroupBaseMock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(false);
            // Add log group to logger
            logger.AddLogGroup(notEligibleLogGroupBaseMock.Object);
            // Logs
            LogModel logModel1 = Samples.LogModels.Standard(LogType.Error);
            LogModel logModel2 = Samples.LogModels.Standard(LogType.Info);

            // Act
            logger.Send(new LogModel[] { logModel1, logModel2 });

            // Assert 
            Assert.AreEqual(1, logger.LogGroups.Count());
            notEligibleLogGroupBaseMock.Verify(x => x.Send(logModel1), Times.Never);
            notEligibleLogGroupBaseMock.Verify(x => x.Send(logModel2), Times.Never);
        }

        #endregion

        #region Tests for SendAsync method

        [Test]
        public async Task SendAsync_When_Called_Sends_Logs_To_All_Active_And_Eligible_Log_Groups()
        {
            // Arrange
            Logger logger = new Logger();
            // First active log group.
            var activeLogGroupBase1Mock = new Mock<LogGroupBase>();
            activeLogGroupBase1Mock.SetupGet(x => x.Name).Returns("firstActive");
            activeLogGroupBase1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            activeLogGroupBase1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Second active log group.
            var activeLogGroupBase2Mock = new Mock<LogGroupBase>();
            activeLogGroupBase1Mock.SetupGet(x => x.Name).Returns("secondActive");
            activeLogGroupBase2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            activeLogGroupBase2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Add log groups to logger
            logger.AddLogGroup(activeLogGroupBase1Mock.Object);
            logger.AddLogGroup(activeLogGroupBase2Mock.Object);
            // Logs
            LogModel logModel1 = Samples.LogModels.Standard(LogType.Error);
            LogModel logModel2 = Samples.LogModels.Standard(LogType.Info);

            // Act
            await logger.SendAsync(new LogModel[] { logModel1, logModel2 });

            // Assert
            Assert.AreEqual(2, logger.LogGroups.Count());
            // that logs were written to first active log group
            activeLogGroupBase1Mock.Verify(x => x.SendAsync(logModel1, default(CancellationToken)), Times.Once);
            activeLogGroupBase1Mock.Verify(x => x.SendAsync(logModel2, default(CancellationToken)), Times.Once);
            // and to second active log group.
            activeLogGroupBase2Mock.Verify(x => x.SendAsync(logModel1, default(CancellationToken)), Times.Once);
            activeLogGroupBase2Mock.Verify(x => x.SendAsync(logModel2, default(CancellationToken)), Times.Once);
        }
        [Test]
        public async Task SendAsync_When_Called_Does_Not_Sends_Logs_To_Inactive_Log_Groups()
        {
            // Arrange
            Logger logger = new Logger();
            // Inactive log group.
            var inactiveLogGroupBaseMock = new Mock<LogGroupBase>();
            inactiveLogGroupBaseMock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Inactive));
            inactiveLogGroupBaseMock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Add log group to logger
            logger.AddLogGroup(inactiveLogGroupBaseMock.Object);
            // Logs
            LogModel logModel1 = Samples.LogModels.Standard(LogType.Error);
            LogModel logModel2 = Samples.LogModels.Standard(LogType.Info);

            // Act
            await logger.SendAsync(new LogModel[] { logModel1, logModel2 });

            // Assert 
            Assert.AreEqual(1, logger.LogGroups.Count());
            inactiveLogGroupBaseMock.Verify(x => x.SendAsync(logModel1, default(CancellationToken)), Times.Never);
            inactiveLogGroupBaseMock.Verify(x => x.SendAsync(logModel2, default(CancellationToken)), Times.Never);
        }
        [Test]
        public async Task SendAsync_When_Called_Does_Not_Sends_Logs_To_Not_Eligible_Log_Groups()
        {
            // Arrange
            Logger logger = new Logger();
            // Inactive log group.
            var notEligibleLogGroupBaseMock = new Mock<LogGroupBase>();
            notEligibleLogGroupBaseMock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            notEligibleLogGroupBaseMock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(false);
            // Add log group to logger
            logger.AddLogGroup(notEligibleLogGroupBaseMock.Object);
            // Logs
            LogModel logModel1 = Samples.LogModels.Standard(LogType.Error);
            LogModel logModel2 = Samples.LogModels.Standard(LogType.Info);

            // Act
            await logger.SendAsync(new LogModel[] { logModel1, logModel2 });

            // Assert 
            Assert.AreEqual(1, logger.LogGroups.Count());
            notEligibleLogGroupBaseMock.Verify(x => x.SendAsync(logModel1, default(CancellationToken)), Times.Never);
            notEligibleLogGroupBaseMock.Verify(x => x.SendAsync(logModel2, default(CancellationToken)), Times.Never);
        }
        [Test]
        public void SendAsync_When_Called_With_Cancelled_Cancellation_Token_Does_Not_Sends_Logs_To_Log_Groups_And_Returns_Cancelled_Task()
        {
            // Arrange
            Logger logger = new Logger();
            // First active log group.
            var activeLogGroupBase1Mock = new Mock<LogGroupBase>();
            activeLogGroupBase1Mock.SetupGet(x => x.Name).Returns("firstActive");
            activeLogGroupBase1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            activeLogGroupBase1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Second active log group.
            var activeLogGroupBase2Mock = new Mock<LogGroupBase>();
            activeLogGroupBase1Mock.SetupGet(x => x.Name).Returns("secondActive");
            activeLogGroupBase2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            activeLogGroupBase2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            // Add log groups to logger
            logger.AddLogGroup(activeLogGroupBase1Mock.Object);
            logger.AddLogGroup(activeLogGroupBase2Mock.Object);
            // Logs
            LogModel logModel1 = Samples.LogModels.Standard(LogType.Error);
            LogModel logModel2 = Samples.LogModels.Standard(LogType.Info);
            // Canceled cancellation token
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert
            Assert.CatchAsync<TaskCanceledException>(async () =>
            {
                // Act
                await logger.SendAsync(new LogModel[] { logModel1, logModel2 }, cancellationToken);
            });
            Assert.AreEqual(2, logger.LogGroups.Count());
            // that logs were written to first active log group
            activeLogGroupBase1Mock.Verify(x => x.SendAsync(logModel1, default(CancellationToken)), Times.Never);
            activeLogGroupBase1Mock.Verify(x => x.SendAsync(logModel2, default(CancellationToken)), Times.Never);
            // and to second active log group.
            activeLogGroupBase2Mock.Verify(x => x.SendAsync(logModel1, default(CancellationToken)), Times.Never);
            activeLogGroupBase2Mock.Verify(x => x.SendAsync(logModel2, default(CancellationToken)), Times.Never);
        }

        #endregion

        #region Tests for WriteLoggerConfigurationToDiagnostics method

        [Test]
        public void WriteLoggerConfigurationToDiagnostics_When_Called_Send_Log_Containing_Configuration_To_Diagnostics_Manager()
        {
            // Arrange
            var diagnosticsManagerMock = new Mock<DiagnosticsManagerBase>();
            Logger logger = new Logger();
            logger.ResetDiagnosticsManager(diagnosticsManagerMock.Object);

            // Act
            logger.WriteLoggerConfigurationToDiagnostics();

            // Assert
            diagnosticsManagerMock.Verify(x => x.WriteToDiagnostics(It.Is<Log>(l => l.Type == LogType.Info &&
                                                                                    l.Description == "Logger has been configured. See the log item." &&
                                                                                    l.Items.Count == 1 &&
                                                                                    l.Items[0].Name == "Configuration" &&
                                                                                    l.Items[0].Value != null)), Times.Once);
        }

        #endregion

        #region Tests for GenerateDefaultLoggerName method

        [Test]
        public void GenerateDefaultLoggerName_When_Called_Creates_Random_String_Of_Predefined_Form()
        {
            // Act
            var res = Logger.GenerateDefaultLoggerName();

            // Assert
            Assert.AreEqual(15, res.Length);
            Assert.AreEqual("Logger_", res.Substring(0, 7));
        }

        #endregion

        #region Tests for Dispose method

        [Test]
        public void Dispose_When_Called_Calls_Dispose_Method_Of_All_Log_Groups()
        {
            Logger logger = new Logger();
            // Log group mocks.
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("logGroup1");
            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("logGroup2");
            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("logGroup3");
            // Add log groups to the logger.
            logger.AddLogGroup(logGroup1Mock.Object);
            logger.AddLogGroup(logGroup2Mock.Object);
            logger.AddLogGroup(logGroup3Mock.Object);

            // Act
            logger.Dispose();

            // Assert
            logGroup1Mock.Verify(x => x.Dispose(), Times.Once);
            logGroup2Mock.Verify(x => x.Dispose(), Times.Once);
            logGroup3Mock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Group_Dispose_Method_Throws_Swallows_The_Exception()
        {
            Logger logger = new Logger();
            // Log group mocks.
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("logGroup1");
            logGroup1Mock.Setup(x => x.Dispose()).Throws<Exception>();
            var logGroup2Mock = new Mock<LogGroupBase>();
            // Add log groups to the logger.
            logger.AddLogGroup(logGroup1Mock.Object);
            logger.AddLogGroup(logGroup2Mock.Object);

            // Act
            logger.Dispose();

            // Assert
            logGroup1Mock.Verify(x => x.Dispose(), Times.Once);
            logGroup2Mock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_Calls_Dispose_Method_Of_All_Log_Contributors()
        {
            // Arrange
            Logger logger = new Logger();
            // Log contributors mocks.
            var logContributor1Mock = new Mock<LogContributorBase>("name1");
            var logContributor2Mock = new Mock<LogContributorBase>("name2");
            var logContributor3Mock = new Mock<LogContributorBase>("name3");
            // Add log contributors to the logger.
            logger.AddLogContributor(logContributor1Mock.Object);
            logger.AddLogContributor(logContributor2Mock.Object);
            logger.AddLogContributor(logContributor3Mock.Object);

            // Act
            logger.Dispose();

            // Assert
            logContributor1Mock.Verify(x => x.Dispose(), Times.Once);
            logContributor2Mock.Verify(x => x.Dispose(), Times.Once);
            logContributor3Mock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Contributor_Dispose_Method_Throws_Swallows_The_Exception()
        {
            // Arrange
            Logger logger = new Logger();
            // Log contributors mocks.
            var logContributor1Mock = new Mock<LogContributorBase>("name");
            logContributor1Mock.Setup(x => x.Dispose()).Throws<Exception>();
            var logContributor2Mock = new Mock<LogContributorBase>("name");
            // Add log destinations to the logger.
            logger.AddLogContributor(logContributor1Mock.Object);
            logger.AddLogContributor(logContributor2Mock.Object);

            // Act
            logger.Dispose();

            // Assert
            logContributor1Mock.Verify(x => x.Dispose(), Times.Once);
            logContributor2Mock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_Calls_Dispose_Method_Of_All_Log_Transformers()
        {
            // Arrange
            Logger logger = new Logger();
            // Log transformers mocks.
            var logTransformer1Mock = new Mock<LogTransformerBase>("name1");
            var logTransformer2Mock = new Mock<LogTransformerBase>("name2");
            var logTransformer3Mock = new Mock<LogTransformerBase>("name3");
            // Add log transformers to the logger.
            logger.AddLogTransformer(logTransformer1Mock.Object);
            logger.AddLogTransformer(logTransformer2Mock.Object);
            logger.AddLogTransformer(logTransformer3Mock.Object);

            // Act
            logger.Dispose();

            // Assert
            logTransformer1Mock.Verify(x => x.Dispose(), Times.Once);
            logTransformer2Mock.Verify(x => x.Dispose(), Times.Once);
            logTransformer3Mock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Transformer_Dispose_Method_Throws_Swallows_The_Exception()
        {
            // Arrange
            Logger logger = new Logger();
            // Log transformers mocks.
            var logTransformer1Mock = new Mock<LogTransformerBase>("name1");
            logTransformer1Mock.Setup(x => x.Dispose()).Throws<Exception>();
            var logTransformer2Mock = new Mock<LogTransformerBase>("name2");
            // Add log transformers to the logger.
            logger.AddLogTransformer(logTransformer1Mock.Object);
            logger.AddLogTransformer(logTransformer2Mock.Object);

            // Act
            logger.Dispose();

            // Assert
            logTransformer1Mock.Verify(x => x.Dispose(), Times.Once);
            logTransformer2Mock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_Calls_Log_Id_Generator_Dispose_Method()
        {
            // Arrange
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            Logger logger = new Logger("", logIdGeneratorMock.Object);

            // Act
            logger.Dispose();

            // Assert
            logIdGeneratorMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Id_Generator_Dispose_Method_Throws_Swallows_The_Exception()
        {
            // Arrange
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.Dispose()).Throws<Exception>();
            Logger logger = new Logger("", logIdGeneratorMock.Object);

            // Act
            logger.Dispose();

            // Assert
            logIdGeneratorMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_Calls_Log_Level_Value_Provider_Dispose_Method()
        {
            // Arrange
            var logLevelValueProviderMock = new Mock<Setting<LogLevel>>();
            Logger logger = new Logger("", new LoggerSettings { LogLevel = logLevelValueProviderMock.Object });

            // Act
            logger.Dispose();

            // Assert
            logLevelValueProviderMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Log_Level_Value_Provider_Dispose_Method_Throws_Swallows_The_Exception()
        {
            // Arrange
            var logLevelValueProviderMock = new Mock<Setting<LogLevel>>();
            logLevelValueProviderMock.Setup(x => x.Dispose()).Throws<Exception>();
            Logger logger = new Logger("", new LoggerSettings { LogLevel = logLevelValueProviderMock.Object });

            // Act
            logger.Dispose();

            // Assert
            logLevelValueProviderMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_Calls_Logger_Health_Info_Destination_Dispose_Method()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();
            Logger logger = new Logger();
            logger.SetDiagnosticsDestination(logDestinationMock.Object);

            // Act
            logger.Dispose();

            // Assert
            logDestinationMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Dispose_When_Called_If_Logger_Health_Info_Destination_Dispose_Method_Throws_Swallows_The_Exception()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();
            logDestinationMock.Setup(x => x.Dispose()).Throws<Exception>();
            Logger logger = new Logger();
            logger.SetDiagnosticsDestination(logDestinationMock.Object);

            // Act
            logger.Dispose();

            // Assert
            logDestinationMock.Verify(x => x.Dispose(), Times.Once);
        }

        #endregion

        #region Tests for ToString method

        [Test]
        public void ToString_When_Called_Contains_Information_About_The_Logger()
        {
            // Arrange
            var loggerName = "Logger1";
            Logger logger = new Logger(loggerName);

            // Act
            var result = logger.ToString();

            // Assert 
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(loggerName));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_All_Log_Contributors()
        {
            // Arrange 
            var logContributor1Mock = new Mock<LogContributorBase>("contributor1");
            var contributor1Description = "contributor1Description";
            logContributor1Mock.Setup(x => x.ToString()).Returns(contributor1Description);

            var logContributor2Mock = new Mock<LogContributorBase>("contributor2");
            var contributor2Description = "contributor2Description";
            logContributor2Mock.Setup(x => x.ToString()).Returns(contributor2Description);

            var logContributor3Mock = new Mock<LogContributorBase>("contributor3");
            var contributor3Description = "contributor3Description";
            logContributor3Mock.Setup(x => x.ToString()).Returns(contributor3Description);

            Logger logger = new Logger();
            logger.AddLogContributor(logContributor1Mock.Object);
            logger.AddLogContributor(logContributor2Mock.Object);
            logger.AddLogContributor(logContributor3Mock.Object);

            // Act
            var result = logger.ToString();

            // Assert
            logContributor1Mock.Verify(x => x.ToString(), Times.Once);
            logContributor2Mock.Verify(x => x.ToString(), Times.Once);
            logContributor3Mock.Verify(x => x.ToString(), Times.Once);

            Assert.IsTrue(result.Contains($"1. {contributor1Description}"));
            Assert.IsTrue(result.Contains($"2. {contributor2Description}"));
            Assert.IsTrue(result.Contains($"3. {contributor3Description}"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_All_Log_Transformers()
        {
            // Arrange 
            var logTransformer1Mock = new Mock<LogTransformerBase>("transformer1");
            var transfomer1Description = "transfomer1Description";
            logTransformer1Mock.Setup(x => x.ToString()).Returns(transfomer1Description);

            var logTransformer2Mock = new Mock<LogTransformerBase>("transformer2");
            var transfomer2Description = "transfomer2Description";
            logTransformer2Mock.Setup(x => x.ToString()).Returns(transfomer2Description);

            var logTransformer3Mock = new Mock<LogTransformerBase>("transformer3");
            var transfomer3Description = "transfomer3Description";
            logTransformer3Mock.Setup(x => x.ToString()).Returns(transfomer3Description);

            Logger logger = new Logger();
            logger.AddLogTransformer(logTransformer1Mock.Object);
            logger.AddLogTransformer(logTransformer2Mock.Object);
            logger.AddLogTransformer(logTransformer3Mock.Object);

            // Act
            var result = logger.ToString();

            // Assert
            logTransformer1Mock.Verify(x => x.ToString(), Times.Once);
            logTransformer2Mock.Verify(x => x.ToString(), Times.Once);
            logTransformer3Mock.Verify(x => x.ToString(), Times.Once);

            Assert.IsTrue(result.Contains($"1. {transfomer1Description}"));
            Assert.IsTrue(result.Contains($"2. {transfomer2Description}"));
            Assert.IsTrue(result.Contains($"3. {transfomer3Description}"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_All_Log_Groups()
        {
            // Arrange 
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("name1");
            var logGroup1Description = "logGroup1Description";
            logGroup1Mock.Setup(x => x.ToString()).Returns(logGroup1Description);

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("name2");
            var logGroup2Description = "logGroup2Description";
            logGroup2Mock.Setup(x => x.ToString()).Returns(logGroup2Description);

            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("name3");
            var logGroup3Description = "logGroup3Description";
            logGroup3Mock.Setup(x => x.ToString()).Returns(logGroup3Description);

            Logger logger = new Logger();
            logger.AddLogGroup(logGroup1Mock.Object);
            logger.AddLogGroup(logGroup2Mock.Object);
            logger.AddLogGroup(logGroup3Mock.Object);

            // Act
            var result = logger.ToString();

            // Assert
            logGroup1Mock.Verify(x => x.ToString(), Times.Once);
            logGroup2Mock.Verify(x => x.ToString(), Times.Once);
            logGroup3Mock.Verify(x => x.ToString(), Times.Once);

            Assert.IsTrue(result.Contains($"1. {logGroup1Description}"));
            Assert.IsTrue(result.Contains($"2. {logGroup2Description}"));
            Assert.IsTrue(result.Contains($"3. {logGroup3Description}"));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Id_Generator()
        {
            // Arrange 
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            var logIdGeneratorDescription = "logIdGeneratorDescription";
            logIdGeneratorMock.Setup(x => x.ToString()).Returns(logIdGeneratorDescription);
            Logger logger = new Logger("Logger1", logIdGeneratorMock.Object);

            // Act
            var result = logger.ToString();

            // Assert
            logIdGeneratorMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logIdGeneratorDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Creation_Strategy()
        {
            // Arrange 
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            var logCreationStrategyDescription = "logCreationStrategyDescription";
            logCreationStrategyMock.Setup(x => x.ToString()).Returns(logCreationStrategyDescription);
            Logger logger = new Logger();
            logger.ResetLogCreationStrategy(logCreationStrategyMock.Object);

            // Act
            var result = logger.ToString();

            // Assert
            logCreationStrategyMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logCreationStrategyDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Exception_Handling_Strategy()
        {
            // Arrange 
            var exceptionHandlingStrategyMock = new Mock<ExceptionHandlingStrategyBase>();
            var exceptionHandlingStrategyDescription = "exceptionHandlingStrategyDescription";
            exceptionHandlingStrategyMock.Setup(x => x.ToString()).Returns(exceptionHandlingStrategyDescription);
            Logger logger = new Logger();
            logger.ResetExceptionHandlingStrategy(exceptionHandlingStrategyMock.Object);

            // Act
            var result = logger.ToString();

            // Assert
            exceptionHandlingStrategyMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(exceptionHandlingStrategyDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Diagnostics_Destnation()
        {
            // Arrange 
            var diagnosticsDestinationMock = new Mock<ILogDestination>();
            var diagnosticsDestinationDescription = "diagnosticsDestinationDescription";
            diagnosticsDestinationMock.Setup(x => x.ToString()).Returns(diagnosticsDestinationDescription);
            Logger logger = new Logger();
            logger.SetDiagnosticsDestination(diagnosticsDestinationMock.Object);

            // Act
            var result = logger.ToString();

            // Assert
            diagnosticsDestinationMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(diagnosticsDestinationDescription));
        }
        [Test]
        public void ToString_When_Called_Calls_ToString_Method_Of_Log_Level_Setting()
        {
            // Arrange 
            var logLevelSettingMock = new Mock<Setting<LogLevel>>();
            var logLevelSettingDescription = "logLevelSettingDescription";
            logLevelSettingMock.Setup(x => x.ToString()).Returns(logLevelSettingDescription);
            Logger logger = new Logger("Logger1", new LoggerSettings() { LogLevel = logLevelSettingMock.Object });

            // Act
            var result = logger.ToString();

            // Assert
            logLevelSettingMock.Verify(x => x.ToString(), Times.Once);
            Assert.IsTrue(result.Contains(logLevelSettingDescription));
        }

        #endregion
    }
}
