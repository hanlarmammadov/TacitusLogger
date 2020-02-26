using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Builders;

namespace TacitusLogger.UnitTests.BuildersTests
{
    [TestFixture]
    public class CustomizedLoggerBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_LoggerName_PreBuildAction_And_PostBuildAction()
        {
            // Arrange
            string loggerName = "logger1";
            Action preBuildAction = () => { };
            Action<Logger> postBuildAction = (lg) => { };

            // Act
            CustomizedLoggerBuilder customizedLoggerBuilder = new CustomizedLoggerBuilder(loggerName, preBuildAction, postBuildAction);

            // Assert
            Assert.AreEqual(loggerName, customizedLoggerBuilder.LoggerName);
            Assert.AreEqual(preBuildAction, customizedLoggerBuilder.PreBuildActionDelegate);
            Assert.AreEqual(postBuildAction, customizedLoggerBuilder.PostBuildActionDelegate);
        }

        [Test]
        public void Ctor_When_Called_Without_PreBuildAction_And_PostBuildAction_Sets_Them_As_Null()
        {
            // Act
            CustomizedLoggerBuilder customizedLoggerBuilder = new CustomizedLoggerBuilder("logger1");

            // Assert 
            Assert.IsNull(customizedLoggerBuilder.PreBuildActionDelegate);
            Assert.IsNull(customizedLoggerBuilder.PostBuildActionDelegate);
        }

        #endregion

        #region Tests for BuildLogger method

        [Test]
        public void BuildLogger_When_Called_Calls_PreBuildAction_And_PostBuildAction()
        {
            // Arrange 
            // Prebuild action.
            int preBuildActionCalledTimes = 0;
            Action preBuildAction = () => { preBuildActionCalledTimes++; };
            // Postbuild action.
            int postBuildActionCalledTimes = 0;
            Logger loggerPassedToPostBuildAction = null;
            Action<Logger> postBuildAction = (lg) => { loggerPassedToPostBuildAction = lg; postBuildActionCalledTimes++; };
            // Logger builder.
            CustomizedLoggerBuilder customizedLoggerBuilder = new CustomizedLoggerBuilder("logger1", preBuildAction, postBuildAction);

            // Act
            var loggerBuilded = customizedLoggerBuilder.BuildLogger();

            // Assert
            Assert.AreEqual(1, preBuildActionCalledTimes);
            Assert.AreEqual(1, postBuildActionCalledTimes);
            Assert.AreEqual(loggerBuilded, loggerPassedToPostBuildAction);
        }

        [Test]
        public void BuildLogger_When_Called_Given_That_Logger_Builder_Created_Without_PreBuildAction_And_PostBuildAction_Does_Not_Throw()
        {
            // Arrange  
            CustomizedLoggerBuilder customizedLoggerBuilder = new CustomizedLoggerBuilder("logger1");

            // Act
            var loggerBuilded = customizedLoggerBuilder.BuildLogger();

            // Assert 
            Assert.NotNull(loggerBuilded);
        }

        #endregion
    }
}