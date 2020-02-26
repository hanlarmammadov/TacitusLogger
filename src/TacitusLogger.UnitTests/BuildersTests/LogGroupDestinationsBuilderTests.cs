using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TacitusLogger.Builders;
using TacitusLogger.Destinations;
using TacitusLogger.LogIdGenerators;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class LogGroupDestinationsBuilderTests
    {

        #region Tests for Ctor 

        [Test]
        public void Ctor_Taking_BuildCallback_When_Called_Sets_BuildCallback_To_Provided_One()
        {
            // Arrange
            Func<List<ILogDestination>, ILoggerBuilder> buildCallback = ld => null;

            // Act 
            LogGroupDestinationsBuilder logGroupDestinationsBuilder = new LogGroupDestinationsBuilder(buildCallback);

            // Assert  
            Assert.AreEqual(buildCallback, logGroupDestinationsBuilder.BuildCallback);
        }

        [Test]
        public void Ctor_Taking_BuildCallback_When_Called_Initializes_Internal_Log_Destinations_Collection()
        {
            // Arrange
            Func<List<ILogDestination>, ILoggerBuilder> buildCallback = ld => null;

            // Act 
            LogGroupDestinationsBuilder logGroupDestinationsBuilder = new LogGroupDestinationsBuilder(buildCallback);

            // Assert  
            Assert.NotNull(logGroupDestinationsBuilder.LogDestinations);
            Assert.AreEqual(0, logGroupDestinationsBuilder.LogDestinations.Count);
        }

        #endregion

        #region Tests for CustomDestination method

        [Test]
        public void CustomDestination_Taking_LogDestination_When_Called_Returns_LogGroupDestinationsBuilder()
        {
            // Arrange 
            LogGroupDestinationsBuilder logGroupDestinationsBuilder = new LogGroupDestinationsBuilder(ld => null);
            var logDestination = new Mock<ILogDestination>().Object;

            // Act 
            ILogGroupDestinationsBuilder logGroupDestinationsBuilderReturned   = logGroupDestinationsBuilder.CustomDestination(logDestination);

            // Assert  
            Assert.AreEqual(logGroupDestinationsBuilder, logGroupDestinationsBuilderReturned);
        }

        [Test]
        public void CustomDestination_Taking_LogDestination_When_Called_Adds_LogDestination_To_LogGroupDestinationsBuilder()
        {
            // Arrange
            LogGroupDestinationsBuilder logGroupDestinationsBuilder = new LogGroupDestinationsBuilder(ld => null);
            var logDestination = new Mock<ILogDestination>().Object;

            // Act 
            logGroupDestinationsBuilder.CustomDestination(logDestination);

            // Assert   
            Assert.AreEqual(1, logGroupDestinationsBuilder.LogDestinations.Count);
            Assert.AreEqual(logDestination, logGroupDestinationsBuilder.LogDestinations[0]);
        }

        [Test]
        public void CustomDestination_Taking_LogDestination_When_Called_With_Null_LogDestination_Throws_ArgumentNullException()
        {
            // Arrange
            LogGroupDestinationsBuilder logGroupDestinationsBuilder = new LogGroupDestinationsBuilder(ld => null);

            // Assert  
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act 
                logGroupDestinationsBuilder.CustomDestination(null as ILogDestination);
            });
        }

        #endregion

        #region Tests for BuildLogGroup method

        [Test]
        public void BuildLogGroup_When_Called_Calls_BuildCallback_Passing_Log_Destinations_List_To_It()
        {
            // Arrange  
            List<ILogDestination> logDestinationsListPassedToBuildCallback = null;
            Func<List<ILogDestination>, ILoggerBuilder> buildCallback = ld => { logDestinationsListPassedToBuildCallback = ld; return null; };
            LogGroupDestinationsBuilder logGroupDestinationsBuilder = new LogGroupDestinationsBuilder(buildCallback);

            // Act
            logGroupDestinationsBuilder.CustomDestination(new Mock<ILogDestination>().Object);
            logGroupDestinationsBuilder.BuildLogGroup();

            // Assert
            Assert.AreEqual(logGroupDestinationsBuilder.LogDestinations, logDestinationsListPassedToBuildCallback);
        }

        [Test]
        public void BuildLogGroup_When_Called_Calls_BuildCallback_And_Returns_Its_Result()
        {
            // Arrange  
            var loggerBuilder = new Mock<ILoggerBuilder>().Object;
            Func<List<ILogDestination>, ILoggerBuilder> buildCallback = ld => loggerBuilder;
            LogGroupDestinationsBuilder logGroupDestinationsBuilder = new LogGroupDestinationsBuilder(buildCallback);

            // Act 
            var returnedLoggerBuilder = logGroupDestinationsBuilder.BuildLogGroup();

            // Assert
            Assert.AreEqual(loggerBuilder, returnedLoggerBuilder);
        }

        #endregion
    }
}
