using Moq;
using NUnit.Framework;
using System; 
using System.IO; 
using TacitusLogger.Builders;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Serializers; 

namespace TacitusLogger.IntegrationTests
{
    [TestFixture]
    public class LoggerBuilderTests
    { 
        [Test]
        public void Logger_When_Trying_To_Create_Two_Log_Groups_With_Same_Name_Throws_InvalidOperationException()
        {
            // Arrange 
            var loggerBuilder = LoggerBuilder.Logger().NewLogGroup("Group").ForRule(x => x.Context != null).Console().Add().BuildLogGroup()
                                                      .NewLogGroup("Group").ForRule(x => x.Context != null).Debug().Add().BuildLogGroup();

            // Assert
            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                loggerBuilder.BuildLogger();
            });
        }

        #region Tests related to TextWriter destination

        [Test]
        public void Logger_With_One_TextWriter_Destination_When_TextWriter_Is_Specified_Twice_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .TextWriter()
                                                              .WithWriter(x => new Mock<TextWriter>().Object)
                                                              .WithWriter(new Mock<TextWriter>().Object)
                                                              .Add()
                                                              .BuildLogger();
            });
        }

        [Test]
        public void Logger_With_One_TextWriter_Destination_When_LogSerializer_Is_Specified_Twice_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .TextWriter()
                                                              .WithJsonLogText()
                                                              .WithExtendedTemplateLogText()
                                                              .Add()
                                                              .BuildLogger();
            });
        }

        [Test]
        public void Logger_With_One_TextWriter_Destination_When_TextWriterObject_Is_Not_Specified_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .TextWriter()
                                                              .WithJsonLogText()
                                                              .Add()
                                                              .BuildLogger();
            });
        }

        #endregion

        #region Tests related to File destination

        [Test]
        public void Logger_With_One_File_Destination_When_FilePath_Is_Specified_Twice_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .File()
                                                              .WithPath(new Mock<ILogSerializer>().Object)
                                                              .WithPath("path2")
                                                              .Add()
                                                              .BuildLogger();
            });
        }

        [Test]
        public void Logger_With_One_File_Destination_When_LogSerializer_Is_Specified_Twice_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .File()
                                                              .WithJsonLogText()
                                                              .WithExtendedTemplateLogText()
                                                              .Add()
                                                              .BuildLogger();
            });
        }

        #endregion

        #region Tests related to Console destination

        [Test]
        public void Logger_With_One_Console_Destination_When_LogSerializer_Is_Specified_Twice_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .Console()
                                                              .WithJsonLogText()
                                                              .WithExtendedTemplateLogText()
                                                              .Add()
                                                              .BuildLogger();
            });
        }

        #endregion

        #region Tests related to Debug destination

        [Test]
        public void Logger_With_One_Debug_Destination_When_LogSerializer_Is_Specified_Twice_Throws_InvalidOperationException()
        {
            //Build log 
            Assert.Catch<InvalidOperationException>(() =>
            {
                Logger logger = (Logger)LoggerBuilder.Logger().ForAllLogs()
                                                              .Debug()
                                                              .WithJsonLogText()
                                                              .WithExtendedTemplateLogText()
                                                              .Add()
                                                              .BuildLogger();
            });
        }

        #endregion

        #region Tests related to WithLogLevel method


        [Test]
        public void LoggerBuilder_When_Log_Level_Is_Set_Then_Log_Level_Is_Set_To_Logger()
        {
            // Arrange
            var logLevel = Setting<LogLevel>.From.Variable(LogLevel.Info);

            // Act
            Logger logger = LoggerBuilder.Logger().WithLogLevel(logLevel).BuildLogger();

            // Assert
            Assert.AreEqual(logLevel, logger.LogLevel);
        }

        [Test]
        public void LoggerBuilder_When_Log_Level_Is_Set_Then_Log_Level_Is_Set_To_Logger2()
        {
            // Act
            Logger logger = LoggerBuilder.Logger().WithLogLevel(LogLevel.Info).BuildLogger();

            // Assert
            Assert.IsInstanceOf<Setting<LogLevel>>(logger.LogLevel);
            Assert.AreEqual(LogLevel.Info, logger.LogLevel.Value);
        }

        #endregion

        #region Tests related to log ID generator

        [Test]
        public void LoggerBuilder_When_Log_Id_Generator_Is_Set_Then_Logger_Is_Built_With_This_Log_Id_Generator()
        {
            // Arrange
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithLogIdGenerator(logIdGenerator).BuildLogger();

            // Assert 
            Assert.AreEqual(logIdGenerator, logger.LogIdGenerator);
        }

        [Test]
        public void LoggerBuilder_When_Guid_Id_Generator_Is_Set_Then_Logger_Is_Built_With_This_Log_Id_Generator()
        {
            // Arrange
            var logIdGenerator = new Mock<ILogIdGenerator>().Object;

            // Act
            Logger logger = LoggerBuilder.Logger().WithGuidLogId("D", 4).BuildLogger();

            // Assert 
            Assert.IsInstanceOf<GuidLogIdGenerator>(logger.LogIdGenerator);
            Assert.AreEqual("D", (logger.LogIdGenerator as GuidLogIdGenerator).GuidFormat);
            Assert.AreEqual(4, (logger.LogIdGenerator as GuidLogIdGenerator).SubstringLength);
        }

        #endregion

    }
}
