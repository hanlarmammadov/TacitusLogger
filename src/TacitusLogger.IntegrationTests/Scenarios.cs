using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Builders;
using TacitusLogger.Caching;
using TacitusLogger.Contributors;
using TacitusLogger.Destinations;
using TacitusLogger.Exceptions;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.DestinationFeeding;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;
using TacitusLogger.Transformers;

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class Scenarios
    {
        #region Helper methods

        private void AssertsForLogLevelAll(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            logger.Info("").Log();
            logger.Info("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsInfo)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Success("").Log();
            logger.Success("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsSuccess)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Event("").Log();
            logger.Event("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsEvent)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Warning("").Log();
            logger.Warning("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsWarning)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Error("").Log();
            logger.Error("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsError)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Failure("").Log();
            logger.Failure("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsFailure)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Critical("").Log();
            logger.Critical("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsCritical)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private void AssertsForLogLevelInfo(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            logger.Info("").Log();
            logger.Info("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsInfo)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Success("").Log();
            logger.Success("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsSuccess)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Event("").Log();
            logger.Event("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsEvent)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Warning("").Log();
            logger.Warning("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsWarning)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Error("").Log();
            logger.Error("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsError)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Failure("").Log();
            logger.Failure("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsFailure)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Critical("").Log();
            logger.Critical("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsCritical)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private void AssertsForLogLevelWarning(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            logger.Info("").Log();
            logger.Info("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsInfo)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Success("").Log();
            logger.Success("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsSuccess)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Event("").Log();
            logger.Event("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsEvent)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Warning("").Log();
            logger.Warning("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsWarning)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Error("").Log();
            logger.Error("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsError)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Failure("").Log();
            logger.Failure("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsFailure)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Critical("").Log();
            logger.Critical("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsCritical)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private void AssertsForLogLevelError(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            logger.Info("").Log();
            logger.Info("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsInfo)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Success("").Log();
            logger.Success("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsSuccess)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Event("").Log();
            logger.Event("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsEvent)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Warning("").Log();
            logger.Warning("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsWarning)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Error("").Log();
            logger.Error("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsError)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Failure("").Log();
            logger.Failure("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsFailure)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            logger.Critical("").Log();
            logger.Critical("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsCritical)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private void AssertsForLogLevelNone(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            logger.Info("").Log();
            logger.Info("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsInfo)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Success("").Log();
            logger.Success("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsSuccess)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Event("").Log();
            logger.Event("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsEvent)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Warning("").Log();
            logger.Warning("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsWarning)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Error("").Log();
            logger.Error("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsError)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Failure("").Log();
            logger.Failure("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsFailure)), Times.Never);
            logGroupMock.Invocations.Clear();

            logger.Critical("").Log();
            logger.Critical("").Log();
            logGroupMock.Verify(x => x.Send(It.Is<LogModel>(m => m.IsCritical)), Times.Never);
            logGroupMock.Invocations.Clear();
        }

        private async Task AsyncAssertsForLogLevelAll(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            await logger.Info("").LogAsync();
            await logger.Info("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsInfo), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Success("").LogAsync();
            await logger.Success("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsSuccess), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Event("").LogAsync();
            await logger.Event("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsEvent), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Warning("").LogAsync();
            await logger.Warning("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsWarning), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Error("").LogAsync();
            await logger.Error("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsError), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Failure("").LogAsync();
            await logger.Failure("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsFailure), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Critical("").LogAsync();
            await logger.Critical("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsCritical), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private async Task AsyncAssertsForLogLevelInfo(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            await logger.Info("").LogAsync();
            await logger.Info("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsInfo), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Success("").LogAsync();
            await logger.Success("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsSuccess), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Event("").LogAsync();
            await logger.Event("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsEvent), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Warning("").LogAsync();
            await logger.Warning("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsWarning), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Error("").LogAsync();
            await logger.Error("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsError), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Failure("").LogAsync();
            await logger.Failure("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsFailure), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Critical("").LogAsync();
            await logger.Critical("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsCritical), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private async Task AsyncAssertsForLogLevelWarning(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            await logger.Info("").LogAsync();
            await logger.Info("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsInfo), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Success("").LogAsync();
            await logger.Success("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsSuccess), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Event("").LogAsync();
            await logger.Event("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsEvent), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Warning("").LogAsync();
            await logger.Warning("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsWarning), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Error("").LogAsync();
            await logger.Error("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsError), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Failure("").LogAsync();
            await logger.Failure("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsFailure), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Critical("").LogAsync();
            await logger.Critical("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsCritical), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private async Task AsyncAssertsForLogLevelError(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            await logger.Info("").LogAsync();
            await logger.Info("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsInfo), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Success("").LogAsync();
            await logger.Success("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsSuccess), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Event("").LogAsync();
            await logger.Event("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsEvent), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Warning("").LogAsync();
            await logger.Warning("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsWarning), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Error("").LogAsync();
            await logger.Error("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsError), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Failure("").LogAsync();
            await logger.Failure("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsFailure), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();

            await logger.Critical("").LogAsync();
            await logger.Critical("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsCritical), default(CancellationToken)), Times.Exactly(2));
            logGroupMock.Invocations.Clear();
        }
        private async Task AsyncAssertsForLogLevelNone(ILogger logger, Mock<LogGroupBase> logGroupMock)
        {
            await logger.Info("").LogAsync();
            await logger.Info("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsInfo), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Success("").LogAsync();
            await logger.Success("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsSuccess), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Event("").LogAsync();
            await logger.Event("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsEvent), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Warning("").LogAsync();
            await logger.Warning("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsWarning), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Error("").LogAsync();
            await logger.Error("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsError), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Failure("").LogAsync();
            await logger.Failure("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsFailure), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();

            await logger.Critical("").LogAsync();
            await logger.Critical("").LogAsync();
            logGroupMock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.IsCritical), default(CancellationToken)), Times.Never);
            logGroupMock.Invocations.Clear();
        }

        #endregion

        [Test]
        public void Logger_With_Custom_Log_Creation_Strategy_Log_Groups_Receive_Created_Log_Model()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("group3");
            logGroup3Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup3Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .NewLogGroup(logGroup3Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            //Assert
            logGroup1Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup2Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup3Mock.Verify(x => x.Send(logModel), Times.Once);
        }
        [Test]
        public async Task Async_Logger_With_Custom_Log_Creation_Strategy_Log_Groups_Receive_Created_Log_Model()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("group3");
            logGroup3Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup3Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .NewLogGroup(logGroup3Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            //Assert
            logGroup1Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup2Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup3Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
        }
        [Test]
        public void Logger_With_Mutable_Log_Level_Log_Groups_Receive_Created_Log_Model_Depending_On_Log_Level()
        {
            // Arrange
            var logGroupMock = new Mock<LogGroupBase>();
            logGroupMock.SetupGet(x => x.Name).Returns("group1");
            logGroupMock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroupMock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            MutableSetting<LogLevel> logLevel = Setting<LogLevel>.From.Variable(LogLevel.All);

            ILogger logger = LoggerBuilder.Logger().WithLogLevel(logLevel)
                                                   .NewLogGroup(logGroupMock.Object)
                                                   .BuildLogger();
            // Act
            // Log level = All
            AssertsForLogLevelAll(logger, logGroupMock);

            // Log level = Info
            logLevel.SetValue(LogLevel.Info);
            AssertsForLogLevelInfo(logger, logGroupMock);

            // Log level = Warning
            logLevel.SetValue(LogLevel.Warning);
            AssertsForLogLevelWarning(logger, logGroupMock);

            // Log level = Error
            logLevel.SetValue(LogLevel.Error);
            AssertsForLogLevelError(logger, logGroupMock);

            // Log level = None
            logLevel.SetValue(LogLevel.None);
            AssertsForLogLevelNone(logger, logGroupMock);
        }
        [Test]
        public async Task Async_Logger_With_Mutable_Log_Level_Log_Groups_Receive_Created_Log_Model_Depending_On_Log_Level()
        {
            // Arrange
            var logGroupMock = new Mock<LogGroupBase>();
            logGroupMock.SetupGet(x => x.Name).Returns("group1");
            logGroupMock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroupMock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            MutableSetting<LogLevel> logLevel = Setting<LogLevel>.From.Variable(LogLevel.All);

            ILogger logger = LoggerBuilder.Logger().WithLogLevel(logLevel)
                                                   .NewLogGroup(logGroupMock.Object)
                                                   .BuildLogger();
            // Act
            // Log level = All
            await AsyncAssertsForLogLevelAll(logger, logGroupMock);

            // Log level = Info
            logLevel.SetValue(LogLevel.Info);
            await AsyncAssertsForLogLevelInfo(logger, logGroupMock);

            // Log level = Warning
            logLevel.SetValue(LogLevel.Warning);
            await AsyncAssertsForLogLevelWarning(logger, logGroupMock);

            // Log level = Error
            logLevel.SetValue(LogLevel.Error);
            await AsyncAssertsForLogLevelError(logger, logGroupMock);

            // Log level = None
            logLevel.SetValue(LogLevel.None);
            await AsyncAssertsForLogLevelNone(logger, logGroupMock);
        }
        [Test]
        public void Logger_Containing_LogGroup_With_Greedy_Destination_Feeding_Serves_Logs_Correctly()
        {
            // Arrange
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup("group1")
                                                   .WithDestinationFeeding(DestinationFeeding.Greedy)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination1Mock.Object)
                                                   .CustomDestination(logDestination2Mock.Object)
                                                   .CustomDestination(logDestination3Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            //Assert
            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel)), Times.Once);
            logDestination1Mock.VerifyNoOtherCalls();
            //
            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel)), Times.Once);
            logDestination2Mock.VerifyNoOtherCalls();
            //            
            logDestination3Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel)), Times.Once);
            logDestination3Mock.VerifyNoOtherCalls();
        }
        [Test]
        public async Task Async_Logger_Containing_LogGroup_With_Greedy_Destination_Feeding_Serves_Logs_Correctly()
        {
            // Arrange
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup("group1")
                                                   .WithDestinationFeeding(DestinationFeeding.Greedy)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination1Mock.Object)
                                                   .CustomDestination(logDestination2Mock.Object)
                                                   .CustomDestination(logDestination3Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            //Assert
            logDestination1Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel), cancellationToken), Times.Once);
            logDestination1Mock.VerifyNoOtherCalls();
            //
            logDestination2Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel), cancellationToken), Times.Once);
            logDestination2Mock.VerifyNoOtherCalls();
            //            
            logDestination3Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel), cancellationToken), Times.Once);
            logDestination3Mock.VerifyNoOtherCalls();
        }
        [Test]
        public void Logger_Containing_LogGroup_With_FirstSuccess_Destination_Feeding_Serves_Logs_Correctly()
        {
            // Arrange
            var logDestination1Mock = new Mock<ILogDestination>();
            logDestination1Mock.Setup(x => x.Send(It.IsAny<LogModel[]>())).Throws<Exception>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup("group1")
                                                   .WithDestinationFeeding(DestinationFeeding.FirstSuccess)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination1Mock.Object)
                                                   .CustomDestination(logDestination2Mock.Object)
                                                   .CustomDestination(logDestination3Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            //Assert
            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel)), Times.Once);
            logDestination1Mock.VerifyNoOtherCalls();
            //
            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel)), Times.Once);
            logDestination2Mock.VerifyNoOtherCalls();
            //            
            logDestination3Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel)), Times.Never);
            logDestination3Mock.VerifyNoOtherCalls();
        }
        [Test]
        public async Task Async_Logger_Containing_LogGroup_With_FirstSuccess_Destination_Feeding_Serves_Logs_Correctly()
        {
            // Arrange
            var logDestination1Mock = new Mock<ILogDestination>();
            logDestination1Mock.Setup(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>())).Throws<Exception>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logDestination3Mock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup("group1")
                                                   .WithDestinationFeeding(DestinationFeeding.FirstSuccess)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination1Mock.Object)
                                                   .CustomDestination(logDestination2Mock.Object)
                                                   .CustomDestination(logDestination3Mock.Object)
                                                   .BuildLogger();
            // Act 
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            //Assert
            logDestination1Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel), cancellationToken), Times.Once);
            logDestination1Mock.VerifyNoOtherCalls();
            //
            logDestination2Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel), cancellationToken), Times.Once);
            logDestination2Mock.VerifyNoOtherCalls();
            //
            logDestination3Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 && m[0] == logModel), cancellationToken), Times.Never);
            logDestination3Mock.VerifyNoOtherCalls();
        }
        [Test]
        public void Logger_Containing_LogGroup_With_Cache_Enabled_Implements_Cache_Logic_Correctly()
        {
            // Arrange
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel1 = new LogModel();
            LogModel logModel2 = new LogModel();
            LogModel logModel3 = new LogModel();
            LogModel logModel4 = new LogModel();
            logCreationStrategyMock.SetupSequence(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>()))
                                   .Returns(logModel1)
                                   .Returns(logModel2)
                                   .Returns(logModel3)
                                   .Returns(logModel4);
            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup("group1")
                                                   .WithDestinationFeeding(DestinationFeeding.Greedy)
                                                   .WithCaching(3, 100000)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination1Mock.Object)
                                                   .CustomDestination(logDestination2Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            logDestination1Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestination2Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);

            logger.Log(new Log());

            logDestination1Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestination2Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);

            logger.Log(new Log());

            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 3 && m[0] == logModel1 && m[1] == logModel2 && m[2] == logModel3)), Times.Once);
            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 3 && m[0] == logModel1 && m[1] == logModel2 && m[2] == logModel3)), Times.Once);
            logDestination1Mock.VerifyNoOtherCalls();
            logDestination2Mock.VerifyNoOtherCalls();
            logDestination1Mock.Invocations.Clear();
            logDestination2Mock.Invocations.Clear();

            logger.Log(new Log());

            logDestination1Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestination2Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);

            logDestination1Mock.VerifyNoOtherCalls();
            logDestination2Mock.VerifyNoOtherCalls();
        }
        [Test]
        public async Task Async_Logger_Containing_LogGroup_With_Cache_Enabled_Implements_Cache_Logic_Correctly()
        {
            // Arrange
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel1 = new LogModel();
            LogModel logModel2 = new LogModel();
            LogModel logModel3 = new LogModel();
            LogModel logModel4 = new LogModel();
            logCreationStrategyMock.SetupSequence(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(logModel1)
                                   .ReturnsAsync(logModel2)
                                   .ReturnsAsync(logModel3)
                                   .ReturnsAsync(logModel4);
            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup("group1")
                                                   .WithDestinationFeeding(DestinationFeeding.Greedy)
                                                   .WithCaching(3, 100000)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination1Mock.Object)
                                                   .CustomDestination(logDestination2Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            logDestination1Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);
            logDestination2Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);

            await logger.LogAsync(new Log());

            logDestination1Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);
            logDestination2Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);

            await logger.LogAsync(new Log());

            logDestination1Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 3 && m[0] == logModel1 && m[1] == logModel2 && m[2] == logModel3), cancellationToken), Times.Once);
            logDestination2Mock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 3 && m[0] == logModel1 && m[1] == logModel2 && m[2] == logModel3), cancellationToken), Times.Once);
            logDestination1Mock.VerifyNoOtherCalls();
            logDestination2Mock.VerifyNoOtherCalls();
            logDestination1Mock.Invocations.Clear();
            logDestination2Mock.Invocations.Clear();

            await logger.LogAsync(new Log());

            logDestination1Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);
            logDestination2Mock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);

            logDestination1Mock.VerifyNoOtherCalls();
            logDestination2Mock.VerifyNoOtherCalls();
        }
        [Test]
        public void Logger_With_Log_Contributors_Adds_Log_Items_To_Logs_Correctly()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();

            var contr1 = new { a = 1 };
            var contr2 = new { b = "b" };
            var logContributor1 = new UserDataContributor("Contributor1", contr1);
            var logContributor2 = new UserDataContributor("Contributor2", contr2);


            ILogger logger = LoggerBuilder.Logger().WithLogCreation(LogCreation.Standard)
                                                   .Contributors()
                                                   .Custom(logContributor1)
                                                   .Custom(logContributor2)
                                                   .BuildContributors()
                                                   .NewLogGroup("group1")
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestinationMock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                         m[0].LogItems.Length == 2 &&
                                                                         m[0].LogItems[0].Name == "Contributor1" &&
                                                                         m[0].LogItems[0].Value == contr1 &&
                                                                         m[0].LogItems[1].Name == "Contributor2" &&
                                                                         m[0].LogItems[1].Value == contr2)), Times.Once);
            logDestinationMock.VerifyNoOtherCalls();
            logDestinationMock.Invocations.Clear();

            logger.Log(new Log());
            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                         m[0].LogItems.Length == 2 &&
                                                                         m[0].LogItems[0].Name == "Contributor1" &&
                                                                         m[0].LogItems[0].Value == contr1 &&
                                                                         m[0].LogItems[1].Name == "Contributor2" &&
                                                                         m[0].LogItems[1].Value == contr2)), Times.Once);
        }
        [Test]
        public async Task Async_Logger_With_Log_Contributors_Adds_Log_Items_To_Logs_Correctly()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();

            var contr1 = new { a = 1 };
            var contr2 = new { b = "b" };
            var logContributor1 = new UserDataContributor("Contributor1", contr1);
            var logContributor2 = new UserDataContributor("Contributor2", contr2);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(LogCreation.Standard)
                                                   .Contributors()
                                                   .Custom(logContributor1)
                                                   .Custom(logContributor2)
                                                   .BuildContributors()
                                                   .NewLogGroup("group1")
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestinationMock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                              m[0].LogItems.Length == 2 &&
                                                                              m[0].LogItems[0].Name == "Contributor1" &&
                                                                              m[0].LogItems[0].Value == contr1 &&
                                                                              m[0].LogItems[1].Name == "Contributor2" &&
                                                                              m[0].LogItems[1].Value == contr2), cancellationToken), Times.Once);
            logDestinationMock.VerifyNoOtherCalls();
            logDestinationMock.Invocations.Clear();

            await logger.LogAsync(new Log(), cancellationToken);
            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                              m[0].LogItems.Length == 2 &&
                                                                              m[0].LogItems[0].Name == "Contributor1" &&
                                                                              m[0].LogItems[0].Value == contr1 &&
                                                                              m[0].LogItems[1].Name == "Contributor2" &&
                                                                              m[0].LogItems[1].Value == contr2), cancellationToken), Times.Once);
        }
        [Test]
        public void Logger_With_Custom_Exception_Handling_Strategy_Handles_Exception_From_Contributor()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();

            var exceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);

            var logContributorMock = new Mock<LogContributorBase>("contributor1");
            Exception ex = new Exception();
            logContributorMock.Setup(x => x.ProduceLogItem()).Throws(ex);

            ILogger logger = LoggerBuilder.Logger("logger1").WithLogCreation(LogCreation.Standard)
                                                            .WithExceptionHandling(exceptionHandlingStrategyMock.Object)
                                                            .Contributors()
                                                            .Custom(logContributorMock.Object)
                                                            .BuildContributors()
                                                            .NewLogGroup("group1")
                                                            .ForAllLogs()
                                                            .CustomDestination(logDestinationMock.Object)
                                                            .BuildLogger();
            // Act
            logger.Log(new Log(LogType.Critical, "description1"));

            //Assert
            exceptionHandlingStrategyMock.Verify(x => x.HandleException(ex, "Log contributor: contributor1"), Times.Once);
            exceptionHandlingStrategyMock.VerifyGet(x => x.ShouldRethrow, Times.Once);
            exceptionHandlingStrategyMock.VerifyNoOtherCalls();

            logDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 && m[0].LogType == LogType.Critical)), Times.Once);
            logDestinationMock.VerifyNoOtherCalls();
        }
        [Test]
        public async Task Async_Logger_With_Custom_Exception_Handling_Strategy_Handles_Exception_From_Contributor()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();

            var exceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);

            var logContributorMock = new Mock<LogContributorBase>("contributor1");
            Exception ex = new Exception();
            logContributorMock.Setup(x => x.ProduceLogItemAsync(It.IsAny<CancellationToken>())).ThrowsAsync(ex);

            ILogger logger = LoggerBuilder.Logger("logger1").WithLogCreation(LogCreation.Standard)
                                                            .WithExceptionHandling(exceptionHandlingStrategyMock.Object)
                                                            .Contributors()
                                                            .Custom(logContributorMock.Object)
                                                            .BuildContributors()
                                                            .NewLogGroup("group1")
                                                            .ForAllLogs()
                                                            .CustomDestination(logDestinationMock.Object)
                                                            .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(LogType.Critical, "description1"), cancellationToken);

            //Assert
            exceptionHandlingStrategyMock.Verify(x => x.HandleExceptionAsync(ex, "Log contributor: contributor1", cancellationToken), Times.Once);
            exceptionHandlingStrategyMock.VerifyGet(x => x.ShouldRethrow, Times.Once);
            exceptionHandlingStrategyMock.VerifyNoOtherCalls();

            logDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 && m[0].LogType == LogType.Critical), cancellationToken), Times.Once);
            logDestinationMock.VerifyNoOtherCalls();
        }
        [Test]
        public void Logger_With_Log_Transformers_Transforms_Logs_Correctly()
        {
            // Arrange 
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logTransformer1Mock = new Mock<LogTransformerBase>("contributor1");
            logTransformer1Mock.Setup(x => x.Transform(It.IsAny<LogModel>())).Callback((LogModel m) => { m.Context = m.Context + "1"; });
            var logTransformer2Mock = new Mock<LogTransformerBase>("contributor2");
            logTransformer2Mock.Setup(x => x.Transform(It.IsAny<LogModel>())).Callback((LogModel m) => { m.Context = m.Context + "2"; });
            var logTransformer3Mock = new Mock<LogTransformerBase>("contributor3");
            logTransformer3Mock.Setup(x => x.Transform(It.IsAny<LogModel>())).Callback((LogModel m) => { m.Context = m.Context + "3"; });

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel() { Context = "0" };
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .Transformers()
                                                       .Custom(logTransformer1Mock.Object)
                                                       .Custom(logTransformer2Mock.Object)
                                                       .Custom(logTransformer3Mock.Object)
                                                   .BuildTransformers()
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            // Assert
            logGroup1Mock.Verify(x => x.Send(It.Is<LogModel>(m => m.Context == "0123")), Times.Once);
        }
        [Test]
        public async Task Async_Logger_With_Log_Transformers_Transforms_Logs_Correctly()
        {
            // Arrange 
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logTransformer1Mock = new Mock<LogTransformerBase>("contributor1");
            logTransformer1Mock.Setup(x => x.TransformAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).Callback(async (LogModel m, CancellationToken c) => { m.Context = m.Context + "1"; await Task.CompletedTask; });
            var logTransformer2Mock = new Mock<LogTransformerBase>("contributor2");
            logTransformer2Mock.Setup(x => x.TransformAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).Callback(async (LogModel m, CancellationToken c) => { m.Context = m.Context + "2"; await Task.CompletedTask; });
            var logTransformer3Mock = new Mock<LogTransformerBase>("contributor3");
            logTransformer3Mock.Setup(x => x.TransformAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).Callback(async (LogModel m, CancellationToken c) => { m.Context = m.Context + "3"; await Task.CompletedTask; });

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel() { Context = "0" };
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .Transformers()
                                                       .Custom(logTransformer1Mock.Object)
                                                       .Custom(logTransformer2Mock.Object)
                                                       .Custom(logTransformer3Mock.Object)
                                                   .BuildTransformers()
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            // Assert
            logGroup1Mock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.Context == "0123"), cancellationToken), Times.Once);
        }
        [Test]
        public void Logger_With_Custom_Exception_Handling_Strategy_Handles_Exception_From_Transformer_Correctly()
        {
            // Arrange 
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logTransformer1Mock = new Mock<LogTransformerBase>("transformer1");
            logTransformer1Mock.Setup(x => x.Transform(It.IsAny<LogModel>())).Callback((LogModel m) => { m.Context = m.Context + "1"; });
            var logTransformer2Mock = new Mock<LogTransformerBase>("transformer2");
            logTransformer2Mock.Setup(x => x.Transform(It.IsAny<LogModel>())).Throws<Exception>();
            var logTransformer3Mock = new Mock<LogTransformerBase>("transformer3");
            logTransformer3Mock.Setup(x => x.Transform(It.IsAny<LogModel>())).Callback((LogModel m) => { m.Context = m.Context + "3"; });

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel() { Context = "0" };
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            var exceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);

            Logger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .WithExceptionHandling(exceptionHandlingStrategyMock.Object)
                                                   .Transformers()
                                                       .Custom(logTransformer1Mock.Object)
                                                       .Custom(logTransformer2Mock.Object)
                                                       .Custom(logTransformer3Mock.Object)
                                                   .BuildTransformers()
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            // Assert
            exceptionHandlingStrategyMock.Verify(x => x.HandleException(It.IsAny<Exception>(), "transformer2"));
            logGroup1Mock.Verify(x => x.Send(It.Is<LogModel>(m => m.Context == "013")), Times.Once);
        }
        [Test]
        public async Task Async_Logger_With_Custom_Exception_Handling_Strategy_Handles_Exception_From_Transformer_Correctly()
        {
            // Arrange 
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            var logTransformer1Mock = new Mock<LogTransformerBase>("transformer1");
            logTransformer1Mock.Setup(x => x.TransformAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).Callback((LogModel m, CancellationToken t) => { m.Context = m.Context + "1"; });
            var logTransformer2Mock = new Mock<LogTransformerBase>("transformer2");
            logTransformer2Mock.Setup(x => x.TransformAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var logTransformer3Mock = new Mock<LogTransformerBase>("transformer3");
            logTransformer3Mock.Setup(x => x.TransformAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).Callback((LogModel m, CancellationToken t) => { m.Context = m.Context + "3"; });

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel() { Context = "0" };
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            var exceptionHandlingStrategyMock = new Mock<LogExceptionHandlingStrategy>();
            exceptionHandlingStrategyMock.SetupGet(x => x.ShouldRethrow).Returns(false);

            Logger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .WithExceptionHandling(exceptionHandlingStrategyMock.Object)
                                                   .Transformers()
                                                       .Custom(logTransformer1Mock.Object)
                                                       .Custom(logTransformer2Mock.Object)
                                                       .Custom(logTransformer3Mock.Object)
                                                   .BuildTransformers()
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            // Assert
            exceptionHandlingStrategyMock.Verify(x => x.HandleExceptionAsync(It.IsAny<Exception>(), "transformer2", cancellationToken));
            logGroup1Mock.Verify(x => x.SendAsync(It.Is<LogModel>(m => m.Context == "013"), cancellationToken), Times.Once);
        }
        [Test]
        public void Logger_With_Log_Exception_Handling_Strategy_And_Custom_Self_Monitoring_Destination_Handles_Exception_From_Log_Id_Generator()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();

            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            Exception ex = new Exception();
            logIdGeneratorMock.Setup(x => x.Generate(It.IsAny<LogModel>())).Throws(ex);

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            ILogger logger = LoggerBuilder.Logger("logger1").WithLogCreation(LogCreation.Standard)
                                                            .WithExceptionHandling(ExceptionHandling.Log)
                                                            .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                            .WithLogIdGenerator(logIdGeneratorMock.Object)
                                                            .WriteLoggerConfigurationToDiagnostics(false)
                                                            .NewLogGroup("group1")
                                                            .ForAllLogs()
                                                            .CustomDestination(logDestinationMock.Object)
                                                            .BuildLogger();
            // Act
            logger.Log(new Log());

            //Assert
            selfMonitoringDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                                    m[0].Description == $"Logger threw an exception. See the log item." &&
                                                                                    m[0].Source == "logger1" &&
                                                                                    m[0].Context == "Log method" &&
                                                                                    m[0].LogItems.Length == 1 &&
                                                                                    m[0].LogItems[0].Name == "Exception" &&
                                                                                    m[0].LogItems[0].Value is LoggerException &&
                                                                                    ((LoggerException)m[0].LogItems[0].Value).InnerException.InnerException == ex &&
                                                                                    m[0].LogType == LogType.Error
                                                                                )), Times.Once);
            selfMonitoringDestinationMock.VerifyNoOtherCalls();

            logDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestinationMock.VerifyNoOtherCalls();
        }
        [Test]
        public async Task Async_Logger_With_Log_Exception_Handling_Strategy_And_Custom_Self_Monitoring_Destination_Handles_Exception_From_Log_Id_Generator()
        {
            // Arrange
            var logDestinationMock = new Mock<ILogDestination>();

            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            Exception ex = new Exception();
            logIdGeneratorMock.Setup(x => x.GenerateAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).Throws(ex);

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            ILogger logger = LoggerBuilder.Logger("logger1").WithLogCreation(LogCreation.Standard)
                                                            .WithExceptionHandling(ExceptionHandling.Log)
                                                            .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                            .WithLogIdGenerator(logIdGeneratorMock.Object)
                                                            .WriteLoggerConfigurationToDiagnostics(false)
                                                            .NewLogGroup("group1")
                                                            .ForAllLogs()
                                                            .CustomDestination(logDestinationMock.Object)
                                                            .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            //Assert
            selfMonitoringDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                                    m[0].Description == $"Logger threw an exception. See the log item." &&
                                                                                    m[0].Source == "logger1" &&
                                                                                    m[0].Context == "LogAsync method" &&
                                                                                    m[0].LogItems.Length == 1 &&
                                                                                    m[0].LogItems[0].Name == "Exception" &&
                                                                                    m[0].LogItems[0].Value is LoggerException &&
                                                                                    ((LoggerException)m[0].LogItems[0].Value).InnerException.InnerException == ex &&
                                                                                    m[0].LogType == LogType.Error), cancellationToken), Times.Once);
            selfMonitoringDestinationMock.VerifyNoOtherCalls();

            logDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);
            logDestinationMock.VerifyNoOtherCalls();
        }
        [Test]
        public void Logger_With_Silent_Exception_Handling_And_Several_Log_Groups_Sends_Log_To_All_Groups_Despite_Of_The_Exceptions_They_Throw()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup1Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws<Exception>();

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup2Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws<Exception>();

            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("group3");
            logGroup3Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup3Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup3Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws<Exception>();

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Silent)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .NewLogGroup(logGroup3Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            //Assert 
            logGroup1Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup2Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup3Mock.Verify(x => x.Send(logModel), Times.Once);
        }
        [Test]
        public async Task Async_Logger_With_Silent_Exception_Handling_And_Several_Log_Groups_Sends_Log_To_All_Groups_Despite_Of_The_Exceptions_They_Throw()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup1Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup2Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("group3");
            logGroup3Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup3Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup3Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Silent)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .NewLogGroup(logGroup3Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            //Assert
            logGroup1Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup2Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup3Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
        }
        [Test]
        public void Logger_With_Silent_Exception_Handling_And_Self_Monitoring_Destination_Swallows_All_Thrown_Exceptions()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup1Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws<Exception>();

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup2Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws<Exception>();

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Silent)
                                                   .WriteLoggerConfigurationToDiagnostics(false)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            //Assert
            logGroup1Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup2Mock.Verify(x => x.Send(logModel), Times.Once);

            selfMonitoringDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }
        [Test]
        public async Task Async_Logger_With_Silent_Exception_Handling_And_Self_Monitoring_Destination_Swallows_All_Thrown_Exceptions()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup1Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            logGroup2Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Silent)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            //Assert
            logGroup1Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup2Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);

            selfMonitoringDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), cancellationToken), Times.Never);
        }
        [Test]
        public void Logger_With_Log_Exception_Handling_And_Self_Monitoring_Destination_Sends_All_Thrown_Exceptions_To_Self_Monitoring_Destination()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex1 = new Exception();
            logGroup1Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws(ex1);

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex2 = new Exception();
            logGroup2Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws(ex2);

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Log)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .BuildLogger();
            // Act
            logger.Log(new Log());

            //Assert
            logGroup1Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup2Mock.Verify(x => x.Send(logModel), Times.Once);

            selfMonitoringDestinationMock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                        m[0].LogType == LogType.Error &&
                                                                        m[0].LogItems[0].Value is LoggerException &&
                                                                        (((LoggerException)m[0].LogItems[0].Value).InnerException is AggregateException) &&
                                                                        (((LoggerException)m[0].LogItems[0].Value).InnerException as AggregateException).InnerExceptions.Count == 2 &&
                                                                        (((LoggerException)m[0].LogItems[0].Value).InnerException as AggregateException).InnerExceptions.Contains(ex1) &&
                                                                        (((LoggerException)m[0].LogItems[0].Value).InnerException as AggregateException).InnerExceptions.Contains(ex2))), Times.Once);
        }
        [Test]
        public async Task Async_Logger_With_Log_Exception_Handling_And_Self_Monitoring_Destination_Sends_All_Thrown_Exceptions_To_Self_Monitoring_Destination()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex1 = new Exception();
            logGroup1Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(ex1);

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex2 = new Exception();
            logGroup2Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(ex2);

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Log)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .BuildLogger();
            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await logger.LogAsync(new Log(), cancellationToken);

            //Assert
            logGroup1Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup2Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);

            selfMonitoringDestinationMock.Verify(x => x.SendAsync(It.Is<LogModel[]>(m => m.Length == 1 &&
                                                                                         m[0].LogType == LogType.Error &&
                                                                                         m[0].LogItems[0].Value is LoggerException &&
                                                                                         (((LoggerException)m[0].LogItems[0].Value).InnerException is AggregateException) &&
                                                                                         (((LoggerException)m[0].LogItems[0].Value).InnerException as AggregateException).InnerExceptions.Count == 2 &&
                                                                                         (((LoggerException)m[0].LogItems[0].Value).InnerException as AggregateException).InnerExceptions.Contains(ex1) &&
                                                                                         (((LoggerException)m[0].LogItems[0].Value).InnerException as AggregateException).InnerExceptions.Contains(ex2)), cancellationToken), Times.Once);
        }
        [Test]
        public void Logger_With_Rethrow_Exception_Handling_And_Self_Monitoring_Destination_Rethrow_Thrown_Exceptions()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex1 = new Exception();
            logGroup1Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws(ex1);

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex2 = new Exception();
            logGroup2Mock.Setup(x => x.Send(It.IsAny<LogModel>())).Throws(ex2);

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Rethrow)
                                                   .WriteLoggerConfigurationToDiagnostics(false)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .BuildLogger();

            //Assert
            var loggerEx = Assert.Catch<LoggerException>(() =>
            {
                // Act
                logger.Log(new Log());
            });

            logGroup1Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup2Mock.Verify(x => x.Send(logModel), Times.Once);

            Assert.IsInstanceOf<AggregateException>(loggerEx.InnerException);
            var aex = (AggregateException)loggerEx.InnerException;
            Assert.AreEqual(2, aex.InnerExceptions.Count);
            Assert.IsTrue(aex.InnerExceptions.Contains(ex1));
            Assert.IsTrue(aex.InnerExceptions.Contains(ex2));

            selfMonitoringDestinationMock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }
        [Test]
        public void Async_Logger_With_Rethrow_Exception_Handling_And_Self_Monitoring_Destination_Rethrow_Thrown_Exceptions()
        {
            // Arrange
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex1 = new Exception();
            logGroup1Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(ex1);

            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            Exception ex2 = new Exception();
            logGroup2Mock.Setup(x => x.SendAsync(It.IsAny<LogModel>(), It.IsAny<CancellationToken>())).ThrowsAsync(ex2);

            var selfMonitoringDestinationMock = new Mock<ILogDestination>();

            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            ILogger logger = LoggerBuilder.Logger().WithExceptionHandling(ExceptionHandling.Rethrow)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup(logGroup1Mock.Object)
                                                   .NewLogGroup(logGroup2Mock.Object)
                                                   .BuildLogger();
            CancellationToken cancellationToken = new CancellationToken();

            //Assert
            var loggerEx = Assert.CatchAsync<LoggerException>(async () =>
            {
                // Act
                await logger.LogAsync(new Log(), cancellationToken);
            });

            logGroup1Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup2Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);

            Assert.IsInstanceOf<AggregateException>(loggerEx.InnerException);
            var aex = (AggregateException)loggerEx.InnerException;
            Assert.AreEqual(2, aex.InnerExceptions.Count);
            Assert.IsTrue(aex.InnerExceptions.Contains(ex1));
            Assert.IsTrue(aex.InnerExceptions.Contains(ex2));

            selfMonitoringDestinationMock.Verify(x => x.SendAsync(It.IsAny<LogModel[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public void Logger_Containing_Several_LogGroups_Each_With_Own_Cache_When_Loggers_Dispose_Is_Called_Each_Cache_Is_Disposed_Correctly()
        {
            // Arrange
            // Log destination of first log group
            var logDestination1Mock = new Mock<ILogDestination>();
            // Log destination of second log group
            var logDestination2Mock = new Mock<ILogDestination>();
            // Log destination of third log group that will never get any log model.
            var logDestination3Mock = new Mock<ILogDestination>();

            // Log creation strategy will return new log model from set of 4 objects on each call.
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel1 = new LogModel();
            LogModel logModel2 = new LogModel();
            LogModel logModel3 = new LogModel();
            logCreationStrategyMock.SetupSequence(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>()))
                                   .Returns(logModel1)
                                   .Returns(logModel2)
                                   .Returns(logModel3);
            ILogger logger = LoggerBuilder.Logger().WithLogCreation(logCreationStrategyMock.Object)
                                                   .NewLogGroup("group1")
                                                   .WithCaching(3, 1000000)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination1Mock.Object)
                                                   .BuildLogGroup()
                                                   .NewLogGroup("group2")
                                                   .WithCaching(3, 1000000)
                                                   .ForAllLogs()
                                                   .CustomDestination(logDestination2Mock.Object)
                                                   .BuildLogGroup()
                                                   .NewLogGroup("group with empty cache")
                                                   .WithCaching(1, 1000000)
                                                   .ForEventLogs()
                                                   .CustomDestination(logDestination3Mock.Object)
                                                   .BuildLogger();

            // Act
            logger.Log(new Log());
            logger.Log(new Log());

            // Log group caches have not flushed at this point yet. 
            logDestination1Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
            logDestination2Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);


            // Dispose should trigger dispose of all log group caches and their forced flush.
            logger.Dispose();

            logDestination1Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 2 && m[0] == logModel1 && m[1] == logModel2)), Times.Once);
            logDestination2Mock.Verify(x => x.Send(It.Is<LogModel[]>(m => m.Length == 2 && m[0] == logModel1 && m[1] == logModel2)), Times.Once);
            // Third log group does not have any log models in cache, that is why its destination will not be called on dispose.
            logDestination3Mock.Verify(x => x.Send(It.IsAny<LogModel[]>()), Times.Never);
        }
        [Test]
        public void Logger_With_Various_Compenents_When_Dispose_Method_Of_Logger_Called_It_Triggers_Dispose_Methods_Of_All_Its_Components()
        {
            // Arrange 
            var logDestination1Mock = new Mock<ILogDestination>();
            var logDestination2Mock = new Mock<ILogDestination>();
            var selfMonitoringDestinationMock = new Mock<ILogDestination>();
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            var logCache1Mock = new Mock<ILogCache>();
            var logCache2Mock = new Mock<ILogCache>();
            var customLogGroupMock = new Mock<LogGroupBase>();
            var customLogContributorMock = new Mock<LogContributorBase>("contributor1");
            var customLogTransformerMock = new Mock<LogTransformerBase>("transformer1");
            var logLevelProviderMock = new Mock<Setting<LogLevel>>();
            var logGroup1StatusProviderMock = new Mock<Setting<LogGroupStatus>>();
            var logGroup2StatusProviderMock = new Mock<Setting<LogGroupStatus>>();
            var logContributorStatusProviderMock = new Mock<Setting<bool>>();
            var logTransformerStatusProviderMock = new Mock<Setting<bool>>();

            ILogger logger = LoggerBuilder.Logger().WithLogIdGenerator(logIdGeneratorMock.Object)
                                                   .WithLogLevel(logLevelProviderMock.Object)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .Contributors()
                                                       .StackTrace(logContributorStatusProviderMock.Object)
                                                       .Custom(customLogContributorMock.Object)
                                                   .BuildContributors()
                                                   .Transformers()
                                                       .StringsManual((ref string str) => { }, logTransformerStatusProviderMock.Object)
                                                       .Custom(customLogTransformerMock.Object)
                                                   .BuildTransformers()
                                                   .NewLogGroup("group1")
                                                       .SetStatus(logGroup1StatusProviderMock.Object)
                                                       .WithCaching(logCache1Mock.Object)
                                                       .ForAllLogs()
                                                       .CustomDestination(logDestination1Mock.Object)
                                                   .BuildLogGroup()
                                                   .NewLogGroup("group2")
                                                       .SetStatus(logGroup2StatusProviderMock.Object)
                                                       .WithCaching(logCache2Mock.Object)
                                                       .ForAllLogs()
                                                       .CustomDestination(logDestination2Mock.Object)
                                                   .BuildLogGroup()
                                                   .NewLogGroup(customLogGroupMock.Object)
                                                   .BuildLogger();
            // Act
            // Dispose should trigger dispose of all IDisposable components registered with it.
            logger.Dispose();

            // Assert
            logDestination1Mock.Verify(x => x.Dispose(), Times.Once);
            logDestination2Mock.Verify(x => x.Dispose(), Times.Once);
            selfMonitoringDestinationMock.Verify(x => x.Dispose(), Times.Once);
            logIdGeneratorMock.Verify(x => x.Dispose(), Times.Once);
            logCache1Mock.Verify(x => x.Dispose(), Times.Once);
            logCache2Mock.Verify(x => x.Dispose(), Times.Once);
            customLogGroupMock.Verify(x => x.Dispose(), Times.Once);
            customLogContributorMock.Verify(x => x.Dispose(), Times.Once);
            customLogTransformerMock.Verify(x => x.Dispose(), Times.Once);
            logLevelProviderMock.Verify(x => x.Dispose(), Times.Once);
            logGroup1StatusProviderMock.Verify(x => x.Dispose(), Times.Once);
            logGroup2StatusProviderMock.Verify(x => x.Dispose(), Times.Once);
            logContributorStatusProviderMock.Verify(x => x.Dispose(), Times.Once);
            logTransformerStatusProviderMock.Verify(x => x.Dispose(), Times.Once);
        }
        [Test]
        public void Logger_With_Various_Compenents_When_ToString_Method_Of_Logger_Called_It_Triggers_ToString_Methods_Of_All_Its_Components()
        {
            // Arrange 
            var logDestination1Mock = new Mock<ILogDestination>();
            logDestination1Mock.Setup(x => x.ToString()).Returns("");
            var logDestination2Mock = new Mock<ILogDestination>();
            logDestination2Mock.Setup(x => x.ToString()).Returns("");
            var selfMonitoringDestinationMock = new Mock<ILogDestination>();
            selfMonitoringDestinationMock.Setup(x => x.ToString()).Returns("");
            var logIdGeneratorMock = new Mock<ILogIdGenerator>();
            logIdGeneratorMock.Setup(x => x.ToString()).Returns("");
            var logCache1Mock = new Mock<ILogCache>();
            logCache1Mock.Setup(x => x.ToString()).Returns("");
            var logCache2Mock = new Mock<ILogCache>();
            logCache2Mock.Setup(x => x.ToString()).Returns("");
            var customLogGroupMock = new Mock<LogGroupBase>();
            customLogGroupMock.Setup(x => x.ToString()).Returns("");
            var customLogContributorMock = new Mock<LogContributorBase>("contributor1");
            customLogContributorMock.Setup(x => x.ToString()).Returns("");
            var customLogTransformerMock = new Mock<LogTransformerBase>("transformer1");
            customLogTransformerMock.Setup(x => x.ToString()).Returns("");
            var logLevelProviderMock = new Mock<Setting<LogLevel>>();
            logLevelProviderMock.Setup(x => x.ToString()).Returns("");
            var logGroup1StatusProviderMock = new Mock<Setting<LogGroupStatus>>();
            logGroup1StatusProviderMock.Setup(x => x.ToString()).Returns("");
            var logGroup2StatusProviderMock = new Mock<Setting<LogGroupStatus>>();
            logGroup2StatusProviderMock.Setup(x => x.ToString()).Returns("");
            var logContributorStatusProviderMock = new Mock<Setting<bool>>();
            logContributorStatusProviderMock.Setup(x => x.ToString()).Returns("");
            var logTransformerStatusProviderMock = new Mock<Setting<bool>>();
            logTransformerStatusProviderMock.Setup(x => x.ToString()).Returns("");

            ILogger logger = LoggerBuilder.Logger().WithLogIdGenerator(logIdGeneratorMock.Object)
                                                   .WithLogLevel(logLevelProviderMock.Object)
                                                   .WithSelfMonitoring(selfMonitoringDestinationMock.Object)
                                                   .WriteLoggerConfigurationToDiagnostics(false)
                                                   .Contributors()
                                                       .StackTrace(logContributorStatusProviderMock.Object)
                                                       .Custom(customLogContributorMock.Object)
                                                   .BuildContributors()
                                                   .Transformers()
                                                       .StringsManual((ref string str) => { }, logTransformerStatusProviderMock.Object)
                                                       .Custom(customLogTransformerMock.Object)
                                                   .BuildTransformers()
                                                   .NewLogGroup("group1")
                                                       .SetStatus(logGroup1StatusProviderMock.Object)
                                                       .WithCaching(logCache1Mock.Object)
                                                       .ForAllLogs()
                                                       .CustomDestination(logDestination1Mock.Object)
                                                   .BuildLogGroup()
                                                   .NewLogGroup("group2")
                                                       .SetStatus(logGroup2StatusProviderMock.Object)
                                                       .WithCaching(logCache2Mock.Object)
                                                       .ForAllLogs()
                                                       .CustomDestination(logDestination2Mock.Object)
                                                   .BuildLogGroup()
                                                   .NewLogGroup(customLogGroupMock.Object)
                                                   .BuildLogger();
            // Act
            // ToString should trigger ToString of all components registered with the logger.
            logger.ToString();

            // Assert
            logDestination1Mock.Verify(x => x.ToString(), Times.Once);
            logDestination2Mock.Verify(x => x.ToString(), Times.Once);
            selfMonitoringDestinationMock.Verify(x => x.ToString(), Times.Once);
            logIdGeneratorMock.Verify(x => x.ToString(), Times.Once);
            logCache1Mock.Verify(x => x.ToString(), Times.Once);
            logCache2Mock.Verify(x => x.ToString(), Times.Once);
            customLogGroupMock.Verify(x => x.ToString(), Times.Once);
            customLogContributorMock.Verify(x => x.ToString(), Times.Once);
            customLogTransformerMock.Verify(x => x.ToString(), Times.Once);
            logLevelProviderMock.Verify(x => x.ToString(), Times.Once);
            logGroup1StatusProviderMock.Verify(x => x.ToString(), Times.Once);
            logGroup2StatusProviderMock.Verify(x => x.ToString(), Times.Once);
            logContributorStatusProviderMock.Verify(x => x.ToString(), Times.Once);
            logTransformerStatusProviderMock.Verify(x => x.ToString(), Times.Once);
        }
        [Test]
        public void Logger_That_Takes_Another_Loggers_As_Log_Destinations()
        {
            // Arrange
            // First log group will be assigned to loggerAsDestination1.
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            // Second log group will be assigned to loggerAsDestination1.
            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            // Third log group will be assigned to loggerAsDestination2.
            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("group3");
            logGroup3Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup3Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            // Forth log group will be assigned to loggerAsDestination2.
            var logGroup4Mock = new Mock<LogGroupBase>();
            logGroup4Mock.SetupGet(x => x.Name).Returns("group4");
            logGroup4Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup4Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            // Will be assigned to the main logger.
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModel(It.IsAny<Log>(), It.IsAny<string>())).Returns(logModel);

            Logger loggerAsDestination1 = LoggerBuilder.Logger()
                                                       .NewLogGroup(logGroup1Mock.Object)
                                                       .NewLogGroup(logGroup2Mock.Object)
                                                       .BuildLogger();
            Logger loggerAsDestination2 = LoggerBuilder.Logger()
                                                 .NewLogGroup(logGroup3Mock.Object)
                                                 .NewLogGroup(logGroup4Mock.Object)
                                                 .BuildLogger();

            // Main logger uses previous two loggers as log destinations for its only log group.
            Logger mainLogger = LoggerBuilder.Logger()
                                             .WithLogCreation(logCreationStrategyMock.Object)
                                             .NewLogGroup()
                                             .ForAllLogs()
                                             .CustomDestination(loggerAsDestination1)
                                             .CustomDestination(loggerAsDestination2)
                                             .BuildLogger();

            // Act
            mainLogger.Log(new Log());

            // Assert
            logGroup1Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup2Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup3Mock.Verify(x => x.Send(logModel), Times.Once);
            logGroup4Mock.Verify(x => x.Send(logModel), Times.Once);

            logGroup1Mock.Verify(x => x.SendAsync(logModel, It.IsAny<CancellationToken>()), Times.Never);
            logGroup2Mock.Verify(x => x.SendAsync(logModel, It.IsAny<CancellationToken>()), Times.Never);
            logGroup3Mock.Verify(x => x.SendAsync(logModel, It.IsAny<CancellationToken>()), Times.Never);
            logGroup4Mock.Verify(x => x.SendAsync(logModel, It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task Async_Logger_That_Takes_Another_Loggers_As_Log_Destinations()
        {
            // Arrange
            // First log group will be assigned to loggerAsDestination1.
            var logGroup1Mock = new Mock<LogGroupBase>();
            logGroup1Mock.SetupGet(x => x.Name).Returns("group1");
            logGroup1Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup1Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            // Second log group will be assigned to loggerAsDestination1.
            var logGroup2Mock = new Mock<LogGroupBase>();
            logGroup2Mock.SetupGet(x => x.Name).Returns("group2");
            logGroup2Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup2Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            // Third log group will be assigned to loggerAsDestination2.
            var logGroup3Mock = new Mock<LogGroupBase>();
            logGroup3Mock.SetupGet(x => x.Name).Returns("group3");
            logGroup3Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup3Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));
            // Forth log group will be assigned to loggerAsDestination2.
            var logGroup4Mock = new Mock<LogGroupBase>();
            logGroup4Mock.SetupGet(x => x.Name).Returns("group4");
            logGroup4Mock.Setup(x => x.IsEligible(It.IsAny<LogModel>())).Returns(true);
            logGroup4Mock.SetupGet(x => x.Status).Returns(Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active));

            // Will be assigned to the main logger.
            var logCreationStrategyMock = new Mock<LogCreationStrategyBase>();
            LogModel logModel = new LogModel();
            logCreationStrategyMock.Setup(x => x.CreateLogModelAsync(It.IsAny<Log>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(logModel);

            Logger loggerAsDestination1 = LoggerBuilder.Logger()
                                                       .NewLogGroup(logGroup1Mock.Object)
                                                       .NewLogGroup(logGroup2Mock.Object)
                                                       .BuildLogger();
            Logger loggerAsDestination2 = LoggerBuilder.Logger()
                                                 .NewLogGroup(logGroup3Mock.Object)
                                                 .NewLogGroup(logGroup4Mock.Object)
                                                 .BuildLogger();

            // Main logger uses previous two loggers as log destinations for its only log group.
            Logger mainLogger = LoggerBuilder.Logger()
                                             .WithLogCreation(logCreationStrategyMock.Object)
                                             .NewLogGroup()
                                             .ForAllLogs()
                                             .CustomDestination(loggerAsDestination1)
                                             .CustomDestination(loggerAsDestination2)
                                             .BuildLogger();

            // Act
            CancellationToken cancellationToken = new CancellationToken();
            await mainLogger.LogAsync(new Log(), cancellationToken);

            // Assert
            logGroup1Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup2Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup3Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);
            logGroup4Mock.Verify(x => x.SendAsync(logModel, cancellationToken), Times.Once);

            logGroup1Mock.Verify(x => x.Send(logModel), Times.Never);
            logGroup2Mock.Verify(x => x.Send(logModel), Times.Never);
            logGroup3Mock.Verify(x => x.Send(logModel), Times.Never);
            logGroup4Mock.Verify(x => x.Send(logModel), Times.Never);
        }
    }
}
