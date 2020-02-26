using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.UnitTests
{
    [TestFixture]
    public class LogBuilderExtensionsForILoggerTests
    {
        [Test]
        public void Success_When_Called_Creates_And_Returns_New_Log_Builder_With_Specified_Logger_Description_And_Success_LogType()
        {
            ILogger logger = new Mock<ILogger>().Object;
            string description = "description1";

            var logBuilder = (LogBuilder)LogBuilderExtensionsForILogger.Success(logger, description);

            // Assert 
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.AreEqual(LogType.Success, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
        }
        [Test]
        public void Info_When_Called_Creates_And_Returns_New_Log_Builder_With_Specified_Logger_Description_And_Info_LogType()
        {
            ILogger logger = new Mock<ILogger>().Object;
            string description = "description1";

            var logBuilder = (LogBuilder)LogBuilderExtensionsForILogger.Info(logger, description);

            // Assert 
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.AreEqual(LogType.Info, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
        }
        [Test]
        public void Event_When_Called_Creates_And_Returns_New_Log_Builder_With_Specified_Logger_Description_And_Event_LogType()
        {
            ILogger logger = new Mock<ILogger>().Object;
            string description = "description1";

            var logBuilder = (LogBuilder)LogBuilderExtensionsForILogger.Event(logger, description);

            // Assert 
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.AreEqual(LogType.Event, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
        }
        [Test]
        public void Warning_When_Called_Creates_And_Returns_New_Log_Builder_With_Specified_Logger_Description_And_Warning_LogType()
        {
            ILogger logger = new Mock<ILogger>().Object;
            string description = "description1";

            var logBuilder = (LogBuilder)LogBuilderExtensionsForILogger.Warning(logger, description);

            // Assert 
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.AreEqual(LogType.Warning, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
        }
        [Test]
        public void Failure_When_Called_Creates_And_Returns_New_Log_Builder_With_Specified_Logger_Description_And_Failure_LogType()
        {
            ILogger logger = new Mock<ILogger>().Object;
            string description = "description1";

            var logBuilder = (LogBuilder)LogBuilderExtensionsForILogger.Failure(logger, description);

            // Assert 
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.AreEqual(LogType.Failure, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
        }
        [Test]
        public void Error_When_Called_Creates_And_Returns_New_Log_Builder_With_Specified_Logger_Description_And_Error_LogType()
        {
            ILogger logger = new Mock<ILogger>().Object;
            string description = "description1";

            var logBuilder = (LogBuilder)LogBuilderExtensionsForILogger.Error(logger, description);

            // Assert 
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.AreEqual(LogType.Error, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
        }
        [Test]
        public void Critical_When_Called_Creates_And_Returns_New_Log_Builder_With_Specified_Logger_Description_And_Critical_LogType()
        {
            ILogger logger = new Mock<ILogger>().Object;
            string description = "description1";

            var logBuilder = (LogBuilder)LogBuilderExtensionsForILogger.Critical(logger, description);

            // Assert 
            Assert.AreEqual(logger, logBuilder.Logger);
            Assert.AreEqual(LogType.Critical, logBuilder.BuiltLog.Type);
            Assert.AreEqual(description, logBuilder.BuiltLog.Description);
        }
    }
}
