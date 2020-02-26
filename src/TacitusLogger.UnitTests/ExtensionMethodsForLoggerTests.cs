using NUnit.Framework;
using System; 
using System.Linq; 
using TacitusLogger.Destinations;  

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class ExtensionMethodsForLoggerTests
    {
        #region Tests for AddLogDestinations(this Logger self, LogModelFunc<bool> predicate, params ILogDestination[] logDestinations)

        [Test]
        public void AddLogDestinations_WithPredicateAndDestinations_WhenCalledWithSpecificPredicate_NewLogGroupIsCreatedWithAccordingPredicateAndLogDestinations()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination1 = new Moq.Mock<ILogDestination>().Object;
            var logDestination2 = new Moq.Mock<ILogDestination>().Object;
            LogModelFunc<bool> predicate = (l) => false;

            // Act
            logger.AddLogDestinations(predicate, logDestination1, logDestination2);

            // Assert    
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count);
            Assert.AreEqual(predicate, ((LogGroup)logGroups[0]).Rule);
            Assert.AreEqual(2, ((LogGroup)logGroups[0]).LogDestinations.Count);
            Assert.AreEqual(logDestination1, ((LogGroup)logGroups[0]).LogDestinations[0]);
            Assert.AreEqual(logDestination2, ((LogGroup)logGroups[0]).LogDestinations[1]);
        }

        [Test]
        public void AddLogDestinations_WithPredicateAndDestinations_WhenCalledSeveralTimesWithSamePredicate_NewLogGroupIsCreatedEachTimeWithAccordingPredicateAndLogDestinations()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination1 = new Moq.Mock<ILogDestination>().Object;
            var logDestination2 = new Moq.Mock<ILogDestination>().Object;
            LogModelFunc<bool> predicate = (l) => false;

            // Act
            logger.AddLogDestinations(predicate, logDestination1);
            logger.AddLogDestinations(predicate, logDestination2);

            // Assert    
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(2, logGroups.Count());
            Assert.AreEqual(predicate, ((LogGroup)logGroups[0]).Rule);
            Assert.AreEqual(predicate, ((LogGroup)logGroups[1]).Rule);
            Assert.AreEqual(logDestination1, ((LogGroup)logGroups[0]).LogDestinations[0]);
            Assert.AreEqual(logDestination2, ((LogGroup)logGroups[1]).LogDestinations[0]);
        }

        [Test]
        public void AddLogDestinations_WithPredicateAndDestinations_WhenCalledSeveralTimesWithDifferentPredicates_NewLogGroupIsCreatedEachTimeWithAccordingPredicateAndLogDestinations()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination1 = new Moq.Mock<ILogDestination>().Object;
            var logDestination2 = new Moq.Mock<ILogDestination>().Object;
            LogModelFunc<bool> predicate1 = (l) => false;
            LogModelFunc<bool> predicate2 = (l) => true;

            // Act
            logger.AddLogDestinations(predicate1, logDestination1);
            logger.AddLogDestinations(predicate2, logDestination2);

            // Assert    
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(2, logGroups.Count());
            Assert.AreEqual(predicate1, ((LogGroup)logGroups[0]).Rule);
            Assert.AreEqual(predicate2, ((LogGroup)logGroups[1]).Rule);
            Assert.AreEqual(logDestination1, ((LogGroup)logGroups[0]).LogDestinations[0]);
            Assert.AreEqual(logDestination2, ((LogGroup)logGroups[1]).LogDestinations[0]);
        }

        [Test]
        public void AddLogDestinations_WithPredicateAndDestinations_WhenCalledWithNullPredicate_NewLogGroupIsCreatedWithAccordingPredicateAndLogDestinations()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;
            LogModelFunc<bool> predicate = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.AddLogDestinations(predicate, logDestination);
            });
        }

        [Test]
        public void AddLogDestinations_WithPredicateAndDestinations_WhenCalledWithEmptyDestinationsList_ThrowsAnArgumentException()
        {
            // Arrange
            Logger logger = new Logger();
            ILogDestination[] logDestination = new ILogDestination[0];
            LogModelFunc<bool> predicate = (l) => false;

            // Assert
            Assert.Catch<ArgumentException>(() =>
            {
                // Act
                logger.AddLogDestinations(predicate, logDestination);
            });
        }

        [Test]
        public void AddLogDestinations_WithPredicateAndDestinations_WhenCalledWithNullLogDestionation_ThrowsAnArgumentNullException()
        {
            // Arrange
            Logger logger = new Logger();
            ILogDestination logDestination1 = new Moq.Mock<ILogDestination>().Object;
            ILogDestination logDestination2 = null;
            LogModelFunc<bool> predicate = (l) => false;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.AddLogDestinations(predicate, logDestination1, logDestination2);
            });
        }

        #endregion

        #region Tests for AddLogDestinations(this Logger self, params ILogDestination[] logDestinations)

        [Test]
        public void AddLogDestinations_WithDestinations_WhenCalled_NewLogGroupIsCreatedWithAlwaysTruePredicateAndLogDestinations()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddLogDestinations(logDestination);

            // Assert 
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel()));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddLogDestinations_WithDestinations_WhenCalledWithEmptyDestinationsList_ThrowsAnArgumentException()
        {
            // Arrange
            Logger logger = new Logger();

            Assert.Catch<ArgumentException>(() =>
            {
                // Act
                logger.AddLogDestinations();
            });
        }

        [Test]
        public void AddLogDestinations_WithDestinations_WhenCalledWithNullLogDestionation_ThrowsAnArgumentNullException()
        {
            // Arrange
            Logger logger = new Logger();
            ILogDestination logDestination1 = new Moq.Mock<ILogDestination>().Object;
            ILogDestination logDestination2 = null;

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logger.AddLogDestinations(logDestination1, logDestination2);
            });
        }

        #endregion

        #region Tests for AddLogDestinations(this Logger self, string context, params ILogDestination[] logDestinations)

        [TestCase("context1")]
        [TestCase("context2")]
        [TestCase("context3")]
        public void AddLogDestinations_WithContextAndDestinations_WhenCalled_NewLogGroupIsCreatedWithRightPredicate(string context)
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;
            LogModelFunc<bool> predicate = (l) => l.Context == context;

            // Act
            logger.AddLogDestinations(context, logDestination);

            // Assert 
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { Context = context }));
            Assert.IsFalse(((LogGroup)logGroups[0]).Rule(new LogModel() { Context = "wrong context" }));
        }

        #endregion

        #region Tests for AddLogDestinations(this Logger self, LogType logType, params ILogDestination[] logDestinations)

        [TestCase(LogType.Success)]
        [TestCase(LogType.Info)]
        [TestCase(LogType.Event)]
        [TestCase(LogType.Failure)]
        [TestCase(LogType.Error)]
        [TestCase(LogType.Critical)]
        public void AddLogDestinations_WithLogTypeAndLogDestinations_WhenCalledWithSpecificLogType_NewLogGroupIsCreatedWithAccordingPredicateAndDestinationList(LogType logType)
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;


            // Act
            logger.AddLogDestinations(logType, logDestination);

            // Assert    
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = logType }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        #endregion

        #region AddXXXDestinations

        [Test]
        public void AddSuccessDestinations_WhenCalledWithNotNullDest_LogGroupIsCreatedWithRightPredicate()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddSuccessDestinations(logDestination);

            // Assert   
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = LogType.Success }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddInfoDestinations_WhenCalledWithNotNullDest_LogGroupIsCreatedWithRightPredicate()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddInfoDestinations(logDestination);

            // Assert   
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = LogType.Info }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddEventDestinations_WhenCalledWithNotNullDest_LogGroupIsCreatedWithRightPredicate()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddEventDestinations(logDestination);

            // Assert   
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = LogType.Event }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddWarningDestinations_WhenCalledWithNotNullDest_LogGroupIsCreatedWithRightPredicate()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddWarningDestinations(logDestination);

            // Assert   
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = LogType.Warning }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddFailureDestinations_WhenCalledWithNotNullDest_LogGroupIsCreatedWithRightPredicate()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddFailureDestinations(logDestination);

            // Assert   
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = LogType.Failure }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddErrorDestinations_WhenCalledWithNotNullDest_LogGroupIsCreatedWithRightPredicate()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddErrorDestinations(logDestination);

            // Assert   
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = LogType.Error }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddCriticalDestinations_WhenCalledWithNotNullDest_LogGroupIsCreatedWithRightPredicate()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Act
            logger.AddCriticalDestinations(logDestination);

            // Assert   
            var logGroups = logger.LogGroups.ToList();
            Assert.AreEqual(1, logGroups.Count());
            Assert.IsTrue(((LogGroup)logGroups[0]).Rule(new LogModel() { LogType = LogType.Critical }));
            Assert.AreEqual(logDestination, ((LogGroup)logGroups[0]).LogDestinations[0]);
        }

        [Test]
        public void AddXXXDestinations_WhenCalled_ReturnsSelfObject()
        {
            // Arrange
            Logger logger = new Logger();
            var logDestination = new Moq.Mock<ILogDestination>().Object;

            // Assert    
            Assert.AreEqual(logger, logger.AddSuccessDestinations(logDestination));
            Assert.AreEqual(logger, logger.AddInfoDestinations(logDestination));
            Assert.AreEqual(logger, logger.AddEventDestinations(logDestination));
            Assert.AreEqual(logger, logger.AddWarningDestinations(logDestination));
            Assert.AreEqual(logger, logger.AddFailureDestinations(logDestination));
            Assert.AreEqual(logger, logger.AddErrorDestinations(logDestination));
            Assert.AreEqual(logger, logger.AddCriticalDestinations(logDestination));
        }

        #endregion 
    }
}
