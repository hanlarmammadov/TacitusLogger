using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Contributors;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;
using TacitusLogger.Strategies.LogCreation;

namespace TacitusLogger.UnitTests.StrategyTests.LogCreation
{
    [TestFixture]
    public class LogCreationStrategyBaseTests
    {
        public class TestLogCreationStrategy : LogCreationStrategyBase
        {
            public override LogModel CreateLogModel(Log log, string source)
            {
                throw new NotImplementedException();
            }

            public override Task<LogModel> CreateLogModelAsync(Log log, string source, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        #region Ctor tests

        [Test]
        public void Ctor_When_Called_All_Dependencies_Are_Null()
        {
            // Act
            TestLogCreationStrategy testLogCreationStrategy = new TestLogCreationStrategy();

            // Assert
            Assert.AreEqual(null, testLogCreationStrategy.LogIdGenerator);
            Assert.AreEqual(null, testLogCreationStrategy.LogContributors);
            Assert.AreEqual(null, testLogCreationStrategy.ExceptionHandlingStrategy);
        }

        #endregion

        #region Tests for InitStrategy method

        [Test]
        public void InitStrategy_When_Called_Sets_All_Dependencies()
        {
            // Arrange
            TestLogCreationStrategy testLogCreationStrategy = new TestLogCreationStrategy();
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            // Act
            testLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Assert
            Assert.AreEqual(logIdGenerator, testLogCreationStrategy.LogIdGenerator);
            Assert.AreEqual(logContributors, testLogCreationStrategy.LogContributors);
            Assert.AreEqual(exceptionHandlingStrategy, testLogCreationStrategy.ExceptionHandlingStrategy);
        }
        [Test]
        public void InitStrategy_When_Called_With_Null_Log_Id_Generator_Throws_ArgumentNullException()
        {
            // Arrange
            TestLogCreationStrategy testLogCreationStrategy = new TestLogCreationStrategy();
            ILogIdGenerator logIdGenerator = null;
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                testLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            });
            Assert.AreEqual("logIdGenerator", ex.ParamName);
        }
        [Test]
        public void InitStrategy_When_Called_With_Null_Log_Contributors_Throws_ArgumentNullException()
        {
            // Arrange
            TestLogCreationStrategy testLogCreationStrategy = new TestLogCreationStrategy();
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = null;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = new Mock<ExceptionHandlingStrategyBase>().Object;

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                testLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);
            });
            Assert.AreEqual("logContributors", ex.ParamName);
        }
        [Test]
        public void InitStrategy_When_Called_With_Null_Exception_Handling_Strategy_Does_Not_Throws()
        {
            // Arrange
            TestLogCreationStrategy testLogCreationStrategy = new TestLogCreationStrategy();
            ILogIdGenerator logIdGenerator = new Mock<ILogIdGenerator>().Object;
            IList<LogContributorBase> logContributors = new Mock<IList<LogContributorBase>>().Object;
            ExceptionHandlingStrategyBase exceptionHandlingStrategy = null;

            // Act
            testLogCreationStrategy.InitStrategy(logIdGenerator, logContributors, exceptionHandlingStrategy);

            // Assert 
            Assert.IsNull(testLogCreationStrategy.ExceptionHandlingStrategy);
        }

        #endregion
    }
}
