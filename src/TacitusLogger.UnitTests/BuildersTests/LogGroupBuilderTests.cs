using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TacitusLogger.Builders;
using TacitusLogger.Strategies.DestinationFeeding;
using TacitusLogger.Destinations;
using TacitusLogger.Caching;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class LogGroupBuilderTests
    {
        #region Ctor Tests 

        [Test]
        public void Ctor_When_Called_Sets_Name_And_Build_Callback()
        {
            // Arrange
            string name = "name1";
            Func<LogGroupBase, ILoggerBuilder> buildCallback = logGroupBase => null;

            // Act 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder(name, buildCallback);

            // Assert  
            Assert.AreEqual(name, logGroupBuilder.Name);
            Assert.AreEqual(buildCallback, logGroupBuilder.BuildCallback);
        }
        [Test]
        public void Ctor_When_Called_Rule_Is_Null()
        {
            // Act 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);

            // Assert  
            Assert.IsNull(logGroupBuilder.Rule);
        }
        [Test]
        public void Ctor_When_Called_Status_Is_Set_To_Active()
        {
            // Act 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);

            // Assert  
            Assert.AreEqual(LogGroupStatus.Active, logGroupBuilder.LogGroupStatus);
        }
        [Test]
        public void Ctor_When_Created_LogCache_Is_Null()
        {
            // Act 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);

            // Assert  
            Assert.IsNull(logGroupBuilder.LogCache);
        }
        [Test]
        public void Ctor_When_Created_Destination_Feeding_Strategy_Is_Null()
        {
            // Act 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);

            // Assert   
            Assert.IsNull(logGroupBuilder.DestinationFeedingStrategy);
        }

        #endregion

        #region Tests for ForRule method

        [Test]
        public void ForRule_When_Called_Sets_Rule_To_Provided_Predicate()
        {
            // Arrange
            Func<LogGroupBase, ILoggerBuilder> buildCallback = lg => null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("logGroup", buildCallback);
            LogModelFunc<bool> predicate = x => x.Context == "";

            // Act
            logGroupBuilder.ForRule(predicate);

            // Assert  
            Assert.AreEqual(predicate, logGroupBuilder.Rule);
        }
        [Test]
        public void ForRule_When_Called_With_Null_Predicate_Throws_ArgumentNullException()
        {
            // Arrange
            Func<LogGroupBase, ILoggerBuilder> buildCallback = lg => null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("logGroup", buildCallback);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logGroupBuilder.ForRule(null);
            });
        }
        [Test]
        public void ForRule_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            Func<LogGroupBase, ILoggerBuilder> buildCallback = lg => null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("logGroup", buildCallback);
            // Was already set here
            logGroupBuilder.ForRule(x => x.Context == "1");

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                logGroupBuilder.ForRule(x => x.Context == "2");
            });
        }

        #endregion

        #region Tests for SetStatus method

        [Test]
        public void SetStatus_When_Called_Sets_Log_Group_Status()
        {
            // Arrange
            Func<LogGroupBase, ILoggerBuilder> buildCallback = lg => null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("logGroup", buildCallback);

            // Act
            logGroupBuilder.SetStatus(LogGroupStatus.Inactive);
            Assert.AreEqual(LogGroupStatus.Inactive, logGroupBuilder.LogGroupStatus);
            logGroupBuilder.SetStatus(LogGroupStatus.Active);
            Assert.AreEqual(LogGroupStatus.Active, logGroupBuilder.LogGroupStatus);
        }

        #endregion

        #region Tests for WithCaching method

        [TestCase(true)]
        [TestCase(false)]
        public void WithCaching_Taking_ILogCache_And_IsActive_Flag_When_Called_Sets_Custom_Log_Data_Cache_And_Flag(bool isActive)
        {
            // Arrange
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);
            ILogCache logCache = new Mock<ILogCache>().Object;

            // Act
            logGroupBuilder.WithCaching(logCache, isActive);

            // Assert
            Assert.AreEqual(logCache, logGroupBuilder.LogCache);
            Assert.AreEqual(isActive, logGroupBuilder.CachingIsActive);
        }
        [Test]
        public void WithCaching_Taking_ILogCache_And_IsActive_Flag_When_Called_Without_Is_Active_Flag_Sets_To_True()
        {
            // Arrange
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);
            ILogCache logCache = new Mock<ILogCache>().Object;

            // Act
            logGroupBuilder.WithCaching(logCache);

            // Assert
            Assert.AreEqual(logCache, logGroupBuilder.LogCache);
            Assert.AreEqual(true, logGroupBuilder.CachingIsActive);
        }
        [Test]
        public void WithCaching_Taking_ILogCache_And_IsActive_Flag_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);
            // Was already been set here.
            logGroupBuilder.WithCaching(new Mock<ILogCache>().Object, true);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                logGroupBuilder.WithCaching(new Mock<ILogCache>().Object, true);
            });
        }
        [Test]
        public void WithCaching_Taking_ILogCache_And_IsActive_Flag_When_Called_With_Null_ILogCache_Throws_ArgumentNullException()
        {
            // Arrange
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name1", logGroupBase => null);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logGroupBuilder.WithCaching(null as ILogCache, true);
            });
        }

        #endregion

        #region Tests for WithDestinationFeeding method

        [Test]
        public void WithDestinationFeeding_Taking_DestinationFeedingStrategyBase_When_Called_Sets_DestinationFeedingStrategy()
        {
            // Arrange 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("logGroup", lg => null);
            DestinationFeedingStrategyBase destinationFeedingStrategy = new Mock<DestinationFeedingStrategyBase>().Object;

            // Act
            logGroupBuilder.WithDestinationFeeding(destinationFeedingStrategy);

            // Assert
            Assert.AreEqual(destinationFeedingStrategy, logGroupBuilder.DestinationFeedingStrategy);
        }
        [Test]
        public void WithDestinationFeeding_Taking_DestinationFeedingStrategyBase__When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("logGroup", lg => null);
            // Already set here.
            logGroupBuilder.WithDestinationFeeding(new Mock<DestinationFeedingStrategyBase>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                logGroupBuilder.WithDestinationFeeding(new Mock<DestinationFeedingStrategyBase>().Object);
            });
        }
        [Test]
        public void WithDestinationFeeding_Taking_DestinationFeedingStrategyBase_When_Called_Returns_The_LogGroupBuilder()
        {
            // Arrange 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("logGroup", lg => null);

            // Act
            var returnedLogGroupBuilder = logGroupBuilder.WithDestinationFeeding(new Mock<DestinationFeedingStrategyBase>().Object);

            // Assert
            Assert.AreEqual(logGroupBuilder, returnedLogGroupBuilder);
        }

        #endregion

        #region Tests for CreateLogGroup method

        [Test]
        public void CreateLogGroup_Taking_Log_Destinations_When_Called_Calls_BuildCallback_Passing_Built_LogGroup_To_It()
        {
            // Arrange 
            List<ILogDestination> logDestinations = new List<ILogDestination>() { };
            LogGroup logGroupPassedToBuildCallback = null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name", lg =>
            {
                logGroupPassedToBuildCallback = (LogGroup)lg;
                return null;
            });
            LogModelFunc<bool> rule = (ld) => ld.Context == "";

            // Act
            logGroupBuilder.ForRule(rule);
            logGroupBuilder.CreateLogGroup(logDestinations);

            // Assert  
            Assert.NotNull(logGroupPassedToBuildCallback);
            Assert.IsInstanceOf<LogGroup>(logGroupPassedToBuildCallback);
            Assert.AreEqual(logDestinations, logGroupPassedToBuildCallback.LogDestinations);
            Assert.AreEqual(LogGroupStatus.Active, logGroupPassedToBuildCallback.Status.Value);
            Assert.AreEqual("name", logGroupPassedToBuildCallback.Name);
            Assert.AreEqual(rule, logGroupPassedToBuildCallback.Rule);
            Assert.IsNull(logGroupPassedToBuildCallback.LogCache);
            Assert.IsFalse(logGroupPassedToBuildCallback.CachingIsActive);
        }
        [TestCase(true)]
        [TestCase(false)]
        public void CreateLogGroup_Taking_Log_Destinations_When_Called_Given_That_Cache_Is_Set(bool cacheIsActive)
        {
            // Arrange   
            LogGroup logGroupPassedToBuildCallback = null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name", lg =>
            {
                logGroupPassedToBuildCallback = (LogGroup)lg;
                return null;
            });
            ILogCache logCache = new Mock<ILogCache>().Object;
            ILogGroupBuilderExtensions.WithCaching(logGroupBuilder, logCache, cacheIsActive);

            // Act 
            logGroupBuilder.CreateLogGroup(new List<ILogDestination>() { });

            // Assert   
            Assert.AreEqual(logCache, logGroupPassedToBuildCallback.LogCache);
            Assert.AreEqual(cacheIsActive, logGroupPassedToBuildCallback.CachingIsActive);
        }
        [Test]
        public void CreateLogGroup_Taking_Log_Destinations_When_Called_Calls_BuildCallback_And_Returns_Its_Result()
        {
            // Arrange
            var loggerBuilder = new Mock<ILoggerBuilder>().Object;
            List<ILogDestination> logDestinations = new List<ILogDestination>() { };
            //Func<ILogGroup, ILoggerBuilder> _buildCallback = lg => loggerBuilder; 
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name", lg => loggerBuilder);

            // Act
            ILoggerBuilder loggerBuilderReturned = logGroupBuilder.CreateLogGroup(logDestinations);

            // Assert  
            Assert.AreEqual(loggerBuilder, loggerBuilderReturned);
        }
        [Test]
        public void CreateLogGroup_Taking_Log_Destinations_When_Called_Having_That_Rule_Was_Not_Set_Provides_Builded_LogGroup_With_Default_Rule()
        {

            // Arrange 
            List<ILogDestination> logDestinations = new List<ILogDestination>() { };
            LogGroup logGroupPassedToBuildCallback = null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name", lg =>
            {
                logGroupPassedToBuildCallback = (LogGroup)lg;
                return null;
            });
            LogModelFunc<bool> rule = (ld) => ld.Context == "";

            // Act
            logGroupBuilder.CreateLogGroup(logDestinations);

            // Assert  
            Assert.NotNull(logGroupPassedToBuildCallback.Rule);
        }
        [Test]
        public void CreateLogGroup_Taking_Log_Destinations_When_Called_With_Null_Destinations_List_Provides_Builded_LogGroup_With_Empty_Destinations_List()
        {
            // Arrange 
            List<ILogDestination> logDestinations = new List<ILogDestination>() { };
            LogGroup logGroupPassedToBuildCallback = null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name", lg =>
            {
                logGroupPassedToBuildCallback = (LogGroup)lg;
                return null;
            });
            LogModelFunc<bool> rule = (ld) => ld.Context == "";

            // Act
            logGroupBuilder.CreateLogGroup(null as List<ILogDestination>);

            // Assert  
            Assert.AreEqual(0, logGroupPassedToBuildCallback.LogDestinations.Count);
        }
        [Test]
        public void CreateLogGroup_Taking_Log_Destinations_When_Called_Having_That_DestinationFeedingStrategy_Was_Not_Set_Builded_LogGroup_Got_Default_One()
        {

            // Arrange  
            LogGroup logGroupPassedToBuildCallback = null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name", lg =>
            {
                logGroupPassedToBuildCallback = (LogGroup)lg;
                return null;
            });

            // Act
            logGroupBuilder.CreateLogGroup(new List<ILogDestination>() { });

            // Assert  
            Assert.IsNull(logGroupBuilder.DestinationFeedingStrategy);
            Assert.IsInstanceOf<GreedyDestinationFeedingStrategy>(logGroupPassedToBuildCallback.DestinationFeedingStrategy);
        }
        [Test]
        public void CreateLogGroup_Taking_Log_Destinations_When_Called_Having_That_DestinationFeedingStrategy_Was_Set_Builded_LogGroup_Gets_That_Strategy()
        {

            // Arrange  
            LogGroup logGroupPassedToBuildCallback = null;
            LogGroupBuilder logGroupBuilder = new LogGroupBuilder("name", lg =>
            {
                logGroupPassedToBuildCallback = (LogGroup)lg;
                return null;
            });
            DestinationFeedingStrategyBase destinationFeedingStrategy = new Mock<DestinationFeedingStrategyBase>().Object;
            logGroupBuilder.WithDestinationFeeding(destinationFeedingStrategy);

            // Act
            logGroupBuilder.CreateLogGroup(new List<ILogDestination>() { });

            // Assert   
            Assert.AreEqual(destinationFeedingStrategy, logGroupPassedToBuildCallback.DestinationFeedingStrategy);
        }

        #endregion 
    }
}
