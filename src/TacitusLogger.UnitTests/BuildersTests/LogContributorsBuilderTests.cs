using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TacitusLogger.Builders;
using TacitusLogger.Contributors;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class LogContributorsBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_Build_Callback()
        {
            //Arrange
            Func<IList<LogContributorBase>, ILoggerBuilder> buildCallback = list => null;

            //Act
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(buildCallback);

            //Assert
            Assert.AreEqual(buildCallback, logContributorsBuilder.BuildCallback);
        }
        [Test]
        public void Ctor_When_Called_Initializes_LogContributorsList_With_Empty_List()
        {
            //Act
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(list => null);

            //Assert
            Assert.NotNull(logContributorsBuilder.LogContributors);
            Assert.AreEqual(0, logContributorsBuilder.LogContributors.Count);
        }

        #endregion

        #region Tests for Custom method
        
        [Test]
        public void Custom_When_Called_With_Given_IsActive_Property_Changes_Status_And_Adds_Log_Contributor_To_Log_Contributors_List()
        {
            //Arrange
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(list => null);
            var logContributor = new Mock<LogContributorBase>("contributor1").Object;
            var isActive = new Mock<Setting<bool>>().Object;

            //Act
            logContributorsBuilder.Custom(logContributor, isActive);

            //Assert 
            Assert.AreEqual(isActive, logContributor.IsActive);
            Assert.AreEqual(1, logContributorsBuilder.LogContributors.Count);
            Assert.AreEqual(logContributor, logContributorsBuilder.LogContributors[0]);
        }
        [Test]
        public void Custom_When_Called_Several_Times_Adds_Log_Contributor_To_Log_Contributors_List()
        {
            //Arrange
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(list => null);
            var logContributor1 = new Mock<LogContributorBase>("contributor1").Object;
            var logContributor2 = new Mock<LogContributorBase>("contributor2").Object;
            Setting<bool> isActive = true;

            //Act
            logContributorsBuilder.Custom(logContributor1, isActive);
            logContributorsBuilder.Custom(logContributor2, isActive);

            //Assert 
            Assert.AreEqual(2, logContributorsBuilder.LogContributors.Count);
            Assert.AreEqual(logContributor1, logContributorsBuilder.LogContributors[0]);
            Assert.AreEqual(logContributor2, logContributorsBuilder.LogContributors[1]);
        }
        [Test]
        public void Custom_When_Called_Returns_LogContributorsBuilder()
        {
            // Arrange
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(list => null);
            Setting<bool> isActive = true;

            // Act
            var returnedLogContributorsBuilder = logContributorsBuilder.Custom(new Mock<LogContributorBase>("contributor1").Object, isActive);

            // Assert 
            Assert.AreEqual(logContributorsBuilder, returnedLogContributorsBuilder);
        }
        [Test]
        public void Custom_When_Called_With_Null_Log_Contributor_Throws_ArgumentNullException()
        {
            // Arrange
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(list => null);
            Setting<bool> isActive = true;

            // Assert
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                var returnedLogContributorsBuilder = logContributorsBuilder.Custom(null as LogContributorBase, isActive);
            });
        }

        #endregion

        #region Tests for BuildContributors method

        [Test]
        public void BuildContributors_When_Called_Calls_Build_Callback_And_Returns_It_Result()
        {
            // Arrange
            ILoggerBuilder loggerBuilder = new Mock<ILoggerBuilder>().Object;
            ILoggerBuilder loggerBuilderReturnedFromBuildCallback = new Mock<ILoggerBuilder>().Object;
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(list => loggerBuilderReturnedFromBuildCallback);

            // Act
            var returnedLoggerBuilder = logContributorsBuilder.BuildContributors();

            // Assert 
            Assert.AreEqual(loggerBuilderReturnedFromBuildCallback, returnedLoggerBuilder);
        }
        [Test]
        public void BuildContributors_When_Called_Sends_Log_Contributors_List_To_BuildCallback()
        {
            // Arrange
            IList<LogContributorBase> logContributors = null;
            ILoggerBuilder loggerBuilder = new Mock<ILoggerBuilder>().Object;
            LogContributorsBuilder logContributorsBuilder = new LogContributorsBuilder(list => { logContributors = list; return null; });
            var logContributor = new Mock<LogContributorBase>("contributor1").Object;
            logContributorsBuilder.Custom(logContributor, Setting<bool>.From.Variable(true));

            // Act
            logContributorsBuilder.BuildContributors();

            // Assert 
            Assert.NotNull(logContributors.Count);
            Assert.AreEqual(1, logContributors.Count);
            Assert.AreEqual(logContributor, logContributors[0]);
        }

        #endregion
    }
}
