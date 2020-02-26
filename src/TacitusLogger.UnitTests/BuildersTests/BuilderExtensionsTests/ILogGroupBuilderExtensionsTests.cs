using Moq;
using NUnit.Framework;
using TacitusLogger.Builders;
using TacitusLogger.Caching;
using TacitusLogger.Strategies.DestinationFeeding;

namespace TacitusLogger.UnitTests.BuildersTests.BuilderExtensionsTests
{
    [TestFixture]
    public class ILogGroupBuilderExtensionsTests
    {
        #region Tests for ForAllLogs extension method

        [Test]
        public void ForAllLogs_Taking_ILogGroupBuilder_When_Called_Calls_ForRule_Method_Of_ILogGroupBuilder()
        {
            // Arrange  
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();

            // Act 
            ILogGroupBuilderExtensions.ForAllLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.IsNotNull<LogModelFunc<bool>>()), Times.Once);
        }

        [Test]
        public void ForAllLogs_Taking_ILogGroupBuilder_When_Called_Calls_ForRule_And_Returns_Its_Result()
        {
            // Arrange  
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            logGroupBuilderMock.Setup(x => x.ForRule(It.IsNotNull<LogModelFunc<bool>>())).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForAllLogs(logGroupBuilderMock.Object);

            // Assert
            Assert.NotNull(returned);
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForLogType extension method

        [TestCase(LogType.Success)]
        [TestCase(LogType.Info)]
        [TestCase(LogType.Event)]
        [TestCase(LogType.Warning)]
        [TestCase(LogType.Failure)]
        [TestCase(LogType.Error)]
        [TestCase(LogType.Critical)]
        public void ForLogType_Taking_ILogGroupBuilder_And_LogType_When_Called_Calls_ForRule_Method_Of_ILogGroupBuilder(LogType logType)
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = logType };

            // Act 
            ILogGroupBuilderExtensions.ForLogType(logGroupBuilderMock.Object, logType);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        public void ForLogType_Taking_ILogGroupBuilder_And_LogType_When_Called_Creates_And_Returns_LogGroupDestinationsBuilder_With_BuildCallback_Set_On_it()
        {
            // Arrange 
            var logGroupBuilder = new Mock<ILogGroupBuilder>().Object;

            // Act 
            LogGroupDestinationsBuilder returned = (LogGroupDestinationsBuilder)ILogGroupBuilderExtensions.ForLogType(logGroupBuilder, LogType.Info);

            // Assert        
            Assert.NotNull(returned);
            Assert.NotNull(returned.BuildCallback);
        }

        #endregion

        #region Tests for ForSuccessLogs extension method

        [Test]
        public void ForSuccessLogs_Taking_ILogGroupBuilder_When_Called_Calls_ForRule_Method_Of_ILogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = LogType.Success };

            // Act 
            ILogGroupBuilderExtensions.ForSuccessLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForSuccessLogs_Taking_ILogGroupBuilder_When_Called_Calls_ForRule_With_Success_LogType_Predicate_And_Returns_Its_Result()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            LogModel logModel = new LogModel() { LogType = LogType.Success };
            logGroupBuilderMock.Setup(x => x.ForRule(It.Is<LogModelFunc<bool>>(p => p(logModel)))).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForSuccessLogs(logGroupBuilderMock.Object);

            // Assert 
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForInfoLogs extension method

        [Test]
        public void ForInfoLogs_Taking_ILogGroupBuilder_When_Called_Calls_ForRule_Method_Of_ILogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = LogType.Info };

            // Act 
            ILogGroupBuilderExtensions.ForInfoLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForInfoLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeWithInfoLogTypePredicateAndReturnsItsResult()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            LogModel logModel = new LogModel() { LogType = LogType.Info };
            logGroupBuilderMock.Setup(x => x.ForRule(It.Is<LogModelFunc<bool>>(p => p(logModel)))).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForInfoLogs(logGroupBuilderMock.Object);

            // Assert 
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForEventLogs extension method

        [Test]
        public void ForEventLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeMethodOfLogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = LogType.Event };

            // Act 
            ILogGroupBuilderExtensions.ForEventLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForEventLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeWithEventLogTypePredicateAndReturnsItsResult()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            LogModel logModel = new LogModel() { LogType = LogType.Event };
            logGroupBuilderMock.Setup(x => x.ForRule(It.Is<LogModelFunc<bool>>(p => p(logModel)))).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForEventLogs(logGroupBuilderMock.Object);

            // Assert 
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForWarningLogs extension method

        [Test]
        public void ForWarningLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeMethodOfLogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = LogType.Warning };

            // Act 
            ILogGroupBuilderExtensions.ForWarningLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForWarningLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeWithWarningLogTypePredicateAndReturnsItsResult()
        {
            // Arrange  
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            LogModel logModel = new LogModel() { LogType = LogType.Warning };
            logGroupBuilderMock.Setup(x => x.ForRule(It.Is<LogModelFunc<bool>>(p => p(logModel)))).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForWarningLogs(logGroupBuilderMock.Object);

            // Assert 
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForFailureLogs extension method

        [Test]
        public void ForFailureLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeMethodOfLogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = LogType.Failure };

            // Act 
            ILogGroupBuilderExtensions.ForFailureLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForFailureLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeWithFailureLogTypePredicateAndReturnsItsResult()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            LogModel logModel = new LogModel() { LogType = LogType.Failure };
            logGroupBuilderMock.Setup(x => x.ForRule(It.Is<LogModelFunc<bool>>(p => p(logModel)))).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForFailureLogs(logGroupBuilderMock.Object);

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForErrorLogs extension method

        [Test]
        public void ForErrorLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeMethodOfLogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = LogType.Error };

            // Act 
            ILogGroupBuilderExtensions.ForErrorLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForErrorLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeWithErrorLogTypePredicateAndReturnsItsResult()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            LogModel logModel = new LogModel() { LogType = LogType.Error };
            logGroupBuilderMock.Setup(x => x.ForRule(It.Is<LogModelFunc<bool>>(p => p(logModel)))).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForErrorLogs(logGroupBuilderMock.Object);

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForCriticalLogs extension method

        [Test]
        public void ForCriticalLogs_WithLogGroupBuilderParam_WhenCalled_CallsForLogTypeMethodOfLogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { LogType = LogType.Critical };

            // Act 
            ILogGroupBuilderExtensions.ForCriticalLogs(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForCriticalLogs_WithLogGroupBuilderParam_WhenCalled_CallsForForRuleMethodWithCriticalLogPredicateAndReturnsItsResult()
        {
            // Arrange              
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            LogModel logModel = new LogModel() { LogType = LogType.Critical };
            logGroupBuilderMock.Setup(x => x.ForRule(It.Is<LogModelFunc<bool>>(p => p(logModel)))).Returns(logGroupDestinationsBuilder);

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForCriticalLogs(logGroupBuilderMock.Object);

            // Assert 
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for ForLogContext extension method

        [Test]
        public void ForLogContext_WithLogGroupBuilderAndContextParam_WhenCalled_CallsForRuleMethodOfLogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            LogModel logModel = new LogModel() { Context = "context" };

            // Act 
            ILogGroupBuilderExtensions.ForLogContext(logGroupBuilderMock.Object, logModel.Context);

            // Assert
            logGroupBuilderMock.Verify(x => x.ForRule(It.Is<LogModelFunc<bool>>(d => d(logModel))), Times.Once);
        }

        [Test]
        public void ForLogContext_WithLogGroupBuilderAndContextParam_WhenCalled_ReturnsLogGroupDestinationsBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;
            logGroupBuilderMock.Setup(x => x.ForRule(It.IsNotNull<LogModelFunc<bool>>())).Returns(logGroupDestinationsBuilder);
            string context = "context";

            // Act 
            ILogGroupDestinationsBuilder returned = ILogGroupBuilderExtensions.ForLogContext(logGroupBuilderMock.Object, context);

            // Assert
            Assert.NotNull(returned);
            Assert.AreEqual(logGroupDestinationsBuilder, returned);
        }

        #endregion

        #region Tests for WithCaching extension method

        [TestCase(true)]
        [TestCase(false)]
        public void WithCaching_Taking_LogGroupBuilder_CacheSize_CacheTime_And_IsActive_Flag_When_Called_Calls_WithCaching_Method_Of_ILogGroupBuilder(bool isActive)
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            int cacheSize = 10;
            int cacheTime = 3000;

            // Act 
            ILogGroupBuilderExtensions.WithCaching(logGroupBuilderMock.Object, cacheSize, cacheTime, isActive);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithCaching(It.Is<InMemoryLogCache>(c => c.CacheSize == cacheSize && c.CacheTime == cacheTime), isActive));
        }
        [Test]
        public void WithCaching_Taking_LogGroupBuilder_CacheSize_CacheTime_And_IsActive_Flag_When_Called_Without_IsActive_Flag_Sets_To_True()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            int cacheSize = 10;
            int cacheTime = 3000;

            // Act 
            ILogGroupBuilderExtensions.WithCaching(logGroupBuilderMock.Object, cacheSize, cacheTime);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithCaching(It.Is<InMemoryLogCache>(c => c.CacheSize == cacheSize && c.CacheTime == cacheTime), true));
        }
        [Test]
        public void WithCaching_Taking_LogGroupBuilder_CacheSize_CacheTime_And_IsActive_Flag_When_Called_Without_CachTime_And_IsActive_Flag_Set_To_Defaults()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            int cacheSize = 10;

            // Act 
            ILogGroupBuilderExtensions.WithCaching(logGroupBuilderMock.Object, cacheSize);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithCaching(It.Is<InMemoryLogCache>(c => c.CacheSize == cacheSize && c.CacheTime == -1), true));
        }
        [Test]
        public void WithCaching_Taking_LogGroupBuilder_CacheSize_CacheTime_And_IsActive_Flag_When_Called_Returns_Result_Of_WithCaching_Method_Of_ILogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupBuilderReturned = new Mock<ILogGroupBuilder>().Object;
            logGroupBuilderMock.Setup(x => x.WithCaching(It.IsAny<InMemoryLogCache>(), It.IsAny<bool>())).Returns(logGroupBuilderReturned);

            // Act 
            var result = ILogGroupBuilderExtensions.WithCaching(logGroupBuilderMock.Object, 10);

            // Assert
            Assert.AreEqual(result, logGroupBuilderReturned);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void WithCaching_Taking_LogGroupBuilder_LogCache_And_IsActive_Flag_When_Called_Calls_WithCaching_Method_Of_ILogGroupBuilder(bool isActive)
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logCache = new Mock<ILogCache>().Object;

            // Act 
            ILogGroupBuilderExtensions.WithCaching(logGroupBuilderMock.Object, logCache, isActive);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithCaching(logCache, isActive));
        }
        [Test]
        public void WithCaching_Taking_LogGroupBuilder_LogCache_And_IsActive_Flag_When_Called_Without_IsActive_Flag_Sets_To_True()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logCache = new Mock<ILogCache>().Object;

            // Act 
            ILogGroupBuilderExtensions.WithCaching(logGroupBuilderMock.Object, logCache);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithCaching(logCache, true));
        }
        [Test]
        public void WithCaching_Taking_LogGroupBuilder_LogCache_And_IsActive_Flag_When_Called_Returns_Result_Of_WithCaching_Method_Of_ILogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            var logGroupBuilderReturned = new Mock<ILogGroupBuilder>().Object;
            logGroupBuilderMock.Setup(x => x.WithCaching(It.IsAny<ILogCache>(), It.IsAny<bool>())).Returns(logGroupBuilderReturned);

            // Act 
            var result = ILogGroupBuilderExtensions.WithCaching(logGroupBuilderMock.Object, new Mock<ILogCache>().Object);

            // Assert
            Assert.AreEqual(result, logGroupBuilderReturned);
        }
        
        #endregion

        #region Tests for WithDestinationFeeding extension method

        [Test]
        public void WithDestinationFeeding_Taking_LogGroupBuilder_And_DestinationFeedingStrategyType_When_Called_Calls_WithDestinationFeedingStrategy_Method_Of_ILogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();

            // Act 
            ILogGroupBuilderExtensions.WithDestinationFeeding(logGroupBuilderMock.Object, DestinationFeeding.Greedy);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithDestinationFeeding(It.IsNotNull<GreedyDestinationFeedingStrategy>()));
        }

        [Test]
        public void WithDestinationFeeding_Taking_LogGroupBuilder_And_DestinationFeedingStrategyType_When_Called_Returns_ILogGroupBuilder_WithDestinationFeedingStrategy_Method_Result()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            logGroupBuilderMock.Setup(x => x.WithDestinationFeeding(It.IsAny<DestinationFeedingStrategyBase>())).Returns(logGroupBuilderMock.Object);

            // Act 
            var result = ILogGroupBuilderExtensions.WithDestinationFeeding(logGroupBuilderMock.Object, DestinationFeeding.Greedy);

            // Assert
            Assert.AreEqual(result, (logGroupBuilderMock.Object));
        }

        #endregion

        #region Tests for WithGreedyDestinationFeeding extension method

        [Test]
        public void WithGreedyDestinationFeeding_Taking_LogGroupBuilder_When_Called_Calls_WithDestinationFeedingStrategy_Method_Of_ILogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();

            // Act 
            ILogGroupBuilderExtensions.WithGreedyDestinationFeeding(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithDestinationFeeding(It.IsNotNull<GreedyDestinationFeedingStrategy>()));
        }

        [Test]
        public void WithGreedyDestinationFeeding_Taking_LogGroupBuilder_When_Called_Returns_ILogGroupBuilder_WithDestinationFeedingStrategy_Method_Result()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            logGroupBuilderMock.Setup(x => x.WithDestinationFeeding(It.IsAny<GreedyDestinationFeedingStrategy>())).Returns(logGroupBuilderMock.Object);

            // Act 
            var result = ILogGroupBuilderExtensions.WithGreedyDestinationFeeding(logGroupBuilderMock.Object);

            // Assert
            Assert.AreEqual(result, (logGroupBuilderMock.Object));
        }

        #endregion

        #region Tests for WithGreedyDestinationFeeding extension method

        [Test]
        public void WithFirstSuccessDestinationFeeding_Taking_LogGroupBuilder_When_Called_Calls_WithDestinationFeedingStrategy_Method_Of_ILogGroupBuilder()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();

            // Act 
            ILogGroupBuilderExtensions.WithFirstSuccessDestinationFeeding(logGroupBuilderMock.Object);

            // Assert
            logGroupBuilderMock.Verify(x => x.WithDestinationFeeding(It.IsNotNull<FirstSuccessDestinationFeedingStrategy>()));
        }

        [Test]
        public void WithFirstSuccessDestinationFeeding_Taking_LogGroupBuilder_When_Called_Returns_ILogGroupBuilder_WithDestinationFeedingStrategy_Method_Result()
        {
            // Arrange 
            var logGroupBuilderMock = new Mock<ILogGroupBuilder>();
            logGroupBuilderMock.Setup(x => x.WithDestinationFeeding(It.IsAny<FirstSuccessDestinationFeedingStrategy>())).Returns(logGroupBuilderMock.Object);

            // Act 
            var result = ILogGroupBuilderExtensions.WithFirstSuccessDestinationFeeding(logGroupBuilderMock.Object);

            // Assert
            Assert.AreEqual(result, (logGroupBuilderMock.Object));
        }

        #endregion
    }
}
