using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TacitusLogger.Builders;
using TacitusLogger.Contributors;
using TacitusLogger.Destinations;
using TacitusLogger.Diagnostics;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;

namespace TacitusLogger.IntegrationTests.Builders
{
    [TestFixture]
    public class LogCreationStrategy
    {
        [Test]
        public void LoggerBuilder_When_Setting_Creation_Strategy_Builds_Logger_With_This_Creation_Strategy()
        {
            // Act
            Logger logger = LoggerBuilder.Logger().WithLogCreation(LogCreation.Standard)
                                                  .BuildLogger();
            // Assert
            Assert.IsInstanceOf<StandardLogCreationStrategy>(logger.LogCreationStrategy);
        }
        [Test]
        public void LoggerBuilder_When_Not_Setting_Creation_Strategy_Builds_Logger_With_Default_Creation_Strategy()
        {
            // Act
            Logger logger = LoggerBuilder.Logger().BuildLogger();

            // Assert
            Assert.IsInstanceOf<StandardLogCreationStrategy>(logger.LogCreationStrategy);
        }
        [Test]
        public void LoggerBuilder_When_Setting_Custom_Creation_Strategy_Builds_Logger_With_This_Creation_Strategy()
        {
            // Arrange
            var creationStrategy = new Mock<LogCreationStrategyBase>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithLogCreation(creationStrategy)
                                                  .BuildLogger();

            // Assert
            Assert.AreEqual(creationStrategy, logger.LogCreationStrategy);
        }
        [Test]
        public void LoggerBuilder_When_Setting_Custom_Creation_Strategy_Builds_Logger_With_This_Creation_Strategy_With_Its_Dependants()
        {
            // Arrange
            var creationStrategy = new Mock<LogCreationStrategyBase>().Object;
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;
            var contributors1 = new Mock<LogContributorBase>("contributor1").Object;
            var contributors2 = new Mock<LogContributorBase>("contributor2").Object;
            var exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithLogCreation(creationStrategy)
                                                  .WithExceptionHandling(exceptionHandlingStrategy)
                                                  .WithLogIdGenerator(logIdGenerator)
                                                  .Contributors().Custom(contributors1)
                                                                    .Custom(contributors2)
                                                                    .BuildContributors()
                                                  .BuildLogger();

            // Assert
            Assert.AreEqual(creationStrategy, logger.LogCreationStrategy);
            // Log contributors
            var logContributors = logger.LogCreationStrategy.LogContributors.ToList();
            Assert.AreEqual(2, logContributors.Count);
            Assert.Contains(contributors1, logContributors);
            Assert.Contains(contributors2, logContributors);
            // Log id generator
            Assert.AreEqual(logIdGenerator, logger.LogCreationStrategy.LogIdGenerator);
            // Exception handling strategy
            Assert.AreEqual(exceptionHandlingStrategy, logger.LogCreationStrategy.ExceptionHandlingStrategy);
        }

        [Test]
        public void LoggerBuilder_When_Setting_Custom_Creation_Strategy_Then_Strategy_Is_Provided_With_Dependencies()
        {
            // Arrange
            var creationStrategy = new Mock<LogCreationStrategyBase>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithLogCreation(creationStrategy)
                                                  .BuildLogger();
            // Assert
            Assert.AreEqual(logger.LogContributors, logger.LogCreationStrategy.LogContributors);
            Assert.AreEqual(logger.ExceptionHandlingStrategy, logger.LogCreationStrategy.ExceptionHandlingStrategy);
            Assert.AreEqual(logger.LogIdGenerator, logger.LogCreationStrategy.LogIdGenerator);
        }

        [Test]
        public void LoggerBuilder_When_Setting_Custom_Creation_Strategy___()
        {
            // Arrange
            var creationStrategy = new Mock<LogCreationStrategyBase>().Object;
            var contributors1 = new Mock<LogContributorBase>("contributor1").Object;
            var contributors2 = new Mock<LogContributorBase>("contributor2").Object;
            var exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;
            DiagnosticsManagerBase diagnosticsManager = new Mock<DiagnosticsManagerBase>().Object;
            Logger logger = LoggerBuilder.Logger().WithLogCreation(creationStrategy)
                                                  .BuildLogger();

            // Act
            logger.ResetExceptionHandlingStrategy(exceptionHandlingStrategy);
            logger.AddLogContributor(contributors1);
            logger.AddLogContributor(contributors2);
            exceptionHandlingStrategy.SetDiagnosticsManager(diagnosticsManager);

            // Assert
            Assert.AreEqual(creationStrategy, logger.LogCreationStrategy);
            // Log contributors
            var logContributors = logger.LogCreationStrategy.LogContributors.ToList();
            Assert.AreEqual(2, logContributors.Count);
            Assert.Contains(contributors1, logContributors);
            Assert.Contains(contributors2, logContributors);
            // Exception handling strategy
            Assert.AreEqual(exceptionHandlingStrategy, logger.LogCreationStrategy.ExceptionHandlingStrategy);
            // Diagnostics manager of exception handling strategy.
            Assert.AreEqual(diagnosticsManager, logger.LogCreationStrategy.ExceptionHandlingStrategy.DiagnosticsManager);
        }
    }
}
